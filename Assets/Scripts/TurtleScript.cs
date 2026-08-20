using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TurtleScript : MonoBehaviour
{
    private static readonly int SubmergedHash = Animator.StringToHash("submerged");
    private Animator turtleAnimator;
    private float waterTime;
    private bool turtleTotallySubmerged;
    private bool turtleSubmerged;
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
                turtleAnimator.SetInteger(SubmergedHash, 3);
                turtleTotallySubmerged = false;
            }
        }
    }

    public void Submerge(float timeSubmerged)
    {
        if (turtleSubmerged == false)
        {
            turtleAnimator.SetInteger(SubmergedHash, 1);
            waterTime = timeSubmerged;
        }
    }



    public void TurtleHasSubmerged()
    {
        turtleAnimator.SetInteger(SubmergedHash, 2);
        turtleGroupScript.ChangeSubmerged();
        turtleSubmerged = true;
        turtleTotallySubmerged = true;
    }
    public void TurtleHasEmerged()
    {
        turtleAnimator.SetInteger(SubmergedHash, 4);
        turtleGroupScript.TurtleEmerged();
        turtleSubmerged = false;
    }

}
