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

            AccountData data = AccountDataManager.GetAccount(message.userName);

            S2CLoginMessage s2cLoginMessage = new S2CLoginMessage();
            if(data == null)
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
        
    }
}