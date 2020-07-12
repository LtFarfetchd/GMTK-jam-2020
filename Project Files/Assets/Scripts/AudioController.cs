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
        float temp = 0f;
        float audioLayerPercent = 100/5;
        if (temp < audioLayerPercent)
        {
            changeAudioVolume(baselineChatter, temp / audioLayerPercent);
        }
        else if(temp < audioLayerPercent *2)
        {
            changeAudioVolume(beerClink, (temp - audioLayerPercent) / audioLayerPercent);
        }
        else if(temp < audioLayerPercent *3)
        {
            changeAudioVolume(maxChatter, (temp - audioLayerPercent * 2) / audioLayerPercent);
        }
        else if(temp < audioLayerPercent *4)
        {
            changeAudioVolume(maxChatter, (temp - audioLayerPercent * 3) / audioLayerPercent);
        }
    }

    private void changeAudioVolume(AudioClip cliptochange, float audiolevel = 0f)
    {
        foreach(AudioSource adsc in audioPlayer)
        {
            if (adsc.clip == cliptochange)
            {
                adsc.volume = audiolevel;
            }
        }
    }
}
