using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ActionAnimation
{
    public string[] directions { get; set; }
    public float LastAnimationStartTime { get; set; }
    public Animator animator { get; set; }
    public bool AnimationInProgress { get; set; }

    public bool ShouldBeEnded
    {
        get
        {
            //Debug.Log($"Calculating 'ShouldBeEnded'. AnimationInProgress={AnimationInProgress}. Time={Time.time}. LastAnimationStartTime={LastAnimationStartTime}. maxAnimationLength={maxAnimationLength}");
            return AnimationInProgress && Time.time - LastAnimationStartTime > maxAnimationLength;
        }
    }

    protected CharacterRenderer renderer;
    protected float maxAnimationLength;
    protected MonoBehaviour Host;
    protected string ActionName;

    public ActionAnimation(Animator a, CharacterRenderer cr, string name, float maxLength)
    {
        animator = a;
        renderer = cr;
        ActionName = name;
        AnimationInProgress = false;
        string animationPrefix = renderer.PreparedAnimationPrefix;
        maxAnimationLength = maxLength;
        directions = new string[]
        {
                $"{animationPrefix}{ActionName} N",
                $"{animationPrefix}{ActionName} NE",
                $"{animationPrefix}{ActionName} E",
                $"{animationPrefix}{ActionName} SE",
                $"{animationPrefix}{ActionName} S",
                $"{animationPrefix}{ActionName} SW",
                $"{animationPrefix}{ActionName} W",
                $"{animationPrefix}{ActionName} NW"
        };
    }

    public void Start()
    {
        string animationName = directions[renderer.LastDirection];
        animator.Play(animationName);
        AnimationInProgress = true;
        LastAnimationStartTime = Time.time;
    }

    public void End()
    {
        //Debug.Log("Animation end!");
        AnimationInProgress = false;
    }
}