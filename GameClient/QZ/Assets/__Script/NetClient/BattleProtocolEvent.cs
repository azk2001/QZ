using System;
using System.Collections.Generic;

namespace BattleClient
{
    /// <summary>
    /// 战斗网络数据处理中心
    /// </summary>
    public static class BattleProtocolEvent
    {
        private static BytesWriter writer = new BytesWriter();
        public static BytesReader reader = new BytesReader();

        public static void SendLogin(C2SLoginMessage message)
        {
            writer.Clear();
            writer.WriteByte((int)C2SBattleProtocol.C2S_Login);

            writer = message.Message(writer);

            BattleProtocol.SendBytes(writer);
        }

        public static void ReceiveLogin(BytesReader reader)
        {
            S2CLoginMessage message = new S2CLoginMessage();
            message.Message(reader);

        }

    }
}