using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float AttackDistance = 0.9f;

    public bool AnimationInProgress
    {
        get
        {
            return animationInProgress;
        }
    }

    protected CharacterRenderer renderer;
    protected Animator animator;
    protected bool animationInProgress;
    protected float lastAttackStartTime;
    protected CharacterSoundsPlayer soundPlayer;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<CharacterRenderer>();
        soundPlayer = GetComponentInChildren<CharacterSoundsPlayer>();
        animationInProgress = false;
        string animationPrefix = renderer.PreparedAnimationPrefix;
        directions = new string[]
        {
            animationPrefix + "Attack N",
            animationPrefix + "Attack NE",
            animationPrefix + "Attack E",
            animationPrefix + "Attack SE",
            animationPrefix + "Attack S",
            animationPrefix + "Attack SW",
            animationPrefix + "Attack W",
            animationPrefix + "Attack NW"
        };
    }

    // Call in the end if overridden
    protected virtual void Update()
    {
        if (AttackShouldBeEnded)
        {
            ATTACK_EndAnimation();
        }
    }

    bool AttackShouldBeEnded
    {
        get
        {
            return animationInProgress && Time.time - lastAttackStartTime > maxAttackLength;
        }
    }

    public void PerformAttack()
    {
        soundPlayer.PlayAttackSound();
        ATTACK_StartAnimation();
        var allGO = GameObject.FindObjectsOfType(typeof(MonoBehaviour));
    }

    public void ATTACK_StartAnimation()
    {
        string animationName = directions[renderer.LastDirection];
        animator.Play(animationName);
        animationInProgress = true;
        lastAttackStartTime = Time.time;
    }

    public void ATTACK_EndAnimation()
    {
        Debug.Log("Attack ended!");
        animationInProgress = false;
    }

    public string[] directions;

    protected static float maxAttackLength = 0.5f;
}
