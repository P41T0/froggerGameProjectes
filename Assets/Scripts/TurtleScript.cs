using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleScript : MonoBehaviour
{
    private Animator turtleAnimator;
    private float waterTime;
    private bool turtleTotallySubmerged;
    private bool turtleSubmerged;
    private bool turtleSubmerging;
    [SerializeField] private GameObject turtleGroup;
    private TurtleGroupScript turtleGroupScript;

    // Start is called before the first frame update
    void Start()
    {
        waterTime = 0;
        turtleTotallySubmerged = false;
        turtleSubmerged = false;
        turtleAnimator = GetComponent<Animator>();
        turtleGroupScript = turtleGroup.GetComponent<TurtleGroupScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turtleTotallySubmerged == true)
        {
            waterTime -= Time.deltaTime;
            if (waterTime < 0)
            {
                turtleAnimator.SetInteger("submerged", 3);
                turtleTotallySubmerged = false;
            }
        }
    }

    public void Submerge(float timeSubmerged)
    {
        if (turtleSubmerged == false)
        {
            turtleAnimator.SetInteger("submerged", 1);
            waterTime = timeSubmerged;
        }
    }



    public void TurtleHasSubmerged()
    {
        turtleAnimator.SetInteger("submerged", 2);
        turtleGroupScript.ChangeSubmerged();
        turtleSubmerged = true;
        turtleTotallySubmerged = true;
    }
    public void TurtleHasEmerged()
    {
        turtleAnimator.SetInteger("submerged", 4);
        turtleGroupScript.TurtleEmerged();
        turtleSubmerged = false;
    }

}
