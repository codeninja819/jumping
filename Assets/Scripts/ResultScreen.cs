using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScreen : UIScreen
{
    public static ResultScreen instance = null;
    public UILabel nameLabel;
    public UILabel coinLabel;
    public UILabel earnedCoinLabel;
    public UILabel scoreLabel;
    public UILabel highScoreLabel;
    [SerializeField]
    Transform dailyHighScoreParent;

    private void Awake()
    {
        instance = this;
    }
    public override void Init()
    {
        UserInfo userInfo = Network.instance.userInfo;
        nameLabel.text = userInfo.name;
        coinLabel.text = string.Format("{0}", userInfo.coin);
        earnedCoinLabel.text = string.Format("{0}", userInfo.earnedCoin);
        scoreLabel.text = MainScreen.instance.scoreLabel.text;
        highScoreLabel.text = string.Format("{0}", userInfo.highscore);

#if UNITY_WEBGL && !UNITY_EDITOR
        GameManager.turnOffWakeLock();
#endif
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartButtonClicked()
    {
        GameManager.instance.Restart();
        UIManager.instance.heightScreen.Focus();
    }

    public void ShowDailyHighScoreList(List<HighScoreItem> itemInfoList)
    {
        HIScoreItem[] itemList = dailyHighScoreParent.GetComponentsInChildren<HIScoreItem>();
        for (int i = 0; i < itemList.Length; i++)
            Destroy(itemList[i].gameObject);

        GameObject hiScoreItemObj = Resources.Load("Prefabs/HIScoreItem") as GameObject;
        for (int i = 0; i < itemInfoList.Count; i++)
        {
            HighScoreItem itemInfo = itemInfoList[i];
            GameObject hiScoreItem = Instantiate(hiScoreItemObj) as GameObject;
            HIScoreItem itemScript = hiScoreItem.GetComponent<HIScoreItem>();
            hiScoreItem.transform.parent = dailyHighScoreParent;
            hiScoreItem.transform.localPosition = new Vector3(0, -28 * i, 0);
            hiScoreItem.transform.localScale = Vector3.one;
            itemScript.Init(itemInfo.index, itemInfo.name, itemInfo.high_score, itemInfo.id == Network.instance.userInfo.id);
        }
    }
}
