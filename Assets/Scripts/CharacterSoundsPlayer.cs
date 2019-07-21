using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundsPlayer : MonoBehaviour
{
    public AudioSource Source;

    public AudioClip Footsteps;

    public AudioClip AttackSound;

    bool PlayingNow { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Source.clip = Footsteps;
        Source.loop = true;
        PlayingNow = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnFootstepsOn()
    {
        if (!PlayingNow)
        {
            Source.Play();
            PlayingNow = true;
        }
    }

    public void TurnFootstepsOff()
    {
        if (PlayingNow)
        {
            Source.Stop();
            PlayingNow = false;
        }
    }

    public void PlayAttackSound()
    {
        Source.PlayOneShot(AttackSound); 
    }
}
