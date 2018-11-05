using System;
using System.Collections.Generic;

namespace BattleServer
{
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

        Count,
    }

    public class BattleProtocol
    {
        private static BytesWriter writer = new BytesWriter();
        private static BytesReader reader = new BytesReader();

        public static void InitProtocol()
        {
            Program.socket.ReceiveClientData += OnReceiveMsg;

            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_Ping, ReceivePing);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_Login, BattleProtocolEvent.ReceiveLogin);
        }


        /// <summary>
        /// 接受客服端发送来的请求
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="clientbase"></param>
        private static void OnReceiveMsg(NetClientBase token, byte[] buffer)
        {
            reader.ReBytesReader(buffer);

            int protocol = reader.ReadByte();

            ProtocolBattleManager.Invoke(protocol, reader, token.uuid);

        }

        public static void ReceivePing(BytesReader reader, Int32 ClinetConnectId)
        {
  
        }

        public static void SendBytes(int uuid, BytesWriter writer)
        {
            Program.SendMsgToClient(uuid, writer);
        }

    }
}
