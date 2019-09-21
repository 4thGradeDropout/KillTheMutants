using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerScript : MonoBehaviour
{
    public AudioSource SourceOfMusic;

    public AudioClip BackgroundMusic;
    
    bool PlayingNow { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SourceOfMusic.clip = BackgroundMusic;
        PlayingNow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayingNow)
        {
            SourceOfMusic.Play();
            PlayingNow = true;
        }
    }
}
