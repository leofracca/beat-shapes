using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    // It holds the spectrum data
    private float[] spectrumData;

    public static float spectrumValue { get; set; }

    void Start()
    {
        spectrumData = new float[128];
    }

    void Update()
    {
        // Fill the spectrumData array
        AudioListener.GetSpectrumData(spectrumData, 0, FFTWindow.Hamming);

        if (spectrumData != null && spectrumData.Length > 0)
        {
            spectrumValue = spectrumData[0] * 100;
        }
    }
}
