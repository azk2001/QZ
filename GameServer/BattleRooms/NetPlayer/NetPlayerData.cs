using System;

namespace GameServer
{
    //角色战斗数据
    public class BattleUnitData : ICloneable
    {
        public int camp = 0;        //阵营
        public int life = 0;        //血量
        public int speed = 0;       //移动数度
        public int shield = 0;      //护盾
        public int harm = 0;        //伤害

        public void SetBytes(BytesReader reader)
        {
            camp = reader.ReadInt();
            life = reader.ReadInt();
            speed = reader.ReadInt();
            shield = reader.ReadInt();
            harm = reader.ReadInt();
        }

        public BytesWriter GetBytes(BytesWriter writer)
        {
            writer.WriteInt(camp);
            writer.WriteInt(life);
            writer.WriteInt(speed);
            writer.WriteInt(shield);
            writer.WriteInt(harm);

            return writer;
        }

        public void InitDefault()
        {
            camp = 0;
            life = 100;
            speed = 7000;
            shield = 10;
            harm = 20;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    //角色基础数据
    public class PlayerBasicsData
    {
        public int uuid;        //唯一标识ID;
        public string name;     //名字;
        public int sex;         //性别;
        public int level;       //角色等级;

        public int px;          //位置X*100;
        public int py;          //位置Y*100；

        public void SetBytes(BytesReader reader)
        {
            uuid = reader.ReadInt();
            name = reader.ReadString(64);
            name = name.Replace("\0", string.Empty);
            sex = reader.ReadInt();
            level = reader.ReadInt();
            px = reader.ReadInt();
            py = reader.ReadInt();
        }

        public BytesWriter GetBytes(BytesWriter writer)
        {
            writer.WriteInt(uuid);
            writer.WriteString(name, 64);
            writer.WriteInt(sex);
            writer.WriteInt(level);
            writer.WriteInt(px);
            writer.WriteInt(py);

            return writer;
        }
    }
    
}
