using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private bool facingRight;
    [SerializeField] private GameObject enemyRespawn;
    private Vector2 enemyRespawnPos;
    // Start is called before the first frame update
    void Start()
    {
        enemyRespawnPos = enemyRespawn.transform.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!facingRight)
        {
            if (collision.gameObject.CompareTag("LeftWall"))
            {
                gameObject.transform.position = enemyRespawnPos;
            }
        }
        else if (facingRight)
        {
            if (collision.gameObject.CompareTag("RightWall"))
            {
                gameObject.transform.position = enemyRespawnPos;
            }
        }

    }
}
