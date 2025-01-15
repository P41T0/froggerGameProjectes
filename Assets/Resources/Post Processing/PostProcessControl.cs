using UnityEngine.Rendering.PostProcessing;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


//!\\ Install Post Processing Package from:
// Window → Package Manager, Set [Unity Registry] on the right dropdown, search for Post Processing → Install


public class PostProcessControl : MonoBehaviour
{
    [UnityEngine.Min(0)] public float transitionDuration = 0.2f;
    [UnityEngine.Min(0)] public static float s_transitionDuration = 0.2f;
    [Range(0, 1)] public float effectWeight = 1f;
    [Range(0, 1)] public static float s_effectWeight = 1f;

    public static bool HasGameObject { get; private set; }
    static PostProcessVolume volume;

    void Awake()
    {
        if (typeof(PostProcessLayer).ToString() != "UnityEngine.Rendering.PostProcessing.PostProcessLayer")
            Debug.LogError("Post Processing package is not installed.\nInstall it from: Window → Package Manager, Set [Unity Registry] on the right dropdown, search for Post Processing → Install");

        if (HasGameObject) { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject); HasGameObject = true;
        if (TryGetComponent<PostProcessVolume>(out var v)) volume = v;
        else { Debug.LogWarning($"PostProcess component not found", gameObject); return; }

        // s_effectWeight = effectWeight;
        s_effectWeight = PlayerPrefs.GetFloat("PostProcessingLevel", 1);
        s_transitionDuration = transitionDuration;

        bool enabled = PlayerPrefs.GetInt("PostProcessingEnabled", 1) == 1;
        volume.weight = enabled ? effectWeight : 0f;
    }

    public void SetupCamera()
    {
        gameObject.layer = LayerMask.NameToLayer("TransparentFX");

        if (Camera.allCameras.Length > 1) { Debug.LogWarning("More than one camera found in the scene."); return; }
        if (!Camera.main.TryGetComponent<PostProcessLayer>(out var postProcessLayer))
            postProcessLayer = Camera.main.gameObject.AddComponent<PostProcessLayer>();

        postProcessLayer.volumeTrigger = null;
        postProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
        postProcessLayer.volumeLayer = 1 << 1;
        postProcessLayer.stopNaNPropagation = false;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) Debug.LogWarning("Overlay Canvas won't have Post-Processing effects", canvas.gameObject);
    }

    public static void ChangeLevel(float value)
    {
        if (volume == null) { Debug.LogWarning($"Volume not found"); return; }
        s_effectWeight = value;
        PlayerPrefs.SetFloat("PostProcessingLevel", s_effectWeight);
        volume.weight = s_effectWeight;
    }

    public static void Enable(bool enabled)
    {
        if (volume == null) { Debug.LogWarning($"PostProcess gameObject not found in the current scene"); return; }

        PlayerPrefs.SetInt("PostProcessingEnabled", enabled ? 1 : 0);
        float targetWeight = enabled ? s_effectWeight : 0f;
        volume.StartCoroutine(TransitionPostProcessing(targetWeight));
    }

    static IEnumerator TransitionPostProcessing(float targetWeight)
    {
        if (volume == null) yield return null;

        float startWeight = volume.weight;
        float elapsedTime = 0f;

        while (elapsedTime < s_transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            volume.weight = Mathf.Lerp(startWeight, targetWeight, elapsedTime / s_transitionDuration);
            yield return null;
        }
        volume.weight = targetWeight;
    }
}


#if UNITY_EDITOR 
[CustomEditor(typeof(PostProcessControl))]
public class PostProcessingControlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PostProcessControl code = (PostProcessControl)target;
        DrawDefaultInspector();

        GUILayout.BeginVertical("Box");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Enable")) PostProcessControl.Enable(true);
        if (GUILayout.Button("Disable")) PostProcessControl.Enable(false);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Setup Camera")) code.SetupCamera();
        GUILayout.EndVertical();
    }
}
#endif