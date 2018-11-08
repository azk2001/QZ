﻿using System;
using System.Collections.Generic;

namespace BattleServer
{
    public enum S2CBattleProtocol
    {
        S2C_Ping = 0,

        S2C_Connected,          //连接上
        S2C_Login,              //登录返回
        S2C_CreatePlayer,       //创建角色;
        S2C_GetRoom,            //获取房间;
        S2C_CreateRoom,         //创建房间;
        S2C_AddRoom,            //添加房间;
        S2C_StartGame,
        S2C_StartBattle,
        S2C_PlayerMove,
        S2C_PlayerSkill,
        S2C_PlayerHit,
        S2C_PlayerAddBuff,
        S2C_PlayerRemoveBuff,
        S2C_PlayerRefreshBuff,
        S2C_PlayerDie,


        Count,
    }

    public enum C2SBattleProtocol
    {
        C2S_Ping = 0,

        C2S_Login,              //登录
        C2S_CreatePlayer,       //创建角色;
        C2S_GetRoom,            //获取房间;
        C2S_CreateRoom,         //创建房间;
        C2S_AddRoom,            //添加房间;
        C2S_StartGame,
        C2S_StartBattle,
        C2S_PlayerMove,
        C2S_PlayerSkill,
        C2S_PlayerHit,
        C2S_PlayerAddBuff,
        C2S_PlayerRemoveBuff,
        C2S_PlayerDie,

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


            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_Login, BattleProtocolEvent.ReceiveLogin);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_CreatePlayer, BattleProtocolEvent.ReceiveCreatePlayer);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_GetRoom, BattleProtocolEvent.ReceiveGetRoom);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_CreateRoom, BattleProtocolEvent.ReceiveCreateRoom);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_AddRoom, BattleProtocolEvent.ReceiveAddRoom);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_StartGame, BattleProtocolEvent.ReceiveStartGame);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_StartBattle, BattleProtocolEvent.ReceiveStartBattle);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_PlayerMove, BattleProtocolEvent.ReceivePlayerMove);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_PlayerSkill, BattleProtocolEvent.ReceivePlayerSkill);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_PlayerHit, BattleProtocolEvent.ReceivePlayerHit);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_PlayerAddBuff, BattleProtocolEvent.ReceivePlayerAddBuff);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_PlayerRemoveBuff, BattleProtocolEvent.ReceivePlayerRemoveBuff);
            ProtocolBattleManager.AddListener((int)C2SBattleProtocol.C2S_PlayerDie, BattleProtocolEvent.ReceivePlayerDie);


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
