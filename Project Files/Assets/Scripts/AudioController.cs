using UnityEngine;

public class AudioController : MonoBehaviour
{
    public GameObject hud;
    public AudioClip backgroundMusic;
    public AudioClip[] peripheralClips;
    public AudioSource[] peripheralAudioSources;
    private HUDController hudController;

    void Start()
    {
        hudController = (HUDController)hud.GetComponent<MonoBehaviour>();
        for (int i = 0; i < peripheralClips.Length; i++)
        {
            peripheralAudioSources[i].clip = peripheralClips[i];
            peripheralAudioSources[i].volume = 0;
            peripheralAudioSources[i].Play();
        }
    }

    void Update()
    {
        float noisiness = hudController.GetNoisiness();
        float audioLayerPercent = 100f/(peripheralClips.Length + 1);

        for (int i = 0; i < peripheralClips.Length && i < (int)(noisiness/audioLayerPercent); i++)
        {
            peripheralAudioSources[i].volume = (noisiness - (audioLayerPercent * (float)(i+1))) / audioLayerPercent;
        }
    }
}
