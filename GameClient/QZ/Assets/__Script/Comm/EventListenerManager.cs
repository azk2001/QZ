using System;
using UnityEngine;
using System.Collections.Generic;

// -------------------事件添加在下面---------------------------
public enum EventEnum
{
    getHint = 100000,  
    OPEN_UI,  //打开UI;
    CLOSE_UI,//关闭UI;
    LOCAL_PLAYER_STAER_MOVE_POINT,//本地玩家开始移动;
    LOCAL_PLAYER_END_MOVE_POINT,//本地玩家结束移动;

};


// -------------------------------------------------------------------------------------

public delegate void Callback<T>(T arg1);
static public class EventListenerManager
{
    private static Dictionary<int, Delegate> mProtocolEventTable = new Dictionary<int, Delegate>();

    public static void AddListener(EventEnum protocolKey, Callback<System.Object[]> kHandler)
    {
        AddListener((int)protocolKey, kHandler);
    }

    public static void AddListener(int protocolKey, Callback<System.Object[]> kHandler)
    {
        lock (mProtocolEventTable)
        {
            if (!mProtocolEventTable.ContainsKey(protocolKey))
            {
                mProtocolEventTable.Add(protocolKey, null);
            }

            mProtocolEventTable[protocolKey] = (Callback<System.Object[]>)mProtocolEventTable[protocolKey] + kHandler;
        }
    }

    public static void RemoveListener(EventEnum protocolKey, Callback<System.Object[]> kHandler)
    {
        RemoveListener((int)protocolKey, kHandler);
    }

    public static void RemoveListener(int protocolKey, Callback<System.Object[]> kHandler)
    {
        lock (mProtocolEventTable)
        {
            if (mProtocolEventTable.ContainsKey(protocolKey))
            {
                mProtocolEventTable[protocolKey] = (Callback<System.Object[]>)mProtocolEventTable[protocolKey] - kHandler;

                if (mProtocolEventTable[protocolKey] == null)
                {
                    mProtocolEventTable.Remove(protocolKey);
                }
            }
        }
    }

    public static void Invoke(EventEnum protocolKey, params System.Object[] args)
    {
        Invoke((int)protocolKey, args);
    }

    public static void Invoke(int protocolKey, params System.Object[] args)
    {
        try
        {
            Delegate kDelegate;
            if (mProtocolEventTable.TryGetValue(protocolKey, out kDelegate))
            {
                Callback<System.Object[]> kHandler = (Callback<System.Object[]>)kDelegate;

                if (kHandler != null)
                {
                    kHandler(args);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void UnInit()
    {
        mProtocolEventTable.Clear();
    }
}
