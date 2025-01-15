using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Unity.Mathematics;
using UnityEngine.UI;

public class IntroSCScript : MonoBehaviour
{
    private string portText;
    private int buttonSelected;
    private int numButtons;
    private float timer;
    private GameObject postProcess;
    private PostProcessControl control;
    private Color unselected;
    private bool optionChanged;
    [SerializeField] private TMP_Text optionsText;
    [SerializeField] private TMP_Text PlayText;
    [SerializeField] private TMP_Text ExitText;
    [SerializeField] private TMP_Text portDisplayText;
    [SerializeField] private AudioMixer audioEffectsMixer;
    [SerializeField] private AudioMixer audioMusicMixer;

    // Start is called before the first frame update
    void Start()
    {
        optionChanged = true;
        unselected = new Color(0.5f, 0.5f, 0.5f);
        timer = 0.0f;
        buttonSelected = 1;
        numButtons = 3;
        if (PlayerPrefs.HasKey("audioEffectsMixerVolume"))
        {
            audioEffectsMixer.SetFloat("volumEfectes", Mathf.Log10(PlayerPrefs.GetFloat("audioEffectsMixerVolume")) * 20.0f);
        }
        else
        {
            audioEffectsMixer.SetFloat("volumEfectes", Mathf.Log10(0.8f) * 20.0f);
            PlayerPrefs.SetFloat("audioEffectsMixerVolume", 0.8f);
        }
        if (PlayerPrefs.HasKey("audioMusicMixerVolume"))
        {
            audioMusicMixer.SetFloat("volumMusica", Mathf.Log10(PlayerPrefs.GetFloat("audioMusicMixerVolume")) * 20.0f);
        }
        else
        {
            audioMusicMixer.SetFloat("volumMusica", Mathf.Log10(0.8f) * 20.0f);
            PlayerPrefs.SetFloat("audioMusicMixerVolume", 0.8f);
        }

        if (PlayerPrefs.HasKey("PostProcessing"))
        {
            if (PlayerPrefs.GetInt("Postprocessing") == 1)
            {
                if (PlayerPrefs.HasKey("PostProcessingPerc"))
                {

                    PostProcessControl.ChangeLevel(PlayerPrefs.GetFloat("PostProcessingPerc"));
                    PostProcessControl.Enable(true);
                }
                else
                {
                    PostProcessControl.Enable(true);
                    PostProcessControl.ChangeLevel(0.4f);
                    PlayerPrefs.SetFloat("PostProcessingPerc", 0.4f);
                }


            }
            else if (PlayerPrefs.GetInt("Postprocessing") == 0)
            {
                PostProcessControl.Enable(false);
            }

        }
        else
        {

            PostProcessControl.ChangeLevel(0.4f);
            PostProcessControl.Enable(true);
            PlayerPrefs.SetInt("Postprocessing", 1);
            PlayerPrefs.SetFloat("PostProcessingPerc", 0.4f);
        }
        portText = Arcade.ac.GetPort();
        portDisplayText.text = portText;
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            timer = 0.5f;
            if (buttonSelected == 0)
            {
                if (optionChanged == true)
                {
                    optionChanged = false;
                    if (!PlayText.isActiveAndEnabled)
                    {
                        PlayText.enabled = true;
                    }
                    if (!ExitText.isActiveAndEnabled)
                    {
                        ExitText.enabled = true;
                    }
                    optionsText.color = Color.white;
                    PlayText.color = unselected;
                    ExitText.color = unselected;
                }
                if (optionsText.isActiveAndEnabled)
                {
                    optionsText.enabled = false;

                }
                else
                {
                    optionsText.enabled = true;
                }

            }
            else if (buttonSelected == 1)
            {
                if (optionChanged == true)
                {
                    optionChanged = false;
                    if (!optionsText.isActiveAndEnabled)
                    {
                        optionsText.enabled = true;
                    }
                    if (!ExitText.isActiveAndEnabled)
                    {
                        ExitText.enabled = true;
                    }
                    optionsText.color = unselected;
                    PlayText.color = Color.white;
                    ExitText.color = unselected;
                }
                if (PlayText.isActiveAndEnabled)
                {
                    PlayText.enabled = false;

                }
                else
                {
                    PlayText.enabled = true;
                }

            }
            else if (buttonSelected == 2)
            {
                if (optionChanged == true)
                {
                    optionChanged = false;
                    if (!PlayText.isActiveAndEnabled)
                    {
                        PlayText.enabled = true;
                    }
                    if (!optionsText.isActiveAndEnabled)
                    {
                        optionsText.enabled = true;
                    }
                    optionsText.color = unselected;
                    PlayText.color = unselected;
                    ExitText.color = Color.white;
                }
                if (ExitText.isActiveAndEnabled)
                {
                    ExitText.enabled = false;

                }
                else
                {
                    ExitText.enabled = true;
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Arcade.ac.ButtonDown("lb") || Arcade.ac.ButtonDown("j1_Down"))
        {
            buttonSelected++;
            optionChanged = true;
            if (buttonSelected >= numButtons)
            {
                buttonSelected = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Arcade.ac.ButtonDown("la") || Arcade.ac.ButtonDown("j1_Up"))
        {
            buttonSelected--;
            optionChanged = true;
            if (buttonSelected < 0)
            {
                buttonSelected = numButtons - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) || Arcade.ac.ButtonDown("l1") || Arcade.ac.ButtonDown("select"))
        {
            if (buttonSelected == 0)
            {
                ButtonSettingsPressed();
            }
            else if (buttonSelected == 1)
            {
                ButtonStartPressed();
            }
            else if (buttonSelected == 2)
            {
                ButtonQuitPressed();
            }
        }
        if (Arcade.ac.ButtonDown("start"))
        {
            ButtonStartPressed();
        }

    }
    public void ButtonStartPressed()
    {
        PlayerPrefs.SetInt("PlayerLives", 3);
        PlayerPrefs.SetInt("PlayerScore", 0);
        PlayerPrefs.SetFloat("PlayerTime", 130f);
        PlayerPrefs.SetInt("levelCleared", 0);
        SceneManager.LoadScene("level1");
    }
    public void ButtonSettingsPressed()
    {
        SceneManager.LoadScene("SettingsScene");
    }
    public void ButtonQuitPressed()
    {
        Application.Quit();
    }
}
