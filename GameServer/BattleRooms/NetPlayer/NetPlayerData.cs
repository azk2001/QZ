using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServer
{
    //角色战斗数据
    public class BattleUnitData : ICloneable
    {
        public int life = 0;        //血量
        public int speed = 0;       //移动数度
        public int shield = 0;      //护盾
        public int harm = 0;        //伤害

        public void SetBytes(BytesReader reader)
        {
            life = reader.ReadInt();
            speed = reader.ReadInt();
            shield = reader.ReadInt();
            harm = reader.ReadInt();
        }

        public BytesWriter GetBytes(BytesWriter writer)
        {
            writer.WriteInt(life);
            writer.WriteInt(speed);
            writer.WriteInt(shield);
            writer.WriteInt(harm);

            return writer;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    //角色基础数据
    public class PlayerBasicsData
    {
        public RoleData roleData = new RoleData();  //角色基础信息;

        public void SetBytes(BytesReader reader)
        {
            roleData.SetBytes(reader);
        }

        public BytesWriter GetBytes(BytesWriter writer)
        {
            writer = roleData.GetBytes(writer);

            return writer;
        }
    }

    //角色属性信息;
    public class RoleData
    {
        public int uuid;        //唯一标识ID;
        public string name;     //名字;
        public int sex;         //性别;
        public int level;       //角色等级;

        public void SetBytes(BytesReader reader)
        {
            uuid = reader.ReadInt();
            name = reader.ReadString(64);
            name = name.Replace("\0", string.Empty);
            
            sex = reader.ReadInt();
            job = reader.ReadInt();
        }

        public BytesWriter GetBytes(BytesWriter writer)
        {
            writer.WriteInt(uuid);
            writer.WriteString(name, 64);
            writer.WriteInt(sex);
            writer.WriteInt(job);

            return writer;
        }
    }
}
