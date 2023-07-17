using UnityEngine;
using Pautik;

public static class SoundControllerExtensions 
{
    public static AudioSource GetMusicSource(this SoundController soundController)
    {
        return Get<AudioSource>.From(soundController.transform.Find("MusicSRC").gameObject);
    }
}