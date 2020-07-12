using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip backgroundMusic, baselineChatter, maxChatter, beerClink, cheers;
    private AudioSource[] audioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponents<AudioSource>();
        foreach(AudioSource adsc in audioPlayer)
        {
            if (adsc.clip != backgroundMusic)
            {
                adsc.volume = 0f;
            }
            adsc.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
                
    }
}
