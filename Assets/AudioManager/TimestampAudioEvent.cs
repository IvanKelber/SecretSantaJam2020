using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Audio Events/Timestamp")]
public class TimestampAudioEvent : AudioEvent
{
    public AudioClip clip;

    [Range(0,1)]
    public float volume;

    public override void Play(AudioSource source, float masterVolume) {
        PlayFromPercentage(source, masterVolume, 0);
    }

    public override void PlayFromPercentage(AudioSource source, float masterVolume, float percentage) {
        if(source == null) {
            Debug.LogWarning("Source is null");
            return;
        }
        if(clip == null) {
            Debug.LogWarning("AudioClip " + Name + " has no clip");
            return;
        }
        source.volume = volume * masterVolume;
        source.pitch = 1;
        source.clip = clip;
        source.time = clip.length * percentage;
        source.Play();
    }
}
