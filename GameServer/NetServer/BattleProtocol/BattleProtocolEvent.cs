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

            NetPlayer netPlayer = new NetPlayer();
            
            netPlayer.uuid = uuid;                                          //唯一标识ID;
            netPlayer.basicsData.roleData.name = c2SCreatePlayer.name; ;    //名字;
            netPlayer.basicsData.roleData.sex = c2SCreatePlayer.sex; ;      //性别;
            netPlayer.basicsData.roleData.level = 1;                        //角色等级;

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

        }

        public static void ReceiveCreateRoom(BytesReader reader, int uuid)
        {

        }

        public static void ReceiveAddRoom(BytesReader reader, int uuid)
        {

        }

        public static void ReceiveStartGame(BytesReader reader, int uuid)
        {

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