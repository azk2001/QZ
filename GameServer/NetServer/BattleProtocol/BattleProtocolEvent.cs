﻿using System;
using System.Collections.Generic;

namespace BattleServer
{
    /// <summary>
    /// 战斗网络数据处理中心
    /// </summary>
    public static class BattleProtocolEvent
    {
        private static BytesWriter writer = new BytesWriter();

        public static void ReceiveLogin(BytesReader reader, int uuid)
        {
            C2SLoginMessage message = new C2SLoginMessage();
            message.Message(reader);

            Console.WriteLine(message.userName + "   " + message.pwassword);

            AccountData data = AccountDataManager.GetAccount(message.userName);

            S2CLoginMessage s2cLoginMessage = new S2CLoginMessage();
            if (data == null)
            {
                s2cLoginMessage.isCreate = 1;
                s2cLoginMessage.job = 1;
                s2cLoginMessage.level = 1;
                s2cLoginMessage.name = "创建角色";
            }
            else
            {
                s2cLoginMessage.isCreate = 0;
                s2cLoginMessage.job = data.job;
                s2cLoginMessage.level = data.level;
                s2cLoginMessage.name = data.name;
            }

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_Login);

            BattleProtocol.SendBytes(uuid, s2cLoginMessage.Message(writer));
        }

        public static void ReceiveCreatePlayer(BytesReader reader, int uuid)
        {
            C2SCreatePlayerMessage c2SCreatePlayer = new C2SCreatePlayerMessage();
            c2SCreatePlayer.Message(reader);

            NetPlayer netPlayer = new NetPlayer();

            netPlayer.uuid = uuid;                                          //唯一标识ID;
            netPlayer.basicsData.name = c2SCreatePlayer.name; ;    //名字;
            netPlayer.basicsData.sex = c2SCreatePlayer.sex; ;      //性别;
            netPlayer.basicsData.level = 1;                        //角色等级;

            NetPlayerManager.AddNetPlayer(netPlayer);

            S2CCreatePlayerMessage s2CCreatePlayer = new S2CCreatePlayerMessage();
            s2CCreatePlayer.netPlayer = netPlayer;
            s2CCreatePlayer.isCreate = 1;

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_CreatePlayer);

