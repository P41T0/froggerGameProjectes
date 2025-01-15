using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSCScript : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource GOSource;
    [SerializeField] private AudioClip game_over;
    [SerializeField] private AudioClip game_over_music;
    private float game_over_lenght;
    private bool game_over_finished;
    [SerializeField] private TMP_Text PlayerScoreText;
    [SerializeField] private TMP_Text bestPlayerText;
    [SerializeField] private TMP_Text secondPlayerText;
    [SerializeField] private TMP_Text thirdPlayerText;
    [SerializeField] private TMP_Text bestScoreText;
    [SerializeField] private TMP_Text secondScoreText;
    [SerializeField] private TMP_Text thirdScoreText;
    [SerializeField] private TMP_Text playerNameLetter1;
    [SerializeField] private TMP_Text playerNameLetter2;
    [SerializeField] private TMP_Text playerNameLetter3;
    [SerializeField] private TMP_Text playerNameLetter4;
    [SerializeField] private GameObject confirmText;
    [SerializeField] private GameObject highScore;
    [SerializeField] private GameObject setName;
    [SerializeField] private int numBoto;
    private int numBotons;
    private float timerBotons;
    [SerializeField] private TMP_Text quitButtonText;
    [SerializeField] private TMP_Text replayButtonText;
    [SerializeField] private TMP_Text gameOverText;
    private String bestPlayerName;
    private String secondPlayerName;
    private String thirdPlayerName;
    private bool showSetPlayerName;
    private bool ShowBestScoreText;
    private float counter;
    private float counterLletres;
    private int actualPlayerPos;
    private int playerScore;
    private int bestScore;
    private int secondScore;
    private int thirdScore;
    private int numLettersSelected;
    private readonly List<Char> lletres = new() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '_' };
    private String playerName;
    private String lletra1;
    private String lletra2;
    private String lletra3;
    private String lletra4;
    private int lletra1Val;
    private int lletra2Val;
    private int lletra3Val;
    private int lletra4Val;
    private int numLletres;
    private Color unselected;
    private bool optionChanged;

    void Start()
    {
        optionChanged = true;
        if (PlayerPrefs.HasKey("TimeUp"))
        {
            if (PlayerPrefs.GetInt("TimeUp") == 1)
            {
                gameOverText.text = "Time's up";
            }
            else if (PlayerPrefs.GetInt("TimeUp") == 0)
            {
                gameOverText.text = "Game over";
            }
        }
        else
        {
            gameOverText.text = "Game over";
        }
        GOSource = gameObject.GetComponent<AudioSource>();
        GOSource.clip = game_over;
        game_over_finished = false;
        game_over_lenght = game_over.length;
        GOSource.Play();
        unselected = new Color(0.5f, 0.5f, 0.5f);
        numBoto = 0;
        numBotons = 2;
        confirmText.SetActive(false);
        setName.SetActive(false);
        actualPlayerPos = 0;
        lletra1 = lletres[0].ToString();
        lletra2 = lletres[0].ToString();
        lletra3 = lletres[0].ToString();
        lletra4 = lletres[0].ToString();
        lletra1Val = 0;
        lletra2Val = 0;
        lletra3Val = 0;
        lletra4Val = 0;
    numLettersSelected = 0;
        playerName = lletra1 + lletra2 + lletra3 + lletra4;
        playerNameLetter1.text = lletra1;
        playerNameLetter2.text = lletra2;
        playerNameLetter3.text = lletra3;
        numLletres = 0;
        showSetPlayerName = false;
        counter = 0.0f;
        counterLletres = 0.0f;
        timerBotons = 0.0f;
        highScore.SetActive(false);
        ShowBestScoreText = false;
        playerScore = PlayerPrefs.GetInt("PlayerScore");
        PlayerScoreText.text = "Your score: " + playerScore.ToString("D5");
        if (PlayerPrefs.HasKey("BestPlayerName"))
        {
            bestPlayerName = PlayerPrefs.GetString("BestPlayerName");
        }
        else
        {
            bestPlayerName = "PLAY";
        }
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetInt("BestScore");
        }
        else
        {
            bestScore = 0;
        }
        if (PlayerPrefs.HasKey("SecondPlayerName"))
        {
            secondPlayerName = PlayerPrefs.GetString("SecondPlayerName");
        }
        else
        {
            secondPlayerName = "PLAY";
        }
        if (PlayerPrefs.HasKey("SecondScore"))
        {
            secondScore = PlayerPrefs.GetInt("SecondScore");
        }
        else
        {
            secondScore = 0;
        }
        if (PlayerPrefs.HasKey("ThirdPlayerName"))
        {
            thirdPlayerName = PlayerPrefs.GetString("ThirdPlayerName");
        }
        else
        {
            thirdPlayerName = "PLAY";
        }
        if (PlayerPrefs.HasKey("ThirdScore"))
        {
            thirdScore = PlayerPrefs.GetInt("ThirdScore");
        }
        else
        {
            thirdScore = 0;
        }

        if (playerScore >= bestScore)
        {
            ShowBestScoreText = true;
            thirdScore = secondScore;
            thirdPlayerName = secondPlayerName;
            secondScore = bestScore;
            secondPlayerName = bestPlayerName;
            bestScore = playerScore;
            bestPlayerName = "PLAY";
            showSetPlayerName = true;
            setName.SetActive(true);
            actualPlayerPos = 1;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.SetString("BestPlayerName", bestPlayerName);
            PlayerPrefs.SetInt("SecondScore", secondScore);
            PlayerPrefs.SetString("SecondPlayerName", secondPlayerName);
            PlayerPrefs.SetInt("ThirdScore", thirdScore);
            PlayerPrefs.SetString("ThirdPlayerName", thirdPlayerName);

        }
        else if (playerScore >= secondScore)
        {
            thirdScore = secondScore;
            thirdPlayerName = secondPlayerName;
            secondScore = playerScore;
            secondPlayerName = "PLAY";
            showSetPlayerName = true;
            setName.SetActive(true);
            actualPlayerPos = 2;
            PlayerPrefs.SetInt("SecondScore", secondScore);
            PlayerPrefs.SetString("SecondPlayerName", secondPlayerName);
            PlayerPrefs.SetInt("ThirdScore", thirdScore);
            PlayerPrefs.SetString("ThirdPlayerName", thirdPlayerName);
        }
        else if (playerScore >= thirdScore)
        {
            thirdScore = playerScore;
            thirdPlayerName = "PLAY";
            showSetPlayerName = true;
            setName.SetActive(true);
            actualPlayerPos = 3;
            PlayerPrefs.SetInt("ThirdScore", thirdScore);
            PlayerPrefs.SetString("ThirdPlayerName", thirdPlayerName);
        }
        else
        {
            showSetPlayerName = false;
        }
        bestPlayerText.text = "1st (" + bestPlayerName + ")";
        bestScoreText.text = bestScore.ToString("D5");
        secondPlayerText.text = "2nd (" + secondPlayerName + ")";
        secondScoreText.text = secondScore.ToString("D5");
        thirdPlayerText.text = "3rd (" + thirdPlayerName + ")";
        thirdScoreText.text = thirdScore.ToString("D5");
        quitButtonText.color = unselected;
        replayButtonText.color = unselected;
    }

    // Update is called once per frame
    void Update()
    {
        if (game_over_finished == false)
        {
            game_over_lenght -= Time.deltaTime;
            if (game_over_lenght < 0)
            {
                GOSource.Stop();
                GOSource.clip = game_over_music;
                GOSource.Play();
                game_over_finished = true;
            }
        }
        if (showSetPlayerName == false)
        {
            timerBotons -= Time.deltaTime;
            if (timerBotons < 0)
            {
                timerBotons = 0.5f;
                if (numBoto == 0)
                {
                    if (optionChanged == true)
                    {
                        optionChanged = false;
                        if (!quitButtonText.isActiveAndEnabled)
                        {
                            quitButtonText.enabled = true;
                        }
                        replayButtonText.color = Color.white;
                        quitButtonText.color = unselected;
                    }

                    if (replayButtonText.isActiveAndEnabled)
                    {
                        replayButtonText.enabled = false;
                    }
                    else
                    {
                        replayButtonText.enabled = true;
                    }

                }
                else if (numBoto == 1)
                {
                    if (optionChanged == true)
                    {
                        if (!replayButtonText.isActiveAndEnabled)
                        {
                            replayButtonText.enabled = true;
                        }
                        replayButtonText.color = unselected;
                        quitButtonText.color = Color.white;
                    }

                    if (quitButtonText.isActiveAndEnabled)
                    {
                        quitButtonText.enabled = false;
                    }
                    else
                    {
                        quitButtonText.enabled = true;
                    }

                }

            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Arcade.ac.ButtonDown("lb") || Arcade.ac.ButtonDown("j1_Right"))
            {
                numBoto++;
                if (timerBotons > 0.1f)
                {
                    timerBotons = 0.1f;
                }
                optionChanged = true;
                if (numBoto >= numBotons)
                {
                    numBoto = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Arcade.ac.ButtonDown("la") || Arcade.ac.ButtonDown("j1_Left"))
            {
                numBoto--;
                if (timerBotons > 0.1f)
                {
                    timerBotons = 0.1f;
                }
                optionChanged = true;
                if (numBoto < 0)
                {
                    numBoto = numBotons - 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.Return) || Arcade.ac.ButtonDown("l1") || Arcade.ac.ButtonDown("select"))
            {
                if (numBoto == 0)
                {
                    PlayAgainButtonPressed();
                }
                else if (numBoto == 1)
                {
                    QuitButtonPressed();
                }
            }
        }


        else
        {
            if (numLettersSelected < 5)
            {
                counterLletres -= Time.deltaTime;
                if (counterLletres < 0)
                {
                    counterLletres = 0.4f;
                    if (numLettersSelected == 0)
                    {
                        if (optionChanged == true)
                        {
                            optionChanged = false;
                            playerNameLetter1.color = Color.white;
                            playerNameLetter2.color = unselected;
                            playerNameLetter3.color = unselected;
                            playerNameLetter4.color = unselected;
                            if (!playerNameLetter2.isActiveAndEnabled)
                            {
                                playerNameLetter2.enabled = true;
                            }
                            if (!playerNameLetter3.isActiveAndEnabled)
                            {
                                playerNameLetter3.enabled = true;
                            }
                            if (!playerNameLetter4.isActiveAndEnabled)
                            {
                                playerNameLetter4.enabled = true;
                            }
                            if (confirmText.activeInHierarchy)
                            {
                                confirmText.SetActive(false);
                            }
                        }
                        if (playerNameLetter1.isActiveAndEnabled)
                        {
                            playerNameLetter1.enabled = false;
                        }

                        else
                        {
                            playerNameLetter1.enabled = true;
                        }
                    }
                    else if (numLettersSelected == 1)
                    {
                        if (optionChanged == true)
                        {
                            optionChanged = false;
                            playerNameLetter1.color = unselected;
                            playerNameLetter2.color = Color.white;
                            playerNameLetter3.color = unselected;
                            playerNameLetter4.color = unselected;
                            if (!playerNameLetter1.isActiveAndEnabled)
                            {
                                playerNameLetter1.enabled = true;
                            }
                            if (!playerNameLetter3.isActiveAndEnabled)
                            {
                                playerNameLetter3.enabled = true;
                            }
                            if (!playerNameLetter4.isActiveAndEnabled)
                            {
                                playerNameLetter4.enabled = true;
                            }
                            if (confirmText.activeInHierarchy)
                            {
                                confirmText.SetActive(false);
                            }
                        }
                        if (playerNameLetter2.isActiveAndEnabled)
                        {
                            playerNameLetter2.enabled = false;
                        }
                        else
                        {
                            playerNameLetter2.enabled = true;
                        }
                    }
                    else if (numLettersSelected == 2)
                    {
                        if (optionChanged == true)
                        {
                            optionChanged = false;
                            playerNameLetter1.color = unselected;
                            playerNameLetter2.color = unselected;
                            playerNameLetter3.color = Color.white;
                            playerNameLetter4.color = unselected;
                            if (!playerNameLetter1.isActiveAndEnabled)
                            {
                                playerNameLetter1.enabled = true;
                            }
                            if (!playerNameLetter2.isActiveAndEnabled)
                            {
                                playerNameLetter2.enabled = true;
                            }
                            if (!playerNameLetter4.isActiveAndEnabled)
                            {
                                playerNameLetter4.enabled = true;
                            }
                            if (confirmText.activeInHierarchy)
                            {
                                confirmText.SetActive(false);
                            }
                        }
                        if (playerNameLetter3.isActiveAndEnabled)
                        {
                            playerNameLetter3.enabled = false;
                        }
                        else
                        {
                            playerNameLetter3.enabled = true;
                        }

                    }
                    else if (numLettersSelected == 3)
                    {
                        if (optionChanged == true)
                        {
                            optionChanged = false;
                            playerNameLetter1.color = unselected;
                            playerNameLetter2.color = unselected;
                            playerNameLetter3.color = unselected;
                            playerNameLetter4.color = Color.white;
                            if (!playerNameLetter1.isActiveAndEnabled)
                            {
                                playerNameLetter1.enabled = true;
                            }
                            if (!playerNameLetter2.isActiveAndEnabled)
                            {
                                playerNameLetter2.enabled = true;
                            }
                            if (!playerNameLetter3.isActiveAndEnabled)
                            {
                                playerNameLetter3.enabled = true;
                            }
                            if (confirmText.activeInHierarchy)
                            {
                                confirmText.SetActive(false);
                            }
                        }
                        if (playerNameLetter4.isActiveAndEnabled)
                        {
                            playerNameLetter4.enabled = false;
                        }
                        else
                        {
                            playerNameLetter4.enabled = true;
                        }

                    }
                    else if (numLettersSelected == 4)
                    {
                        if (optionChanged == true)
                        {
                            optionChanged = false;
                            playerNameLetter1.color = unselected;
                            playerNameLetter2.color = unselected;
                            playerNameLetter3.color = unselected;
                            playerNameLetter4.color = unselected;
                            if (!playerNameLetter1.isActiveAndEnabled)
                            {
                                playerNameLetter1.enabled = true;
                            }
                            if (!playerNameLetter2.isActiveAndEnabled)
                            {
                                playerNameLetter2.enabled = true;
                            }
                            if (!playerNameLetter3.isActiveAndEnabled)
                            {
                                playerNameLetter3.enabled = true;
                            }
                            if (!playerNameLetter4.isActiveAndEnabled)
                            {
                                playerNameLetter4.enabled = true;
                            }
                        }
                        if (confirmText.activeInHierarchy)
                        {
                            confirmText.SetActive(false);
                        }
                        else
                        {
                            confirmText.SetActive(true);
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.UpArrow) || Arcade.ac.ButtonDown("lb") || Arcade.ac.ButtonDown("j1_Up"))
                {
                    numLletres++;
                    if (numLletres >= lletres.Count)
                    {
                        numLletres = 0;
                    }
                    if (numLettersSelected == 0)
                    {
                        lletra1 = lletres[numLletres].ToString();
                        playerNameLetter1.text = lletra1;
                        lletra1Val = numLletres;
                    }
                    else if (numLettersSelected == 1)
                    {
                        lletra2 = lletres[numLletres].ToString();
                        playerNameLetter2.text = lletra2;
                        lletra2Val = numLletres;
                    }
                    else if (numLettersSelected == 2)
                    {
                        lletra3 = lletres[numLletres].ToString();
                        playerNameLetter3.text = lletra3;
                        lletra3Val = numLletres;
                    }
                    else if (numLettersSelected == 3)
                    {
                        lletra4 = lletres[numLletres].ToString();
                        playerNameLetter4.text = lletra4;
                        lletra4Val = numLletres;
                    }
                    playerName = lletra1 + lletra2 + lletra3 + lletra4;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) || Arcade.ac.ButtonDown("la") || Arcade.ac.ButtonDown("j1_Down"))
                {
                    numLletres--;
                    if (numLletres < 0)
                    {
                        numLletres = lletres.Count - 1;
                    }
                    if (numLettersSelected == 0)
                    {
                        lletra1 = lletres[numLletres].ToString();
                        playerNameLetter1.text = lletra1;
                        lletra1Val = numLletres;
                    }
                    else if (numLettersSelected == 1)
                    {
                        lletra2 = lletres[numLletres].ToString();
                        playerNameLetter2.text = lletra2;
                        lletra2Val = numLletres;
                    }
                    else if (numLettersSelected == 2)
                    {
                        lletra3 = lletres[numLletres].ToString();
                        playerNameLetter3.text = lletra3;
                        lletra3Val = numLletres;
                    }
                    else if (numLettersSelected == 3)
                    {
                        lletra4 = lletres[numLletres].ToString();
                        playerNameLetter4.text = lletra4;
                        lletra4Val = numLletres;
                    }
                    playerName = lletra1 + lletra2 + lletra3 + lletra4;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Arcade.ac.ButtonDown("rb") || Arcade.ac.ButtonDown("j1_Right") || Arcade.ac.ButtonDown("select"))
                {
                    numLettersSelected++;
                    if (counterLletres > 0.1f)
                    {
                        counterLletres = 0.1f;
                    }
                    optionChanged = true;
                    if (numLettersSelected == 0)
                    {
                        numLletres = lletra1Val;
                    }
                    else if (numLettersSelected == 1)
                    {
                        numLletres = lletra2Val;
                    }
                    else if (numLettersSelected == 2)
                    {
                        numLletres = lletra3Val;
                    }
                    else if (numLettersSelected == 3)
                    {
                        numLletres = lletra4Val;
                    }
                    else
                    {
                        numLletres = 0;
                    }
                    if (numLettersSelected > 4)
                    {
                        setName.SetActive(false);
                        showSetPlayerName = false;
                        if (actualPlayerPos == 1)
                        {
                            bestPlayerText.text = "1st (" + playerName + ")";
                            bestScoreText.text = playerScore.ToString("D5");
                            PlayerPrefs.SetString("BestPlayerName", playerName);
                        }
                        else if (actualPlayerPos == 2)
                        {
                            secondPlayerText.text = "2nd (" + playerName + ")";
                            secondScoreText.text = playerScore.ToString("D5");
                            PlayerPrefs.SetString("SecondPlayerName", playerName);
                        }
                        else if (actualPlayerPos == 3)
                        {
                            thirdPlayerText.text = "3rd (" + playerName + ")";
                            thirdScoreText.text = playerScore.ToString("D5");
                            PlayerPrefs.SetString("ThirdPlayerName", playerName);
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Arcade.ac.ButtonDown("lb") || Arcade.ac.ButtonDown("j1_Left"))
                {
                    if (numLettersSelected > 0)
                    {
                        if (counterLletres > 0.1f)
                        {
                            counterLletres = 0.1f;
                        }
                        numLettersSelected--;
                        optionChanged = true;
                        if(numLettersSelected == 0)
                        {
                            numLletres = lletra1Val;
                        }else if (numLettersSelected == 1)
                        {
                            numLletres = lletra2Val;
                        }else if(numLettersSelected == 2)
                        {
                            numLletres = lletra3Val;
                        }else if(numLettersSelected == 3)
                        {
                            numLletres = lletra4Val;
                        }
                        else
                        {
                            numLletres = 0;
                        }
                    }
                }

            }
        }

        if (ShowBestScoreText == true)
        {
            counter += Time.deltaTime;
            if (counter > 1.0f)
            {
                counter = 0.0f;
                if (highScore.activeSelf == false)
                {
                    highScore.SetActive(true);
                }
                else if (highScore.activeSelf == true)
                {
                    highScore.SetActive(false);
                }
            }
        }

    }
    public void PlayAgainButtonPressed()
    {
        SceneManager.LoadScene("IntroScene");
    }
    public void QuitButtonPressed()
    {
        Application.Quit();
    }
}
