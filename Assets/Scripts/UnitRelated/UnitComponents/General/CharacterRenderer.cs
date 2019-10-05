using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRenderer : MonoBehaviour
{
    public string AnimationPrefix;
    //public GameObject ColliderGhost;

    public int LastDirection
    {
        get
        {
            return lastDirection;
        }
    }

    public string PreparedAnimationPrefix
    {
        get
        {
            return AnimationPrefix == "" ? AnimationPrefix : AnimationPrefix + " ";
        }
    }

    Animator animator;
    BoxCollider2D collider;
    Attack attack;
    int lastDirection;

    private void Awake()
    {
        //cache the animator component
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        attack = GetComponent<Attack>();

        staticDirections = new string[]
        {
            PreparedAnimationPrefix + "Static N",
            PreparedAnimationPrefix + "Static NE",
            PreparedAnimationPrefix + "Static E",
            PreparedAnimationPrefix + "Static SE",
            PreparedAnimationPrefix + "Static S",
            PreparedAnimationPrefix + "Static SW",
            PreparedAnimationPrefix + "Static W",
            PreparedAnimationPrefix + "Static NW"
        };
        runDirections = new string[]
        {
            PreparedAnimationPrefix + "Run N",
            PreparedAnimationPrefix + "Run NE",
            PreparedAnimationPrefix + "Run E",
            PreparedAnimationPrefix + "Run SE",
            PreparedAnimationPrefix + "Run S",
            PreparedAnimationPrefix + "Run SW",
            PreparedAnimationPrefix + "Run W",
            PreparedAnimationPrefix + "Run NW"
        };
    }
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDirection(Vector2 direction)
    {
        if (attack.AnimationInProgress)
            return;

        string[] directionArray = null;

        if (direction.magnitude < .01f)
        {
            directionArray = staticDirections;
        }
        else
        {
            directionArray = runDirections;
            lastDirection = DirectionToIndex(direction, 8);
        }

        string lastDirName = directionArray[lastDirection];
        animator.Play(lastDirName);
        //SetColliderSize();
    }

    //void SetColliderSize()
    //{
    //    Vector2 curColliderSize = colliderSizes[lastDirection];
    //    collider.size = curColliderSize;
    //    if (ColliderGhost.activeSelf)
    //        ColliderGhost.transform.localScale = new Vector3(curColliderSize.x, curColliderSize.y, 1);
    //}
	
	//this function converts a Vector2 direction to an index to a slice around a circle
    //this goes in a counter-clockwise direction.

    public static int DirectionToIndex(Vector2 dir, int sliceCount){
        //get the normalized direction
        Vector2 normDir = dir.normalized;
        //calculate how many degrees one slice is
        float step = 360f / sliceCount;
        //calculate how many degress half a slice is.
        //we need this to offset the pie, so that the North (UP) slice is aligned in the center
        float halfstep = step / 2;
        //get the angle from -180 to 180 of the direction vector relative to the Up vector.
        //this will return the angle between dir and North.
        float angle = -Vector2.SignedAngle(Vector2.up, normDir);
        //add the halfslice offset
        angle += halfstep;
        //if angle is negative, then let's make it positive by adding 360 to wrap it around.
        if (angle < 0){
            angle += 360;
        }
        //calculate the amount of steps required to reach this angle
        float stepCount = angle / step;
        //round it, and we have the answer!
        return Mathf.FloorToInt(stepCount);
    }

    public string[] staticDirections =
    {
        "Static N",
        "Static NE",
        "Static E",
        "Static SE",
        "Static S",
        "Static SW",
        "Static W",
        "Static NW"
    };

    public string[] runDirections =
    {
        "Run N",
        "Run NE",
        "Run E",
        "Run SE",
        "Run S",
        "Run SW",
        "Run W",
        "Run NW"
    };

    public Vector2[] colliderSizes =
    {
        new Vector2(0.75f, 0.18f),
        new Vector2(0.75f, 0.18f),
        new Vector2(0.6f, 0.11f),
        new Vector2(0.6f, 0.11f),
        new Vector2(0.78f, 0.15f),
        new Vector2(0.78f, 0.15f),
        new Vector2(0.6f, 0.11f),
        new Vector2(0.6f, 0.11f),
    };
}
