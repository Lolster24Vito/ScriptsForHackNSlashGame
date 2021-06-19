using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Audio Events/Grunt")]
public class GruntAudioEvent : AudioEvent
{

    public AudioClip[] clips;

    public float volumeMin;
    public float volumeMax;
    bool ranOnce = false;

    [Range(0, 2)]
    public float pitchMinValue;
    [Range(0, 2)]
    public float pitchMaxValue;
    public void setPitchAndVolume(AudioSource source)
    {
        source.volume = Random.Range(volumeMin, volumeMax);
        source.pitch = Random.Range(pitchMinValue, pitchMaxValue);
    }
    public override void Play(AudioSource source)
    {

        if (clips.Length == 0) return;
        if (!ranOnce)
        {
            setPitchAndVolume(source);
            ranOnce = true;
        }
        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();
    }
}
