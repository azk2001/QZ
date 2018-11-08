using System;
using System.Collections;
using System.Collections.Generic;

public delegate void VoidByteReaderDelegate(BytesReader reader);

public class ProtocolBattleManager
{
    private static Dictionary<int, VoidByteReaderDelegate> mProtocolEventTable = new Dictionary<int, VoidByteReaderDelegate>();

    public static void AddListener(int protocolKey, VoidByteReaderDelegate kHandler)
    {
        lock (mProtocolEventTable)
        {
            if (!mProtocolEventTable.ContainsKey(protocolKey))
            {
                mProtocolEventTable.Add(protocolKey, null);
            }

            mProtocolEventTable[protocolKey] = (VoidByteReaderDelegate)mProtocolEventTable[protocolKey] + kHandler;
        }
    }


    public static void RemoveListener(int protocolKey, VoidByteReaderDelegate kHandler)
    {
        lock (mProtocolEventTable)
        {
            if (mProtocolEventTable.ContainsKey(protocolKey))
            {
                mProtocolEventTable[protocolKey] = (VoidByteReaderDelegate)mProtocolEventTable[protocolKey] - kHandler;

                if (mProtocolEventTable[protocolKey] == null)
                {
                    mProtocolEventTable.Remove(protocolKey);
                }
            }
        }
    }

    public static void Invoke(int protocol, BytesReader reader)
    {
        try
        {
            VoidByteReaderDelegate kDelegate;
            if (mProtocolEventTable.TryGetValue(protocol, out kDelegate))
            {
                VoidByteReaderDelegate kHandler = (VoidByteReaderDelegate)kDelegate;

                if (kHandler != null)
                {
                    kHandler(reader);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public static void UnInit()
    {
        mProtocolEventTable.Clear();
    }

    internal static void AddListener(int s2C_StartGame, object , object receiveStartGame)
    {
        throw new NotImplementedException();
    }

    internal static void AddListener(int s2C_StartBattle, object , object receiveStartBattle)
    {
        throw new NotImplementedException();
    }
}