            BattleProtocol.SendBytes(uuid, s2CCreatePlayer.Message(writer));
        }

        public static void ReceiveGetRoom(BytesReader reader, int uuid)
        {
            C2SGetRoomMessage c2SGetRoom = new C2SGetRoomMessage();
            c2SGetRoom.Message(reader);

            S2CGetRoomMessage s2CGetRoom = new S2CGetRoomMessage();
            s2CGetRoom.roomCount = RoomManager.GetAllRoomList().Count;

            s2CGetRoom.roomParamList = new List<RoomParam>();
            foreach (RoomBase val in RoomManager.GetAllRoomList().Values)
            {
                RoomParam roomParam = new RoomParam();
                roomParam.roomIndex = val.roomIndex;
                roomParam.roomName = val.roomName;
                roomParam.roomType = (byte)val.roomType;

                s2CGetRoom.roomParamList.Add(roomParam);
            }

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_GetRoom);

            BattleProtocol.SendBytes(uuid, s2CGetRoom.Message(writer));

        }

        public static void ReceiveCreateRoom(BytesReader reader, int uuid)
        {
            C2SCreateRoomMessage c2SCreateRoom = new C2SCreateRoomMessage();
            c2SCreateRoom.Message(reader);

            S2CCreateRoomMessage s2CCreateRoom = new S2CCreateRoomMessage();

            NetPlayer netPlayer = NetPlayerManager.GetNetPlayer(uuid);

            BattleRoom battleRoom = new BattleRoom();
            RoomManager.AddRoom(battleRoom);

            battleRoom.ownerPlayer = netPlayer;

            RoomParam roomParam = new RoomParam();
            roomParam.roomIndex = battleRoom.roomIndex;
            roomParam.roomName = battleRoom.roomName;
            roomParam.roomType = (byte)battleRoom.roomType;

            s2CCreateRoom.roomParam = roomParam;

            PlayerParam playerParam = new PlayerParam();
            playerParam.camp = netPlayer.camp;
            playerParam.isOwner = 1;
            playerParam.level = netPlayer.basicsData.level; ;
            playerParam.playerName = netPlayer.basicsData.name;
            playerParam.sex = netPlayer.basicsData.sex;
            playerParam.uuid = netPlayer.uuid;

            s2CCreateRoom.playerList = new List<PlayerParam>();
            s2CCreateRoom.playerList.Add(playerParam);

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_CreateRoom);

            BattleProtocol.SendBytes(uuid, s2CCreateRoom.Message(writer));
        }

        public static void ReceiveAddRoom(BytesReader reader, int uuid)
        {
            C2SAddRoomMessage c2SAddRoom = new C2SAddRoomMessage();
            c2SAddRoom.Message(reader);

            NetPlayer netPlayer = NetPlayerManager.GetNetPlayer(uuid);

            RoomBase battleRoom = RoomManager.GetRoomBase(c2SAddRoom.roomIndex);

            bool isInRoom = battleRoom.PlayerInRoom(netPlayer);

            S2CAddRoomMessage s2CAddRoom = new S2CAddRoomMessage();
            s2CAddRoom.isInRoom = (byte)(isInRoom ? 1 : 0);

            s2CAddRoom.playerCount = battleRoom.netPlayerList.Count;

            s2CAddRoom.playerList = new List<PlayerParam>();

            foreach (NetPlayer nPlayer in battleRoom.netPlayerList)
            {
                PlayerParam playerParam = new PlayerParam();
                playerParam.camp = nPlayer.camp;
                playerParam.isOwner = (byte)(battleRoom.ownerPlayer == nPlayer ? 1 : 0);
                playerParam.level = nPlayer.basicsData.level; ;
                playerParam.playerName = nPlayer.basicsData.name;
                playerParam.sex = nPlayer.basicsData.sex;
                playerParam.uuid = nPlayer.uuid;

                s2CAddRoom.playerList.Add(playerParam);
            }

            RoomParam roomParam = new RoomParam();
            roomParam.roomIndex = battleRoom.roomIndex;
            roomParam.roomName = battleRoom.roomName;
            roomParam.roomType = (byte)battleRoom.roomType;

            s2CAddRoom.roomParam = roomParam;


            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_AddRoom);

            BattleProtocol.SendBytes(uuid, s2CAddRoom.Message(writer));
        }

        public static void ReceiveStartGame(BytesReader reader, int uuid)
        {
            C2SStartGameMessage c2SStartGame = new C2SStartGameMessage();
            c2SStartGame.Message(reader);

            NetPlayer netPlayer = NetPlayerManager.GetNetPlayer(uuid);
            RoomBase battleRoom = RoomManager.GetRoomBase(c2SStartGame.roomIndex);

            S2CStartGameMessage s2CStartGame = new S2CStartGameMessage();
            s2CStartGame.isStartGame = (byte)(netPlayer == battleRoom.ownerPlayer ? 1 : 0);
            s2CStartGame.playerCount = (byte)battleRoom.netPlayerList.Count;

            s2CStartGame.birthParamList = new List<PlayerBirthParam>();
            foreach(NetPlayer nPlayer in battleRoom.netPlayerList)
            {
                PlayerBirthParam birthParam = new PlayerBirthParam();
                birthParam.camp = nPlayer.camp;
                birthParam.isLoadFinish = 0;
                birthParam.name = nPlayer.basicsData.name;
                birthParam.px = 0;
                birthParam.pz = 0;
                s2CStartGame.birthParamList.Add(birthParam);
            }

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_StartGame);

            BattleProtocol.SendBytes(uuid, s2CStartGame.Message(writer));
        }

        public static void ReceiveStartBattle(BytesReader reader, int uuid)
        {

        }

        public static void ReceivePlayerMove(BytesReader reader, int uuid)
        {

        }

        public static void ReceivePlayerSkill(BytesReader reader, int uuid)
        {

        }

        public static void ReceivePlayerHit(BytesReader reader, int uuid)
        {

        }

        public static void ReceivePlayerAddBuff(BytesReader reader, int uuid)
        {

        }

        public static void ReceivePlayerRemoveBuff(BytesReader reader, int uuid)
        {

        }

        public static void ReceivePlayerDie(BytesReader reader, int uuid)
        {

        }
    }
}