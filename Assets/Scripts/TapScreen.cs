using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapScreen : UIScreen
{
    public static TapScreen instance = null;
    public UILabel tapCountLabel;
    public UISlider timerSlider;
    public UILabel coinLabel;
    public UILabel highScoreLabel;
    public int timeLimit = 5;
    public static int tapCount = 0;

    [SerializeField]
    private float f_MinPower = 800;
    [SerializeField]
    private float f_MaxPower = 2000;
    [SerializeField]
    private int minTapCount = 10;
    [SerializeField]
    private int maxTapCount = 70;
    [SerializeField]
    private int limitTapCount = 80;
    [SerializeField]
    private float f_LimitPower = 2700;


    private void Awake()
    {
        instance = this;
    }
    public override void Init()
    {
        timerSlider.value = 1;
        tapCount = 0;
        coinLabel.text = string.Format("{0}", Network.instance.userInfo.coin);
        highScoreLabel.text = Network.instance.userInfo.highscore.ToString();
        RefreshTapCount();
        StartCoroutine(tapRoutine());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            //tapCount++;
            RefreshTapCount();
        }
    }

    public void RefreshTapCount()
    {
        tapCountLabel.text = tapCount.ToString();
    }
    IEnumerator tapRoutine()
    {
        for (int i = timeLimit - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(1f);
            timerSlider.value = i * (1f / timeLimit);
        }
        yield return new WaitForSeconds(1f);
        UIManager.instance.mainScreen.Focus();
    }

    public void TapClicked()
    {
        tapCount++;
        RefreshTapCount();
    }

    public float GetThrowPower()
    {
        float throwPower = f_MinPower;

        if(tapCount < minTapCount)
        {
            throwPower = (f_MinPower / minTapCount) * tapCount;
        }else if (tapCount < maxTapCount)
        {
            float powerUnit = (f_MaxPower - f_MinPower) / (maxTapCount - minTapCount);
            throwPower = f_MinPower + powerUnit * (tapCount - minTapCount);
        }
        else
        {
            throwPower = f_MaxPower;
        }
            

        return throwPower;
    }

    public float GetThrowPowerByArctan()
    {
        float throwPower = Mathf.Atan(3.0f / limitTapCount * tapCount);
        throwPower *= 2 / Mathf.PI * f_LimitPower;

        return throwPower;
    }
}
