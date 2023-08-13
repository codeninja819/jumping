using Thirdweb;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WalletScreen : UIScreen
{
    private ThirdwebSDK sdk;
    public static string address = "";
    public static float balanceNFT = 0;
    public static bool isNFTOwner = false;

    public string rpcURL;
    public string tokenContractAddress;
    public string tokenContractABI;
    public string tokenId;
    public string testWalletAddress = "0x9E67B60863039a813BC98b0962cb756Ec2bD58BC";
    public override void Init()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        sdk = new ThirdwebSDK(rpcURL);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void ConnectWallet()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        address = testWalletAddress;
        isNFTOwner = true;
#else
        address = await sdk.wallet.Connect(new WalletConnection() { 
            provider = WalletProvider.MetaMask,
            chainId = 5
        });

        Debug.Log("wallet address: " + address);
        string balanceString = await CheckBalance();
        balanceNFT = float.Parse(balanceString);
        isNFTOwner = balanceNFT > 0;
#endif
        if (isNFTOwner)
            Network.instance.LoginByWalletRequest();
        else
            UIManager.instance.loginScreen.Focus();
    }

    public void PlayButtonClicked()
    {
        //UIManager.instance.canvasScreen.Focus();
        UIManager.instance.loginScreen.Focus();
    }

    public async Task<string> CheckBalance()
    {
        Contract contract = sdk.GetContract(tokenContractAddress, tokenContractABI);
        string balance = await contract.ERC1155.BalanceOf(address, tokenId);
        return balance;
    }
}
