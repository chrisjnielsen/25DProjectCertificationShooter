using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySlider : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    
    // Start is called before the first frame update
    void Start()
    {
        _slider = this.GetComponent<Slider>();
        
    }

    public void SetDifficulty()
    {
        
        PlayerPrefs.SetInt("Difficulty", (int)_slider.value);
        
    }

}
