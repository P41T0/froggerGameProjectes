using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRowScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed;
    [SerializeField] private GameObject[] waterRowElems;
    void Start()
    {
        foreach (GameObject waterRowElem in waterRowElems)
        {
            waterRowElem.GetComponent<WaterObstaclesScript>().SetSpeed(speed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + speed * Time.deltaTime, gameObject.transform.position.y);
    }
}
