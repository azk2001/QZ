using System;
using System.Collections.Generic;

namespace GameServer
{
    // -------------------事件添加在下面---------------------------
    public enum EventEnum
    {
        onDungeonRunTime,    //副本游戏时间发生变化;
        gameUnitReduceLife, //角色血量发生变化事件,
        gameUnitBirth,      //角色出生事件;
        gameUnitDeath,      //角色死亡事件;
        gameUnitDestory,    //删除角色事件;
        battleElementFinish,//刷怪器全部触发
    };

    // -------------------------------------------------------------------------------------

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
                MyDebug.WriteLine(ex);
            }
        }

        public static void UnInit()
        {
            mProtocolEventTable.Clear();
        }
    }
}
