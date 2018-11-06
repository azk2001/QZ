using BattleClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

class NetClient :MonoBehaviour
{
    public static NetClient Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        string sysform = Environment.OSVersion.Platform.ToString();

        string ip = string.Empty;
        if (sysform.IndexOf("Win") >= 0)
        {
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    ip = _IPAddress.ToString();
                }
            }
        }

        ClientBattle.Instance.NetConnect(ip, 8000, onConnect);
        BattleProtocol.InitProtocol();

    }

    private void Update()
    {
        ClientBattle.Instance.Update();
    }

    private void OnDestroy()
    {
        ClientBattle.Instance.OnClose();
    }

    private void onConnect(bool flag)
    {
        Debug.Log("连接返回结果：" + flag);
    }
}