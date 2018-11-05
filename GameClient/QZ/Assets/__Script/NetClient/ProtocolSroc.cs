using System;
using System.Collections.Generic;

namespace BattleClient
{
    public struct C2SLoginMessage
    {
        public string userName;
        public string pwassword;

        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteString(userName, 64);
            writer.WriteString(pwassword, 64);

            return writer;
        }

        public void Message(BytesReader reader)
        {
            userName = reader.ReadString(64);
            userName = userName.Replace("\0", "");
            pwassword = reader.ReadString(64);
            pwassword = pwassword.Replace("\0", "");
        }
    }

    public struct S2CLoginMessage
    {
        public string name;         //角色名字
        public int level;           //角色等级
        public int job;             //角色职业
        public int isCreate;        //是否新号

        public void Message(BytesReader reader)
        {
            name = reader.ReadString(64);
            name = name.Replace("\0", "");
            level = reader.ReadInt();
            job = reader.ReadInt();
            isCreate = reader.ReadInt();
        }

        public void Message(BytesWriter writer)
        {
            writer.WriteString(name, 64);
            writer.WriteInt(level);
            writer.WriteInt(job);
            writer.WriteInt(isCreate);
        }
    }

}
