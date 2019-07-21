using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float AttackDistance = 0.9f;
    public CharacterSoundsPlayer SoundPlayer;

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
    

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<CharacterRenderer>();
        animationInProgress = false;
        directions = new string[]
        {
            renderer.AnimationPrefix + "Attack N",
            renderer.AnimationPrefix + "Attack NE",
            renderer.AnimationPrefix + "Attack E",
            renderer.AnimationPrefix + "Attack SE",
            renderer.AnimationPrefix + "Attack S",
            renderer.AnimationPrefix + "Attack SW",
            renderer.AnimationPrefix + "Attack W",
            renderer.AnimationPrefix + "Attack NW"
        };
    }

    // Update is called once per frame
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
        SoundPlayer.PlayAttackSound();
        ATTACK_StartAnimation();
        var allGO = GameObject.FindObjectsOfType(typeof(MonoBehaviour));
        foreach ()
    }

    bool PointIsInAttackZone(Vector2 point)
    {

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
