using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleGroupScript : MonoBehaviour
{
    [SerializeField] private float maxTimeOutside;
    [SerializeField] private float minTimeOutside;
    [SerializeField] private float timeSubmerged;
    [SerializeField] private GameObject[] turtles;
    private float timeOutside;
    private bool submerged;
    private bool submergeTurtle;
    // Start is called before the first frame update
    void Start()
    {
        submergeTurtle = false;
        submerged = false;
        timeOutside = Random.Range(minTimeOutside, maxTimeOutside);
    }

    // Update is called once per frame
    void Update()
    {
        if (submergeTurtle == false)
        {

            timeOutside -= Time.deltaTime;
            if (timeOutside < 0)
            {
                submergeTurtle = true;
                foreach (GameObject turtle in turtles)
                {
                    turtle.GetComponent<TurtleScript>().Submerge(timeSubmerged);
                }
                timeOutside = Random.Range(minTimeOutside, maxTimeOutside);
            }
        }
    }

    public void TurtleEmerged()
    {
        if (submerged == true)
        {
            submerged = false;
            submergeTurtle = false;
        }
    }
    public bool IsSubmerged()
    {
        return submerged;
    }

    public void ChangeSubmerged()
    {
        if (submerged == false)
        {
            submerged = true;
        }
    }
}
