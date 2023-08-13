using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using WebGLSupport;

public class RegisterScreen : UIScreen
{
    public InputField nameInputField;
    public InputField codeInputField;

    /*public InputField nameInputFieldNative;
    public InputField codeInputFieldNative;

    public GameObject windowsInput;
    public GameObject mobileInput;*/
    public override void Init()
    {
        nameInputField.text = "";
        codeInputField.text = "";

        //nameInputFieldNative.text = "";
        //codeInputFieldNative.text = "";

        /*if (Application.isMobilePlatform)
        {
            windowsInput.SetActive(false);
            mobileInput.SetActive(true);
        }
        else
        {
            windowsInput.SetActive(true);
            mobileInput.SetActive(false);
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OkButtonClicked()
    {
        //if (WebGLSupport.WebGLInput.s_InputFocusState)
        //    return;

        string name = nameInputField.text;
        string code = codeInputField.text;

        /*if (Application.isMobilePlatform)
        {
            name = nameInputFieldNative.text;
            code = codeInputFieldNative.text;
        }*/

        Network.instance.RegisterRequest(WalletScreen.address, name, code);
    }

    public void CancelButtonClicked()
    {
        //CanvasScreen.instance.ShowLoginScreen();
        UIManager.instance.loginScreen.Focus();
    }
}
