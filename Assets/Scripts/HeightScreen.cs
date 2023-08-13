using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightScreen : UIScreen
{
    public static HeightScreen instance = null;
    public static float f_JumpAngle;
    public float f_JumpAngleMin = 30;
    public float f_JumpAngleMax = 75;
    public UISlider heightSlider;
    public UILabel walletInfoLabel;
    public UILabel nameLabel;
    public UILabel coinLabel;
    public UILabel highScoreLabel;

    private void Awake()
    {
        instance = this;
    }
    public override void Init()
    {
        string walletInfo = "Wallet Address\n(" + WalletScreen.address + ")" + (WalletScreen.balanceNFT > 0 ? "\nNFT Owner" : "");
        walletInfo = "";
        walletInfoLabel.text = walletInfo;
        nameLabel.text = Network.instance.userInfo.name;
        coinLabel.text = string.Format("{0}", Network.instance.userInfo.coin);
        highScoreLabel.text = Network.instance.userInfo.highscore.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JumpButtonClicked()
    {
        f_JumpAngle = f_JumpAngleMin + (f_JumpAngleMax - f_JumpAngleMin) * heightSlider.value;
        Network.instance.JumpStartRequest();
#if UNITY_WEBGL && !UNITY_EDITOR
        GameManager.turnOnWakeLock();
#endif
    }

    public void LogoutButtonClicked()
    {
        Network.instance.userInfo.id = 0;
        WalletScreen.address = "";
        GameManager.instance.Restart();
        UIManager.instance.walletScreen.Focus();
    }
}
