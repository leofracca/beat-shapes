using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    public float bias; // The spectrum value that triggers the beat if the spectrum data goes above or below
    public float timeStep; // The minimun interval between each beat
    public float timeToBeat; // The time that an object uses to do the action it needs to do
    public float restSmoothTime; // The time that an object uses to return to his original shape/color after the beat

    // To determine if the value is above or below the bias
    private float previousAudioValue;
    private float audioValue;
    private float timer; // Keep track of the time step interval

    protected bool isBeat; // True if there is a beat, false otherwise

    void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        previousAudioValue = audioValue;
        audioValue = AudioSpectrum.spectrumValue;

        if (previousAudioValue > bias && audioValue <= bias)
        {
            if (timer > timeStep)
            {
                OnBeat();
            }
        }

        if (previousAudioValue <= bias && audioValue > bias)
        {
            if (timer > timeStep)
            {
                OnBeat();
            }
        }

        timer += Time.deltaTime;
    }

    // Handle the beat
    public virtual void OnBeat()
    {
        timer = 0;
        isBeat = true;
    }
}
