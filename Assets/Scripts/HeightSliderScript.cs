using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightSliderScript : MonoBehaviour
{
    public UILabel heightLabel;
    UISlider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<UISlider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SliderValudeChanged()
    {
        HeightScreen heightScreen = HeightScreen.instance;
        heightLabel.text = Mathf.RoundToInt(heightScreen.f_JumpAngleMin + slider.value * (heightScreen.f_JumpAngleMax - heightScreen.f_JumpAngleMin)).ToString();
    }
}
