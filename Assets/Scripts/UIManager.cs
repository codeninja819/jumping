using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public UIScreen walletScreen;
    public UIScreen loginScreen;
    public UIScreen registerScreen;
    public UIScreen heightScreen;
    public UIScreen tapScreen;
    public UIScreen mainScreen;
    public UIScreen resultScreen;
    public UIScreen canvasScreen;

    public GameObject loadingScreen;
    public UILabel toastMessageLabel;
    public TMPro.TextMeshProUGUI toastMessage;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        walletScreen.Focus();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToastMessage(string message)
    {
        toastMessage.transform.localScale = Vector3.zero;
        toastMessage.text = message;
        iTween.Stop();

        Hashtable hash = new Hashtable();
        hash.Add("scale", Vector3.one);
        hash.Add("time", .5f);
        hash.Add("islocal", true);
        iTween.ScaleTo(toastMessage.gameObject, hash);

        hash.Clear();
        hash.Add("scale", Vector3.zero);
        hash.Add("delay", 2f);
        hash.Add("time", 1f);
        hash.Add("islocal", true);
        iTween.ScaleTo(toastMessage.gameObject, hash);
    }
}
