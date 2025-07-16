using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowScript : MonoBehaviour
{
    [SerializeField] private float Speed;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + Speed * Time.deltaTime, gameObject.transform.position.y);
    }
}
