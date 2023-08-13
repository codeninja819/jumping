using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;
using Newtonsoft.Json;

public class UserInfo
{
    public int id;
    public string name;
    public int coin;
    public int earnedCoin;
    public int highscore;
    public string wallet;
}

public class RequestData
{

}

public class LoginRequestData : RequestData
{
    public string name;
    public string code;
    public string wallet;
    public bool is_nft_user;
}

public class RegisterRequestData : RequestData
{
    public string wallet;
    public string name;
    public string code;
}

public class JumpStartRequestData : RequestData
{
    public int id;
}

public class JumpEndRequestData : RequestData
{
    public int id;
    public int score;
}

public class ResponseData
{
    public int ret;
    public string msg;
}

public class LoginResponseData : ResponseData
{
    public int id;
    public string name;
    public int coin;
    public int highscore;
    public string wallet;
}

public class JumpStartResponseData : ResponseData
{
    public int coin;
}

public class JumpEndResponseData : ResponseData
{
    public int highscore;
    public int earnedcoin;
    public int coin;
}

public class HighScoreItem
{
    public int id;
    public int index;
    public string name;
    public int high_score;
}

public class HighScoreList : ResponseData
{
    public List<HighScoreItem> list = new List<HighScoreItem>();
}

public class Network : MonoBehaviour
{
    public static Network instance = null;
    public UserInfo userInfo = new UserInfo();
    SocketIOController socket;
    RegisterRequestData registerReqData = new RegisterRequestData();
    LoginRequestData loginReqData = new LoginRequestData();
    JumpStartRequestData jumpStartReqData = new JumpStartRequestData();
    JumpEndRequestData jumpEndReqData = new JumpEndRequestData();

    RequestData lastRequestData = new RequestData();
    string lastReqId;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        socket = gameObject.GetComponent<SocketIOController>();
        socket.On("connect", (SocketIOEvent e) => {
            Debug.Log("connected");
            UIManager.instance.loadingScreen.SetActive(false);

            if(!string.IsNullOrEmpty(lastReqId))
            {
                socket.Emit(lastReqId, JsonConvert.SerializeObject(lastRequestData));
            }
        }            
        );
        socket.On("disconnect", (SocketIOEvent e) => {
            Debug.Log("disconnect");
            UIManager.instance.loadingScreen.SetActive(true);
        }
        );
        socket.On("LOGIN_RESULT", LoginResult);
        socket.On("REGISTER_RESULT", RegisterResult);
        socket.On("JUMP_START_RESULT", JumpStartResult);
        socket.On("JUMP_END_RESULT", JumpEndResult);
        socket.On("USER_HIGH_SCORES_RESULT", UserHighScoresResult);

        UIManager.instance.loadingScreen.SetActive(true);
        socket.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }    

    public void LoginRequest(string name, string code)
    {
        loginReqData.name = name;
        loginReqData.code = code;
        loginReqData.wallet = WalletScreen.address;
        loginReqData.is_nft_user = WalletScreen.isNFTOwner;        
        socket.Emit("USER_LOGIN", JsonConvert.SerializeObject(loginReqData));

        lastRequestData = loginReqData;
        lastReqId = "USER_LOGIN";
        
    }

    public void LoginByWalletRequest()
    {
        loginReqData.wallet = WalletScreen.address;
        loginReqData.is_nft_user = WalletScreen.isNFTOwner;
        socket.Emit("USER_LOGIN_BY_WALLET", JsonConvert.SerializeObject(loginReqData));

        lastRequestData = loginReqData;
        lastReqId = "USER_LOGIN_BY_WALLET";
    }

    public void RegisterRequest(string wallet, string name, string code)
    {
        registerReqData.wallet = wallet;
        registerReqData.name = name;
        registerReqData.code = code;

        socket.Emit("USER_REGISTER", JsonConvert.SerializeObject(registerReqData));

        lastRequestData = registerReqData;
        lastReqId = "USER_REGISTER";
    }

    public void JumpStartRequest()
    {
        jumpStartReqData.id = userInfo.id;
        socket.Emit("JUMP_START", JsonConvert.SerializeObject(jumpStartReqData));

        lastRequestData = jumpStartReqData;
        lastReqId = "JUMP_START";
    }

    public void JumpEndRequest(float score)
    {
        jumpEndReqData.id = userInfo.id;
        jumpEndReqData.score = Mathf.RoundToInt(score);
        socket.Emit("JUMP_END", JsonConvert.SerializeObject(jumpEndReqData));

        lastRequestData = jumpEndReqData;
        lastReqId = "JUMP_END";
    }

    private void LoginResult(SocketIOEvent obj)
    {
        Debug.Log("LoginResult: " + obj.data);
        LoginResponseData retObj = JsonConvert.DeserializeObject<LoginResponseData>(obj.data);
        int ret = retObj.ret;        
        if (ret == 0)
        {
            userInfo.id = retObj.id;
            userInfo.name = retObj.name;
            userInfo.coin = retObj.coin;
            userInfo.highscore = retObj.highscore;
            //CanvasScreen.instance.ClearScreen();
            UIManager.instance.heightScreen.Focus();
        }
        else
        {
            string msg = retObj.msg;
            UIManager.instance.ToastMessage(msg);
            //UIManager.instance.canvasScreen.Focus();
            UIManager.instance.loginScreen.Focus();
        }

        lastReqId = "";
    }

    private void RegisterResult(SocketIOEvent obj)
    {
        Debug.Log("RegisterResult: " + obj.data);

        ResponseData retObj = JsonConvert.DeserializeObject<ResponseData>(obj.data);
        int ret = retObj.ret;        

        if (ret == 0)
            LoginRequest(registerReqData.name, registerReqData.code);
        else
        {
            string msg = retObj.msg;
            UIManager.instance.ToastMessage(msg);
        }

        lastReqId = "";
    }

    private void JumpStartResult(SocketIOEvent obj)
    {
        Debug.Log("JumpStartResult: " + obj.data);
        JumpStartResponseData retObj = JsonConvert.DeserializeObject<JumpStartResponseData>(obj.data);
        int ret = retObj.ret;

        if (ret == 0)
        {
            userInfo.coin = retObj.coin;
            UIManager.instance.tapScreen.Focus();
        }
        else 
        {
            string msg = retObj.msg;
            UIManager.instance.ToastMessage(msg);
        }

        lastReqId = "";
    }

    private void JumpEndResult(SocketIOEvent obj)
    {
        Debug.Log("JumpEndResult: " + obj.data);
        JumpEndResponseData retObj = JsonConvert.DeserializeObject<JumpEndResponseData>(obj.data);
        int ret = retObj.ret;

        if (ret == 0)
        {
            userInfo.highscore = retObj.highscore;
            userInfo.coin = retObj.coin;
            userInfo.earnedCoin = retObj.earnedcoin;
            UIManager.instance.resultScreen.Focus();
        }
        else
        {
            string msg = retObj.msg;
            UIManager.instance.ToastMessage(msg);
        }

        lastReqId = "";
    }

    private void UserHighScoresResult(SocketIOEvent obj)
    {
        Debug.Log("JumpEndResult: " + obj.data);
        HighScoreList retObj = JsonConvert.DeserializeObject<HighScoreList>(obj.data);
        int ret = retObj.ret;
        if(ret == 0)
        {
            List<HighScoreItem> list = retObj.list;
            ResultScreen.instance.ShowDailyHighScoreList(list);
        }
        else
        {
            string msg = retObj.msg;
            UIManager.instance.ToastMessage(msg);
        }

        lastReqId = "";
    }
}
