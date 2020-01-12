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
            return attackAnimation.AnimationInProgress;
        }
    }

    protected ActionAnimation attackAnimation;
    protected CharacterSoundsPlayer soundPlayer;

    protected virtual void Awake()
    {
        var animator = GetComponent<Animator>();
        var renderer = GetComponent<CharacterRenderer>();
        attackAnimation = new ActionAnimation(animator, renderer, "Attack", 0.5f);
        soundPlayer = GetComponentInChildren<CharacterSoundsPlayer>();
    }

    // Call in the end if overridden
    protected virtual void Update()
    {
        if (attackAnimation.ShouldBeEnded)
        {
            //Debug.Log("Ending attack animation from Update!");
            attackAnimation.End();
        }
    }

    public void PerformAttack()
    {
        //Debug.Log("Attack started!");
        soundPlayer.PlayAttackSound();
        attackAnimation.Start();
    }

    public void EndAttackAnimation()
    {
        //Debug.Log("Ending attack animation event fired!");
        attackAnimation.End();
    }
}
