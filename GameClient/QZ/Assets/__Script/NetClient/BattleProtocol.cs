using System.Collections.Generic;

public enum S2CBattleProtocol
{
    S2C_Ping = 0,

    S2C_Connected,          //连接上
    S2C_Login,              //登录返回

    Count,
}

public enum C2SBattleProtocol
{
    C2S_Ping = 0,

    C2S_Login,              //登录
    C2S_Create,             //创建角色;

    Count,
}

public class BattleProtocol
{
    private static BytesWriter writer = new BytesWriter();
    private static BytesReader reader = new BytesReader();

    public static int UUID = 0;  //客服端连接UUID;

    public static void InitProtocol()
    {
        ClientBattle.Instance.socketInst.dispitcher.OnReceive += OnReceiveMsg;

        ProtocolBattleManager.AddListener((int)S2CBattleProtocol.S2C_Connected, ReceiveConnected);
        ProtocolBattleManager.AddListener((int)S2CBattleProtocol.S2C_Ping, ReceivePing);
        ProtocolBattleManager.AddListener((int)S2CBattleProtocol.S2C_Login, BattleProtocolEvent.ReceiveLogin);
    }


    /// <summary>
    /// 接受客服端发送来的请求
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="clientbase"></param>
    private static void OnReceiveMsg(byte[] buffer)
    {
        reader.ReBytesReader(buffer);
        int protocol = reader.ReadByte();

        ProtocolBattleManager.Invoke(protocol, reader);

    }

    //连接成功;
    public static void ReceiveConnected(BytesReader reader)
    {
        UUID = reader.ReadInt();
    }


    public static void ReceivePing(BytesReader reader)
    {
        int d = reader.ReadInt();
        try
        {


        }
        catch (System.Exception err)
        {

        }
    }

    public static void SendBytes(BytesWriter writer)
    {
        ClientBattle.Instance.Send(writer.GetBuffer(), writer.GetBufferLen());
    }
}