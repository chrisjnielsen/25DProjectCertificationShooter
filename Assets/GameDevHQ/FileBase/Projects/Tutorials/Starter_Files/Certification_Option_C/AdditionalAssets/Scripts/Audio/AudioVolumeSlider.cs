using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioVolumeSlider : MonoBehaviour
{

    public AudioMixer mixer;

    public void SetLevel(float sliderValue)
    {

        mixer.SetFloat("VolumeParam", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("Volume", sliderValue);
    }

}
