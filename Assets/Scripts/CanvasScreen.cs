using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScreen : UIScreen
{
    public static CanvasScreen instance = null;
    public GameObject loginScreenObj;
    public GameObject registerScreenObj;
    private void Awake()
    {
        instance = this;
    }

    public override void Init()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
