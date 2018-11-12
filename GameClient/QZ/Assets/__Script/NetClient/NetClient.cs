using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

class NetClient : MonoBehaviour
{
    public static NetClient Instance = null;

    public InputField inputField = null;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        string sysform = Environment.OSVersion.Platform.ToString();
        string ip = "169.254.149.26";
//#if UNITY_EDITOR

//        if (sysform.IndexOf("Win") >= 0)
//        {
//            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
//            {
//                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
//                {
//                    ip = _IPAddress.ToString();
//                }
//            }
//        }
//#else
//        ip = "192.168.2.198";
//#endif
       
        BattleProtocol.InitProtocol();
        ProcessManager.Instance.Init();
        GameSceneManager.Instance.Init();

        TableManager.Instance.Init();

       

    }

    private void Update()
    {
        ClientBattle.Instance.Update();
        TimeManager.Instance.Update();
        GameUnitManager.Instance.OnUpdate(Time.deltaTime);
    }

    private void OnDestroy()
    {
        ClientBattle.Instance.OnClose();
    }

    private void onConnect(bool flag)
    {
        ProcessManager.Instance.Begin(ProcessType.processstart);
    }

    public void OnClicekServer()
    {
        ClientBattle.Instance.NetConnect(inputField.text, 8000, onConnect);
    }
}