using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapButton : MonoBehaviour
{
    public float f_TapDuration = 0.05f;
    [HideInInspector]
    public float f_TapTime;
    // Start is called before the first frame update
    void Start()
    {
        f_TapTime = f_TapDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (f_TapTime > 0)
            f_TapTime -= Time.deltaTime;
    }

    void OnPress(bool isDown)
    {
        if(f_TapTime <= 0)
        {
            if (isDown)
                TapScreen.instance.TapClicked();

            f_TapTime = f_TapDuration;
        }
        
    }
}
