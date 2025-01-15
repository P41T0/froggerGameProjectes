using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class VictoryFrogsScript : MonoBehaviour
{
    private SpriteRenderer frogRenderer;
    private int frogPosition;
    [SerializeField] private Sprite bonificationSprite;
    [SerializeField] private Sprite victoryFrogSprite;
    private GameObject scGameObject;
    private SCLevelScript scScript;
    private float bonificationTimer;
    private bool isUnlocked;
    private bool hasBonification;
    // Start is called before the first frame update
    void Start()
    {
        scGameObject = GameObject.FindGameObjectWithTag("SceneController");
        scScript = scGameObject.GetComponent<SCLevelScript>();
        hasBonification = false;
        isUnlocked = false;
        frogRenderer = gameObject.GetComponent<SpriteRenderer>();
        frogRenderer.sprite = null;
    }



    // Update is called once per frame
    void Update()
    {
        if (isUnlocked == false)
        {
            if (hasBonification)
            {
                bonificationTimer -= Time.deltaTime;
                if (bonificationTimer < 0)
                {
                    hasBonification = false;
                    frogRenderer.sprite = null;
                    scScript.BonificationItemDestroyed();
                }
            }
        }
    }
    public void GiveFrogBonification()
    {
        if (isUnlocked == false)
        {
            bonificationTimer = 5.0f;
            frogRenderer.sprite = bonificationSprite;
            hasBonification = true;
        }
        else
        {
            scScript.BonificationItemDestroyed();
        }
    }
    public void SetFrogPos(int pos)
    {
        frogPosition = pos;
    }

    public bool GetIsUnlocked()
    {
        return isUnlocked;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("victoryFrog"))
            {
                frogRenderer.sprite = victoryFrogSprite;
                gameObject.tag = "TopWallFrog";
                scScript.VictoryFrogUnlocked(frogPosition, hasBonification);
                isUnlocked = true;
            }

        }
    }
}
