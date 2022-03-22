using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightnessSlider : MonoBehaviour
{  
    public void SetBrightness(float brightness)
    {
        
        PlayerPrefs.SetFloat("Brightness", brightness);
    }

}
