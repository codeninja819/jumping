using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public EnvironmentManager rootEnvmanager;
    public EnvironmentManager firstSubEnvmanager;

    [DllImport("__Internal")]
    public static extern void turnOnWakeLock();

    [DllImport("__Internal")]
    public static extern void turnOffWakeLock();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        rootEnvmanager.Init();
        firstSubEnvmanager.InitiateTrampolines();
        JumpPlayer.instance.Reset();        
    }
}
