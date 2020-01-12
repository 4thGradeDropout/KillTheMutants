using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundsPlayer : MonoBehaviour
{
    public AudioClip Footsteps;
    public AudioClip AttackSound;
    public AudioClip DyingSound;

    protected bool playingNow;
    protected AudioSource source;
    protected CharacterMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        movement = gameObject.GetComponentInParent<CharacterMovement>();
        source.clip = Footsteps;
        source.loop = true;
        playingNow = false;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (movement.MovingNow())
            TurnFootstepsOn();
        else
            TurnFootstepsOff();
    }

    public void TurnFootstepsOn()
    {
        if (!playingNow)
        {
            source.Play();
            playingNow = true;
        }
    }

    public void TurnFootstepsOff()
    {
        if (playingNow)
        {
            source.Stop();
            playingNow = false;
        }
    }

    public void PlayAttackSound()
    {
        source.PlayOneShot(AttackSound); 
    }

    public void PlayDyingSound()
    {
        source.PlayOneShot(DyingSound);
    }
}
