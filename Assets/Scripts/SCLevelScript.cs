using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class SCLevelScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject bonificationItem;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private int numFrogs;
    [SerializeField] private AudioSource levelSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip warning;
    [SerializeField] private AudioClip frogUnlocked;
    [SerializeField] private AudioClip levelCleared;
    private int numFrogsUnlocked;
    [SerializeField] private GameObject[] VictoryFrogsPositions;
    private List<int> VictoryFrogsNumList;
    private int randomBonificationPosition;
    private float bonificationItemWaitTime;
    private bool bonificationAppear;
    private int score;
    private int lives;
    [SerializeField] private int level;
    private float effectLenght;
    private bool effectPlaying;
    private int numLevels;
    [SerializeField] private GameObject[] livesImages;
    private float time;

    void Start()
    {
        effectPlaying = false;
        effectLenght = -1;
        if (PlayerPrefs.HasKey("levelCleared"))
        {
            if (PlayerPrefs.GetInt("levelCleared") == 1)
            {
                musicSource.Stop();
                effectPlaying = true;
                effectLenght = levelCleared.length;
                levelSource.clip = levelCleared;
                levelSource.Play();
            }
        }
        VictoryFrogsNumList = new List<int>();
        for (int i = 0; i < VictoryFrogsPositions.Length; i++)
        {
            VictoryFrogsNumList.Add(i);
            VictoryFrogsPositions[i].GetComponent<VictoryFrogsScript>().SetFrogPos(i);
        }
        bonificationAppear = false;
        numLevels = 4;

        lives = PlayerPrefs.GetInt("PlayerLives");
        for (int i = 0; i < lives; i++)
        {
            livesImages[i].SetActive(true);
        }
        score = PlayerPrefs.GetInt("PlayerScore");
        time = PlayerPrefs.GetFloat("PlayerTime");
        numFrogsUnlocked = 0;
        scoreText.text = score.ToString("D5");
        timeText.text = time.ToString();
        bonificationItemWaitTime = UnityEngine.Random.Range(5f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (effectPlaying)
        {
            effectLenght -= Time.deltaTime;
            if (effectLenght < 0)
            {
                musicSource.Play();
                musicSource.loop = true;
                effectPlaying = false;
            }
        }

        time -= Time.deltaTime;
        timeText.text = "Time: " + time.ToString("f0");
        if (time < 0)
        {
            PlayerPrefs.SetInt("PlayerScore", score);
            PlayerPrefs.SetInt("TimeUp", 1);
            SceneManager.LoadScene("GameOverScene");
        }
        if (bonificationAppear == false)
        {
            bonificationItemWaitTime -= Time.deltaTime;
            if (bonificationItemWaitTime < 0)
            {
                if (VictoryFrogsNumList != null)
                {
                    randomBonificationPosition = Mathf.RoundToInt(UnityEngine.Random.Range(0, VictoryFrogsNumList.Count));
                    if (VictoryFrogsPositions[randomBonificationPosition].GetComponent<VictoryFrogsScript>().GetIsUnlocked() == false)
                    {
                        VictoryFrogsPositions[randomBonificationPosition].GetComponent<VictoryFrogsScript>().GiveFrogBonification();
                        bonificationAppear = true;
                    }

                }


            }

        }
    }
    public void IncScore()
    {
        score += 10;
        scoreText.text = score.ToString("D5");
    }
    public void VictoryFrogUnlocked(int posFrog, bool bonification)
    {
        if (bonification == true)
        {
            score += 90;
            BonificationItemDestroyed();
        }
        else
        {
            score += 40;
        }
        scoreText.text = score.ToString("D5");
        numFrogsUnlocked++;
        if (numFrogsUnlocked >= numFrogs)
        {
            bool sceneSelected = false;
            int randomScene = 0;
            while (!sceneSelected)
            {
                randomScene = UnityEngine.Random.Range(0, numLevels);
                if (randomScene != level)
                {
                    time += 5.0f;
                    if (time <= 20.0f)
                    {
                        time = 20.0f;
                    }
                    else
                    {
                        time += 5.0f;
                    }
                    PlayerPrefs.SetInt("levelCleared", 1);
                    PlayerPrefs.SetInt("PlayerLives", lives);
                    PlayerPrefs.SetInt("PlayerScore", score);
                    PlayerPrefs.SetFloat("PlayerTime", time);
                    if (randomScene == 0)
                    {
                        SceneManager.LoadScene("level1");
                    }
                    else if (randomScene == 1)
                    {
                        SceneManager.LoadScene("level2");
                    }
                    else if (randomScene == 2)
                    {
                        SceneManager.LoadScene("level3");
                    }
                    else if (randomScene == 3)
                    {
                        SceneManager.LoadScene("level4");
                    }
                    sceneSelected = true;
                }
            }
        }
        else
        {
            musicSource.Pause();
            levelSource.Stop();
            levelSource.clip = frogUnlocked;
            effectLenght = frogUnlocked.length;
            effectPlaying = true;
            levelSource.Play();
            time += 5.0f;
            if (VictoryFrogsNumList.Contains(posFrog))
            {
                VictoryFrogsNumList.Remove(posFrog);
            }
        }
    }
    public void ReduceLife()
    {
        time += 1.0f;
        lives -= 1;
        if (lives == 0)
        {
            PlayerPrefs.SetInt("PlayerScore", score);
            PlayerPrefs.SetInt("TimeUp", 0);
            SceneManager.LoadScene("GameOverScene");
        }
        else
        {

            livesImages[lives].SetActive(false);
        }
    }
    public void BonificationItemDestroyed()
    {
        bonificationItemWaitTime = UnityEngine.Random.Range(5, 10);
        bonificationAppear = false;
    }
}
