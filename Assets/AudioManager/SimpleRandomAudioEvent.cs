using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Audio Events/SimpleRandom")]
public class SimpleRandomAudioEvent : AudioEvent
{
    public List<AudioClip> clips;

    [Range(0,1)]
    public float minVolume = 1;

    [Range(0,1)]
    public float maxVolume = 1;

    [Range(-1,1)]
    public float minPitch = 1;

    [Range(-1,1)]
    public float maxPitch = 1;

    float pitch;
    float volume;


    public override void Play(AudioSource source, float masterVolume) {
        if(source == null) {
            Debug.LogWarning("Source is null");
            return;
        }
        if(clips.Count == 0) {
            Debug.LogWarning("AudioClip " + Name + " has no clip");
            return;
        }
        pitch = 1 + Random.Range(minPitch, maxPitch);
        volume = Random.Range(minVolume, maxVolume) * masterVolume;
        source.volume = volume;
        source.pitch = pitch;
        source.clip = clips[Random.Range(0,clips.Count - 1)];
        source.Play();
    }
}
