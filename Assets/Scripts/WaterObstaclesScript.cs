using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObstaclesScript : MonoBehaviour
{
    private float speed;
    [SerializeField] private GameObject enemyRespawn;
    private Vector2 enemyRespawnPos;
    // Start is called before the first frame update
    void Start()
    {
        enemyRespawnPos = enemyRespawn.transform.position;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (speed < 0)
        {
            if (collision.gameObject.CompareTag("LeftWall"))
            {
                gameObject.transform.position = enemyRespawnPos;
            }
        }
        else if (speed > 0)
        {
            if (collision.gameObject.CompareTag("RightWall"))
            {
                gameObject.transform.position = enemyRespawnPos;
            }
        }

    }
    
    public void SetSpeed(float rowSpeed)
    {
        speed = rowSpeed;
    }
    public float GetSpeed()
    {
        return speed;
    }
}
