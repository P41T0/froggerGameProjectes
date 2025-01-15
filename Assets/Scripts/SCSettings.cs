using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SCSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioEffectsMixer;
    [SerializeField] private AudioMixer audioMusicMixer;
    private bool optionChanged;
    private Color unselected;
    private GameObject postpross;
    private float effectsVolume;
    private float realEffectsVolume;
    private float musicVolume;
    private float realMusicVolume;
    private int numOptions;
    private int optionSelected;
    private bool postProcessingEnabled;
    private float postProcessingPerc;
    private float postProssFloat;
    [SerializeField] private TMP_Text musicControlText;
    [SerializeField] private TMP_Text volumeControlText;
    [SerializeField] private TMP_Text postProcessingText;
    [SerializeField] private TMP_Text postProcessingPercText;
    // Start is called before the first frame update
    void Start()
    {
        optionChanged = true;
        unselected = new Color(0.5f, 0.5f, 0.5f);
        if (GameObject.FindGameObjectWithTag("postprocessing") == true)
        {
            postpross = GameObject.FindGameObjectWithTag("postprocessing");
        }
        if (PlayerPrefs.HasKey("PostProcessing"))
        {
            if (PlayerPrefs.GetInt("PostProcessing") == 0)
            {
                postProcessingEnabled = false;
                postProcessingText.text = "Retro effect off";

            }
            else if (PlayerPrefs.GetInt("PostProcessing") == 1)
            {
                postProcessingEnabled = true;
                postProcessingText.text = "Retro effect on";

            }

        }
        else
        {
            postProcessingEnabled = true;
            postProcessingText.text = "Retro effect on";
        }

        if (postpross != null)
        {

            PostProcessControl.Enable(postProcessingEnabled);
        }

        if (PlayerPrefs.HasKey("PostProcessingPerc"))
        {
            postProssFloat = PlayerPrefs.GetFloat("PostProcessingPerc");

        }
        else
        {
            postProssFloat = 0.4f;
        }
        postProcessingPerc = postProssFloat * 100;
        if (postpross != null)
        {
            if (postProcessingEnabled)
            {
                PostProcessControl.ChangeLevel(postProssFloat);
            }

        }
        postProcessingPercText.text = "Effect level (" + (Mathf.RoundToInt(postProcessingPerc)).ToString("D3") + "%) ";
        for (int i = 0; i < (Mathf.Round(postProcessingPerc) / 10); i++)
        {
            postProcessingPercText.text += " |";
        }
        numOptions = 4;
        optionSelected = 0;
        if (PlayerPrefs.HasKey("audioEffectsMixerVolume"))
        {
            effectsVolume = PlayerPrefs.GetFloat("audioEffectsMixerVolume");
        }
        else
        {
            effectsVolume = 0.8f;
        }
        realEffectsVolume = effectsVolume * 100;
        audioEffectsMixer.SetFloat("volumEfectes", Mathf.Log10(effectsVolume) * 20.0f);
        volumeControlText.text = "Effects volume (" + (Mathf.RoundToInt(realEffectsVolume)).ToString("D3") + "%) ";
        for (int i = 0; i <= (Mathf.Round(realEffectsVolume)) / 10; i++)
        {
            volumeControlText.text += " |";
        }



        if (PlayerPrefs.HasKey("audioMusicMixerVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("audioMusicMixerVolume");
            realMusicVolume = musicVolume * 100;

            audioMusicMixer.SetFloat("volumMusica", Mathf.Log10(musicVolume) * 20.0f);
            for (int i = 0; i < (Mathf.Round(realMusicVolume)) / 10; i++)
            {
                musicControlText.text += " |";
            }
        }
        else
        {
            musicVolume = 0.8f;
        }
        realMusicVolume = musicVolume * 100;
        audioMusicMixer.SetFloat("volumMusica", Mathf.Log10(musicVolume) * 20.0f);
        musicControlText.text = "Music volume (" + (Mathf.RoundToInt(realMusicVolume)).ToString("D3") + "%) ";
        for (int i = 0; i <= (Mathf.Round(realMusicVolume)) / 10; i++)
        {
            musicControlText.text += " |";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (optionChanged == true)
        {
            optionChanged = false;
            if (optionSelected == 0)
            {
                musicControlText.color = unselected;
                volumeControlText.color = Color.white;
                postProcessingText.color = unselected;
                postProcessingPercText.color = unselected;
            }
            else if (optionSelected == 1)
            {
                musicControlText.color = unselected;
                volumeControlText.color = unselected;
                postProcessingText.color = Color.white;
                postProcessingPercText.color = unselected;
            }
            else if (optionSelected == 2)
            {
                musicControlText.color = unselected;
                volumeControlText.color = unselected;
                postProcessingText.color = unselected;
                postProcessingPercText.color = Color.white;

            }
            else if (optionSelected == 3)
            {
                musicControlText.color = Color.white;
                volumeControlText.color = unselected;
                postProcessingText.color = unselected;
                postProcessingPercText.color = unselected;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Arcade.ac.ButtonDown("la") || Arcade.ac.ButtonDown("j1_Up"))
        {
            optionSelected--;
            optionChanged = true;
            if (optionSelected < 0)
            {
                optionSelected = numOptions - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Arcade.ac.ButtonDown("lb") || Arcade.ac.ButtonDown("j1_Down"))
        {
            optionSelected++;
            optionChanged = true;
            if (optionSelected >= numOptions)
            {
                optionSelected = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Arcade.ac.ButtonDown("rb") || Arcade.ac.ButtonDown("j1_Right"))
        {
            if (optionSelected == 0)
            {
                if (effectsVolume < 1.0f)
                {
                    effectsVolume += 0.05f;
                    realEffectsVolume += 5;
                    if (effectsVolume > 1.0f)
                    {
                        effectsVolume = 1;
                        realEffectsVolume = 100;
                    }

                    PlayerPrefs.SetFloat("audioEffectsMixerVolume", effectsVolume);
                }
                volumeControlText.text = "Effects volume (" + ((Mathf.RoundToInt(realEffectsVolume)).ToString("D3") + "%) ");
                for (int i = 0; i < (Mathf.Round(realEffectsVolume)) / 10; i++)
                {
                    volumeControlText.text += " |";
                }
                audioEffectsMixer.SetFloat("volumEfectes", Mathf.Log10(effectsVolume) * 20.0f);
            }
            if (optionSelected == 3)
            {
                if (musicVolume < 1.0f)
                {
                    musicVolume += 0.05f;
                    realMusicVolume += 5;
                    if (musicVolume > 1.0f)
                    {
                        musicVolume = 1;
                        realMusicVolume = 100;
                    }

                    PlayerPrefs.SetFloat("audioMusicMixerVolume", musicVolume);
                }
                musicControlText.text = "Music volume (" + ((Mathf.RoundToInt(realMusicVolume)).ToString("D3") + "%) ");
                for (int i = 0; i < (Mathf.Round(realMusicVolume)) / 10; i++)
                {
                    musicControlText.text += " |";
                }
                audioMusicMixer.SetFloat("volumMusica", Mathf.Log10(musicVolume) * 20.0f);
            }
            else if (optionSelected == 1)
            {
                if (postProcessingEnabled == false)
                {
                    PlayerPrefs.SetInt("Postprocessing", 1);
                    postProcessingEnabled = true;
                    PlayerPrefs.SetInt("PostProcessing", 1);
                    postProcessingText.text = "Retro effect on";
                    if (postpross != null)
                    {

                        PostProcessControl.Enable(postProcessingEnabled);
                        PostProcessControl.ChangeLevel(postProssFloat);
                    }


                }
            }
            else if (optionSelected == 2)
            {
                if (postProcessingPerc < 100)
                {
                    postProcessingPerc += 5;
                    postProssFloat += 0.05f;
                    if (postProcessingPerc > 100)
                    {
                        postProssFloat = 1f;
                        postProcessingPerc = 100;
                    }
                }
                if (postpross != null)
                {
                    if (postProcessingEnabled)
                    {
                        PostProcessControl.ChangeLevel(postProssFloat);
                    }

                }
                postProcessingPercText.text = "Effect level (" + (Mathf.RoundToInt(postProcessingPerc)).ToString("D3") + "%) ";
                for (int i = 0; i < (Mathf.Round(postProcessingPerc) / 10); i++)
                {
                    postProcessingPercText.text += " |";
                }
                PlayerPrefs.SetFloat("PostProcessingPerc", postProssFloat);
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Arcade.ac.ButtonDown("lb") || Arcade.ac.ButtonDown("j1_Left"))
        {

            if (optionSelected == 0)
            {


                if (effectsVolume > 0.0f)
                {

                    effectsVolume -= 0.05f;
                    realEffectsVolume -= 5;
                    if (effectsVolume <= 0)
                    {
                        effectsVolume = 0.00001f;
                        realEffectsVolume = 0;
                    }
                    PlayerPrefs.SetFloat("audioEffectsMixerVolume", effectsVolume);
                }
                volumeControlText.text = "Effects volume (" + (Mathf.RoundToInt(realEffectsVolume)).ToString("D3") + "%) ";

                for (int i = 0; i < (Mathf.Round(realEffectsVolume)) / 10; i++)
                {
                    volumeControlText.text += " |";
                }
                audioEffectsMixer.SetFloat("volumEfectes", Mathf.Log10(effectsVolume) * 20.0f);
            }
            if (optionSelected == 3)
            {


                if (musicVolume > 0.0f)
                {

                    musicVolume -= 0.05f;
                    realMusicVolume -= 5;
                    if (musicVolume <= 0)
                    {
                        musicVolume = 0.00001f;
                        realMusicVolume = 0;
                    }
                    PlayerPrefs.SetFloat("audioMusicMixerVolume", musicVolume);
                }
                musicControlText.text = "Music volume (" + (Mathf.RoundToInt(realMusicVolume)).ToString("D3") + "%) ";

                for (int i = 0; i < (Mathf.Round(realMusicVolume)) / 10; i++)
                {
                    musicControlText.text += " |";
                }
                audioMusicMixer.SetFloat("volumMusica", Mathf.Log10(musicVolume) * 20.0f);
            }

            else if (optionSelected == 1)
            {
                if (postProcessingEnabled == true)
                {
                    PlayerPrefs.SetInt("Postprocessing", 0);
                    postProcessingEnabled = false;
                    postProcessingText.text = "Retro effect off";
                    PlayerPrefs.SetInt("PostProcessing", 0);
                    if (postpross != null)
                    {

                        PostProcessControl.Enable(postProcessingEnabled);
                    }

                }
            }
            else if (optionSelected == 2)
            {
                if (postProcessingPerc > 0)
                {
                    postProcessingPerc -= 5;
                    postProssFloat -= 0.05f;
                    if (postProcessingPerc < 0)
                    {
                        postProcessingPerc = 0;
                        postProssFloat = 0f;
                    }
                }
                postProcessingPercText.text = "Effect level (" + (Mathf.RoundToInt(postProcessingPerc)).ToString("D3") + "%) ";
                for (int i = 0; i < (Mathf.Round(postProcessingPerc) / 10); i++)
                {
                    postProcessingPercText.text += " |";
                }
                if (postpross != null)
                {
                    if (postProcessingEnabled)
                    {
                        PostProcessControl.ChangeLevel(postProssFloat);
                    }

                }
                PlayerPrefs.SetFloat("PostProcessingPerc", postProssFloat);

            }
        }
        if (Input.GetKeyDown(KeyCode.Return) || Arcade.ac.ButtonDown("l1") || Arcade.ac.ButtonDown("select"))
        {
            ReturnToIntro();
        }

    }
    public void ReturnToIntro()
    {
        SceneManager.LoadScene("IntroScene");
    }
}
