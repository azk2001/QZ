using System;
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

            Console.WriteLine("ReceiveCreatePlayer");

            NetPlayer netPlayer = new NetPlayer();
            netPlayer.InitDefault();

            netPlayer.uuid = uuid;                                  //唯一标识ID;
            netPlayer.basicsData.name = c2SCreatePlayer.name;       //名字;
            netPlayer.basicsData.sex = c2SCreatePlayer.sex;         //性别;
            netPlayer.basicsData.level = 1;                         //角色等级;

            NetPlayerManager.AddNetPlayer(netPlayer);

            List<NetPlayer> netPlayers = NetPlayerManager.GetNetPlayers();

            S2CCreatePlayerMessage s2CCreatePlayer = new S2CCreatePlayerMessage();
            s2CCreatePlayer.playerCount = netPlayers.Count;
            s2CCreatePlayer.netPlayer = new List<NetPlayer>();
            s2CCreatePlayer.netPlayer.AddRange(netPlayers);
            s2CCreatePlayer.isCreate = 1;

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_CreatePlayer);

            BattleProtocol.SendBytes(uuid, s2CCreatePlayer.Message(writer));

            //通知玩家进入主城
            ReceivePlayerInScene(netPlayer, uuid);
        }

        //通知玩家进入房间;
        public static void ReceivePlayerInScene(NetPlayer netPlayer, int uuid)
        {
            S2CPlayerInSceneMessage s2cPlayerInScene = new S2CPlayerInSceneMessage();
            s2cPlayerInScene.netPlayer = netPlayer;

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_PlayerInScene);

            BattleProtocol.SendBytes(uuid, s2cPlayerInScene.Message(writer), true, false);
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
            RoomManager.NetPlayerInRoom(battleRoom.roomIndex, netPlayer);

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

            s2CStartGame.netPlayerList = new List<NetPlayer>();
            s2CStartGame.netPlayerList.AddRange(battleRoom.netPlayerList);

            foreach (NetPlayer sPlayer in battleRoom.netPlayerList)
            {
                sPlayer.gameUnit = GameUnitManager.AddGameUnit(sPlayer.uuid,sPlayer.roomIndex);
                sPlayer.gameUnit.Init(sPlayer.battleUnitData, sPlayer.basicsData);
            }

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_StartGame);

            BattleProtocol.SendBytes(uuid, s2CStartGame.Message(writer));
        }

        public static void ReceiveStartBattle(BytesReader reader, int uuid)
        {
            C2SStartBattleMessage c2SStartBattle = new C2SStartBattleMessage();
            c2SStartBattle.Message(reader);

            NetPlayer netPlayer = NetPlayerManager.GetNetPlayer(c2SStartBattle.uuid);
            netPlayer.isStartBattle = 1;

            RoomBase roomBase = RoomManager.GetRoomBase(netPlayer.roomIndex);

            S2CStartBattleMessage s2CStartBattle = new S2CStartBattleMessage();
            s2CStartBattle.isStartBattle = (byte)(roomBase.allPlayerReady() == true ? 1 : 0);

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_StartBattle);

            BattleProtocol.SendBytes(uuid, s2CStartBattle.Message(writer), true, true);
        }

        public static void ReceivePlayerMove(BytesReader reader, int uuid)
        {
            C2SPlayerMoveMessage c2SPlayerMove = new C2SPlayerMoveMessage();
            c2SPlayerMove.Message(reader);

            S2CPlayerMoveMessage s2CPlayerMove = new S2CPlayerMoveMessage();
            s2CPlayerMove.uuid = c2SPlayerMove.uuid;
            s2CPlayerMove.fx = c2SPlayerMove.fx;
            s2CPlayerMove.fy = c2SPlayerMove.fy;
            s2CPlayerMove.fz = c2SPlayerMove.fz;
            s2CPlayerMove.mx = c2SPlayerMove.mx;
            s2CPlayerMove.my = c2SPlayerMove.my;
            s2CPlayerMove.mz = c2SPlayerMove.mz;
            s2CPlayerMove.px = c2SPlayerMove.px;
            s2CPlayerMove.py = c2SPlayerMove.py;
            s2CPlayerMove.pz = c2SPlayerMove.pz;

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_PlayerMove);

            BattleProtocol.SendBytes(uuid, s2CPlayerMove.Message(writer), true, false);

        }

        public static void ReceivePlayerRoll(BytesReader reader, int uuid)
        {
            C2SPlayerRollMessage c2SPlayerRoll = new C2SPlayerRollMessage();
            c2SPlayerRoll.Message(reader);

            S2CPlayerRollMessage s2CPlayerRoll = new S2CPlayerRollMessage();
            s2CPlayerRoll.uuid = c2SPlayerRoll.uuid;
            s2CPlayerRoll.sx = c2SPlayerRoll.sx;
            s2CPlayerRoll.sy = c2SPlayerRoll.sy;
            s2CPlayerRoll.sz = c2SPlayerRoll.sz;
            s2CPlayerRoll.ex = c2SPlayerRoll.ex;
            s2CPlayerRoll.ey = c2SPlayerRoll.ey;
            s2CPlayerRoll.ez = c2SPlayerRoll.ez;

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_PlayerRoll);

            BattleProtocol.SendBytes(uuid, s2CPlayerRoll.Message(writer), true, false);

        }

        public static void ReceivePlayerSkill(BytesReader reader, int uuid)
        {
            C2SPlayerSkillMessage c2SPlayerSkill = new C2SPlayerSkillMessage();
            c2SPlayerSkill.Message(reader);

            S2CPlayerSkillMessage s2CPlayerSkill = new S2CPlayerSkillMessage();
            s2CPlayerSkill.uuid = c2SPlayerSkill.uuid;
            s2CPlayerSkill.skillIndex = c2SPlayerSkill.skillIndex;

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_PlayerSkill);

            BattleProtocol.SendBytes(uuid, s2CPlayerSkill.Message(writer), true, false);
        }

        public static void ReceivePlayerHit(BytesReader reader, int uuid)
        {
            C2SPlayerHitMessage c2SPlayerHit = new C2SPlayerHitMessage();
            c2SPlayerHit.Message(reader);

            S2CPlayerHitMessage s2CPlayerHit = new S2CPlayerHitMessage();
            s2CPlayerHit.hitUUID = c2SPlayerHit.hitUUID;
            s2CPlayerHit.killUUID = c2SPlayerHit.killUUID;

            NetPlayer killNetPlayer = NetPlayerManager.GetNetPlayer(s2CPlayerHit.killUUID);
            NetPlayer hitNetPlayer = NetPlayerManager.GetNetPlayer(s2CPlayerHit.hitUUID);

            hitNetPlayer.gameUnit.OnHit(killNetPlayer.gameUnit);

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_PlayerHit);

            BattleProtocol.SendBytes(uuid, s2CPlayerHit.Message(writer), true, true);

            if (hitNetPlayer.gameUnit.runUnitData.life <0)
            {
                SendPlayerDie(killNetPlayer, hitNetPlayer, hitNetPlayer.uuid);
            }
        }

        //服务器主动发送角色死亡信息;
        private static void SendPlayerDie(NetPlayer killNetPlayer, NetPlayer hitNetPlayer,int uuid)
        {
            S2CPlayerDieMessage s2CPlayerDie = new S2CPlayerDieMessage();
            s2CPlayerDie.hitUUID = hitNetPlayer.uuid;
            s2CPlayerDie.killUUID = killNetPlayer.uuid;

            writer.Clear();
            writer.WriteByte((byte)S2CBattleProtocol.S2C_PlayerDie);

            BattleProtocol.SendBytes(uuid, s2CPlayerDie.Message(writer), true, true);
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