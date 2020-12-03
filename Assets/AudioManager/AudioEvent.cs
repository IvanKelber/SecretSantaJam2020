﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioEvent : ScriptableObject
{
    public string Name;

    public abstract void Play(AudioSource source, float masterVolume);
    public virtual WaitForSeconds PlayAndWait(AudioSource source, float masterVolume) {
        Play(source, masterVolume);
        return new WaitForSeconds(source.clip.length);
    }

    public virtual void PlayFromPercentage(AudioSource source, float masterVolume, float percentage) {
        Play(source, masterVolume);
    }
}
