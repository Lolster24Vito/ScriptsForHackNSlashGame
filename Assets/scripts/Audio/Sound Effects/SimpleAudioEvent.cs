using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
  
    public AudioClip[] clips;

    public float volumeMin;
    public float volumeMax;


    [Range(0, 2)]
    public float pitchMinValue;
    [Range(0, 2)]
    public float pitchMaxValue;

    public override void Play(AudioSource source)
    {
        if (clips.Length == 0) return;

        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = Random.Range(volumeMin, volumeMax);
        source.pitch = Random.Range(pitchMinValue, pitchMaxValue);
        source.Play();
    }
}
