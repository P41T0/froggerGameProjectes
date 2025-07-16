using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioFrog;
    [SerializeField] private AudioClip frogHop;
    [SerializeField] private AudioClip frogDiedWater;
    [SerializeField] private AudioClip frogDiedLand;
    [SerializeField] private Sprite frogIdleSprite;
    [SerializeField] private Sprite frogMovingSprite;
    private GameObject sceneController;
    private SCLevelScript scScript;
    private SpriteRenderer frogRenderer;
    private float moveVertical;
    private float moveHorizontal;
    private Vector2 initialPosition;
    private bool safeOfWater;
    private float waitTime;
    private float logSpeed;
    private bool inaFrogBool;
    private float maxplayerYposition;
    void Start()
    {
        inaFrogBool = false;
        audioFrog = gameObject.GetComponent<AudioSource>();
        sceneController = GameObject.FindGameObjectWithTag("SceneController");
        scScript = sceneController.GetComponent<SCLevelScript>();
        initialPosition = gameObject.transform.position;
        maxplayerYposition = gameObject.transform.position.y;
        frogRenderer = gameObject.GetComponent<SpriteRenderer>();
        frogRenderer.sprite = frogIdleSprite;
        moveVertical = 0;
        moveHorizontal = 0;
        safeOfWater = false;
        waitTime = 0;
        logSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime >= 0)
        {
            waitTime -= Time.deltaTime;
            if (moveVertical != 0)
            {
                moveVertical = 0;
            }
            if (moveHorizontal != 0)
            {
                moveHorizontal = 0;
            }
        }
        else if (waitTime < 0)
        {
            if (frogRenderer.sprite != frogIdleSprite)
            {
                frogRenderer.sprite = frogIdleSprite;
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Arcade.ac.ButtonDown("j1_Up"))
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                frogRenderer.flipY = false;
                moveVertical = 1;
                waitTime = 0.15f;


            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Arcade.ac.ButtonDown("j1_Down"))
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                frogRenderer.flipY = true;
                moveVertical = -1;
                waitTime = 0.15f;

            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Arcade.ac.ButtonDown("j1_Right"))
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                frogRenderer.flipY = true;
                moveHorizontal = 1;
                waitTime = 0.15f;

            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Arcade.ac.ButtonDown("j1_Left"))
            {
                frogRenderer.flipY = false;
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                moveHorizontal = -1;
                waitTime = 0.15f;
            }
        }
        if (safeOfWater == true)
        {
            if (moveVertical == 0 && moveHorizontal == 0)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x + logSpeed * Time.deltaTime, gameObject.transform.position.y);
            }
            else if (moveVertical != 0 || moveHorizontal != 0)
            {
                if (((gameObject.transform.position.x + 1 * moveHorizontal) > -6.5 && (gameObject.transform.position.x + 1 * moveHorizontal) < 6.5))
                {
                    audioFrog.Stop();
                    audioFrog.clip = frogHop;
                    audioFrog.Play();
                    frogRenderer.sprite = frogMovingSprite;
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x + 1f * moveHorizontal + logSpeed * Time.deltaTime, gameObject.transform.position.y + 1f * moveVertical);
                }


                if (gameObject.transform.position.y > maxplayerYposition)
                {
                    maxplayerYposition = gameObject.transform.position.y;
                    scScript.IncScore();
                }
            }

        }
        else if (safeOfWater == false)
        {
            if (moveVertical != 0 || moveHorizontal != 0)
            {
                if ((((gameObject.transform.position.x + 1 * moveHorizontal) > -6.5 && (gameObject.transform.position.x + 1 * moveHorizontal) < 6.5)) && (((gameObject.transform.position.y + 1 * moveVertical) > -4.5) && ((gameObject.transform.position.y + 1 * moveVertical) < 4.0)))
                {
                    frogRenderer.sprite = frogMovingSprite;
                    audioFrog.Stop();
                    audioFrog.clip = (frogHop);
                    audioFrog.Play();
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x + 1f * moveHorizontal, gameObject.transform.position.y + 1f * moveVertical);
                }

                if (gameObject.transform.position.y > maxplayerYposition)
                {
                    maxplayerYposition = gameObject.transform.position.y;
                    scScript.IncScore();
                }
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyCar"))
        {
            scScript.ReduceLife();
            gameObject.transform.SetPositionAndRotation(initialPosition, Quaternion.Euler(0, 0, 0));
            frogRenderer.flipY = false;
            frogRenderer.sprite = frogMovingSprite;
            audioFrog.Stop();
            audioFrog.clip = (frogDiedLand);
            audioFrog.Play();
            waitTime = 1.0f;
        }
        if (collision.gameObject.CompareTag("log"))
        {
            if (safeOfWater == false)
            {
                safeOfWater = true;
            }

            logSpeed = collision.GetComponent<WaterObstaclesScript>().GetSpeed();
        }
        if (collision.gameObject.CompareTag("turtles"))
        {
            logSpeed = collision.GetComponent<WaterObstaclesScript>().GetSpeed();
            if (collision.GetComponent<TurtleGroupScript>().IsSubmerged() == false)
            {
                if (safeOfWater == false)
                {
                    safeOfWater = true;
                }
            }
            else
            {
                if (safeOfWater == true)
                {
                    safeOfWater = false;
                }

            }
        }
        if (collision.gameObject.CompareTag("victoryFrog"))
        {
            inaFrogBool = true;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            frogRenderer.flipY = false;
            frogRenderer.sprite = frogMovingSprite;
            gameObject.transform.position = initialPosition;
            maxplayerYposition = gameObject.transform.position.y;
            waitTime = 1.0f;
        }

        if (collision.gameObject.CompareTag("TopWall") || collision.gameObject.CompareTag("TopWallFrog"))
        {
            if (inaFrogBool == false)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x, 2.5f);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("log"))
        {
            if (safeOfWater == false)
            {
                safeOfWater = true;
            }

        }
        if (collision.gameObject.CompareTag("turtles"))
        {

            if (collision.GetComponent<TurtleGroupScript>().IsSubmerged() == false)
            {
                if (safeOfWater == false)
                {
                    safeOfWater = true;
                }

            }
            else
            {
                if (safeOfWater == true)
                {
                    safeOfWater = false;
                }

            }
        }





        if (collision.gameObject.CompareTag("TopWall") || collision.gameObject.CompareTag("TopWallFrog"))
        {
            if (inaFrogBool == false)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x, 2.5f);
            }
        }

        if (collision.gameObject.CompareTag("water"))
        {
            if (safeOfWater == false)
            {
                if (waitTime < 0.05f)
                {
                    scScript.ReduceLife();
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    frogRenderer.flipY = false;
                    frogRenderer.sprite = frogMovingSprite;
                    gameObject.transform.position = initialPosition;
                    audioFrog.Stop();
                    audioFrog.clip = (frogDiedWater);
                    audioFrog.Play();
                    waitTime = 1.0f;
                }
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("turtles") || collision.gameObject.CompareTag("log"))
        {
            if (safeOfWater == true)
            {
                safeOfWater = false;
            }

        }

        if (collision.gameObject.CompareTag("victoryFrog") || collision.gameObject.CompareTag("TopWallFrog"))
        {
            if (inaFrogBool == true)
            {
                inaFrogBool = false;
            }
        }
    }

}
