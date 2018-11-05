using System;
using System.Collections.Generic;

public delegate void VoidByteNetUserDelegate(BytesReader arg,Int32 ClinetConnectId);

static public class ProtocolBattleManager
{
    private static Dictionary<int, VoidByteNetUserDelegate> mProtocolEventTable = new Dictionary<int, VoidByteNetUserDelegate>();

    public static void AddListener(int protocolKey, VoidByteNetUserDelegate kHandler)
    {
        lock (mProtocolEventTable)
        {
            if (!mProtocolEventTable.ContainsKey(protocolKey))
            {
                mProtocolEventTable.Add(protocolKey, null);
            }

            mProtocolEventTable[protocolKey] = (VoidByteNetUserDelegate)mProtocolEventTable[protocolKey] + kHandler;
        }
    }


    public static void RemoveListener(int protocolKey, VoidByteNetUserDelegate kHandler)
    {
        lock (mProtocolEventTable)
        {
            if (mProtocolEventTable.ContainsKey(protocolKey))
            {
                mProtocolEventTable[protocolKey] = (VoidByteNetUserDelegate)mProtocolEventTable[protocolKey] - kHandler;

                if (mProtocolEventTable[protocolKey] == null)
                {
                    mProtocolEventTable.Remove(protocolKey);
                }
            }
        }
    }

    public static void Invoke(int protocolKey, BytesReader args, Int32 ClinetConnectId)
    {
        try
        {
            VoidByteNetUserDelegate kDelegate;
            if (mProtocolEventTable.TryGetValue(protocolKey, out kDelegate))
            {
                VoidByteNetUserDelegate kHandler = (VoidByteNetUserDelegate)kDelegate;

                if (kHandler != null)
                {
                    kHandler(args, ClinetConnectId);
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