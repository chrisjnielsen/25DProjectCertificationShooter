using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessEffect : MonoBehaviour
{

    private float alpha, alphaValue;
    [SerializeField]
    private Image _image;


    // Start is called before the first frame update
    void Start()
    {
        alpha = PlayerPrefs.GetFloat("Brightness");
        _image = this.GetComponent<Image>();
        if (alpha == 0)
        {
            alpha = 1;
        }
        AdjustBrightness();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustBrightness()
    {
        alphaValue = 1f-alpha;
        Color color = new Color(0, 0, 0, alphaValue);
        _image.color = color;
    }


}
