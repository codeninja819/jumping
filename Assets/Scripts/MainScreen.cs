using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : UIScreen
{
    public static MainScreen instance = null;
    public UILabel scoreLabel;
    public UILabel highScoreLabel;
    public UILabel coinLabel;

    private void Awake()
    {
        instance = this;
    }
    public override void Init()
    {
        scoreLabel.text = "0";
        highScoreLabel.text = Network.instance.userInfo.highscore.ToString();
        coinLabel.text = Network.instance.userInfo.coin.ToString();
        float initTrust = TapScreen.instance.GetThrowPowerByArctan();
        JumpPlayer.instance.Init(initTrust);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopButtonClicked()
    {
        GameManager.instance.Restart();
    }

    public void RefreshDistance(float distance)
    {
        scoreLabel.text = string.Format("{0}", Mathf.RoundToInt(distance));
    }
}
