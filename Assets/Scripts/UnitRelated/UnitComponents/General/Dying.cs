using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dying : MonoBehaviour
{
    public bool InitiallyDead;

    public bool Dead { get; set; }
    protected Animator animator
    {
        get
        {
            return dyingAnimation.animator;
        }
    }

    protected ActionAnimation dyingAnimation;
    protected CharacterSoundsPlayer soundPlayer;
    protected UnitSelection selection;
    protected CharacterRenderer renderer;
    protected CharacterMovement movement;
    protected ComponentsSet switchOffAbleComponents;

    // Start is called before the first frame update
    void Awake()
    {
        Dead = InitiallyDead;
        var animator = GetComponent<Animator>();
        renderer = GetComponent<CharacterRenderer>();
        selection = GetComponent<UnitSelection>();
        dyingAnimation = new ActionAnimation(animator, renderer, "Dying", 0.5f);
        soundPlayer = GetComponentInChildren<CharacterSoundsPlayer>();
        movement = GetComponent<CharacterMovement>();
        switchOffAbleComponents = new ComponentsSet(this,
            "Attack",
            "CharacterMovement",
            "ClickCatching",
            "UnitSelection",
            "Navigator");
    }

    // Update is called once per frame
    void Update()
    {
        if (dyingAnimation.ShouldBeEnded)
        {
            dyingAnimation.End();
        }
    }

    public virtual void Die()
    {
        if (Dead)
            return; //can't die twice

        movement.CurrentDirection = new Vector2(0, 0);
        selection.Deselect();
        soundPlayer.PlayDyingSound();
        dyingAnimation.Start();
        TurnOffComponents();
        TurnOffChildGameObjects();
        Dead = true;
        Debug.Log("DEAD!");
    }

    public virtual bool ShouldBeDead(HealthInfo info)
    {
        bool res = info.CurHP <= 0f;
        if (res)
            Die();
        return res;
    }

    public void EndDyingAnimation()
    {
        dyingAnimation.End();

        animator.Play($"{renderer.PreparedAnimationPrefix}Dead");
        
    }

    void TurnOffComponents()
    {
        switchOffAbleComponents.SetActive(false);
    }

    void TurnOffChildGameObjects()
    {
        foreach (Transform childTransform in transform)
        {
            var go = childTransform.gameObject;
            if (!go.name.ToLower().Contains("soundplayer"))
                go.SetActive(false);
        }
    }
}

public class HealthInfo
{
    public float CurHP { get; set; }

    public float MaxHP {get; set; }
}
