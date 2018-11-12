using System;
using System.Collections.Generic;

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

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteString(name, 64);
        writer.WriteInt(level);
        writer.WriteInt(job);
        writer.WriteInt(isCreate);

        return writer;
    }
}

public struct C2SCreatePlayerMessage
{
    public string name;         //角色名字
    public int level;           //角色等级
    public byte sex;            //性别;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteString(name, 64);
        writer.WriteInt(level);
        writer.WriteByte(sex);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        name = reader.ReadString(64);
        name = name.Replace("\0", "");
        level = reader.ReadInt();
        sex = reader.ReadByte();
    }
}

public struct S2CCreatePlayerMessage
{
    public int isCreate;        //是否新号
    public int playerCount;     //网络玩家个数;
    public List<NetPlayer> netPlayer; //角色信息

    public void Message(BytesReader reader)
    {
        isCreate = reader.ReadInt();
        playerCount = reader.ReadInt();
        netPlayer = new List<NetPlayer>();
        for (int i = 0; i < playerCount; i++)
        {
            NetPlayer np = new NetPlayer();
            np.SetBytes(reader);
            netPlayer.Add(np);

        }
    }

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(isCreate);
        writer.WriteInt(netPlayer.Count);

        for (int i = 0; i < netPlayer.Count; i++)
        {
            netPlayer[i].GetBytes(writer);
        }

        return writer;
    }
}

public struct S2CPlayerInSceneMessage
{
    public NetPlayer netPlayer; //角色信息

    public void Message(BytesReader reader)
    {
        netPlayer = new NetPlayer();
        netPlayer.SetBytes(reader);
    }

    public BytesWriter Message(BytesWriter writer)
    {
        netPlayer.GetBytes(writer);

        return writer;
    }
}

public struct C2SGetRoomMessage
{
    public BytesWriter Message(BytesWriter writer)
    {
        return writer;
    }

    public void Message(BytesReader reader)
    {

    }
}

public struct S2CGetRoomMessage
{

    public int roomCount;                   //房间个数;
    public List<RoomParam> roomParamList;   //房间参数;


    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(roomCount);

        for (int i = 0, max = roomCount; i < max; i++)
        {
            RoomParam roomParam = roomParamList[i];
            writer.WriteString(roomParam.roomName, 64);
            writer.WriteInt(roomParam.roomIndex);
            writer.WriteByte(roomParam.roomType);
        }

        return writer;
    }

    public void Message(BytesReader reader)
    {
        roomCount = reader.ReadInt();

        if (roomCount > 0)
        {
            roomParamList = new List<RoomParam>();
            for (int i = 0, max = roomCount; i < max; i++)
            {
                RoomParam roomParam = new RoomParam();
                roomParam.roomName = reader.ReadString(64);
                roomParam.roomName = roomParam.roomName.Replace("\0", "");
                roomParam.roomIndex = reader.ReadInt();
                roomParam.roomType = reader.ReadByte();

                roomParamList.Add(roomParam);
            }
        }
    }
}

public struct C2SCreateRoomMessage
{
    public BytesWriter Message(BytesWriter writer)
    {
        return writer;
    }

    public void Message(BytesReader reader)
    {

    }
}

public struct S2CCreateRoomMessage
{
    public RoomParam roomParam;             //房间参数;
    public List<PlayerParam> playerList;    //房间里面的角色信息;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteString(roomParam.roomName, 64);
        writer.WriteInt(roomParam.roomIndex);
        writer.WriteByte(roomParam.roomType);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        roomParam.roomName = reader.ReadString(64);
        roomParam.roomName = roomParam.roomName.Replace("\0", "");
        roomParam.roomIndex = reader.ReadInt();
        roomParam.roomType = reader.ReadByte();
    }
}

public struct C2SAddRoomMessage
{
    public int roomIndex;
    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(roomIndex);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        roomIndex = reader.ReadInt();
    }
}

public struct S2CAddRoomMessage
{
    public byte isInRoom;                   //是否能进入;
    public RoomParam roomParam;             //房间参数;
    public int playerCount;                 //角色数量;
    public List<PlayerParam> playerList;    //房间里面的角色信息;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteByte(isInRoom);

        if (isInRoom == 1)
        {
            writer.WriteString(roomParam.roomName, 64);
            writer.WriteInt(roomParam.roomIndex);
            writer.WriteByte(roomParam.roomType);
            writer.WriteInt(playerCount);

            for (int i = 0, max = playerCount; i < max; i++)
            {
                PlayerParam playerParam = playerList[i];
                writer.WriteString(playerParam.playerName, 64);
                writer.WriteInt(playerParam.level);
                writer.WriteByte(playerParam.sex);
                writer.WriteInt(playerParam.camp);
                writer.WriteByte(playerParam.isOwner);
                writer.WriteInt(playerParam.uuid);
            }
        }

        return writer;
    }

    public void Message(BytesReader reader)
    {
        isInRoom = reader.ReadByte();
        if (isInRoom == 1)
        {
            roomParam.roomName = reader.ReadString(64);
            roomParam.roomName = roomParam.roomName.Replace("\0", "");
            roomParam.roomIndex = reader.ReadInt();
            roomParam.roomType = reader.ReadByte();

            playerCount = reader.ReadInt();

            playerList = new List<PlayerParam>();
            for (int i = 0, max = playerCount; i < max; i++)
            {
                PlayerParam playerParam = new PlayerParam();
                playerParam.playerName = reader.ReadString(64);
                playerParam.playerName = playerParam.playerName.Replace("\0", "");
                playerParam.level = reader.ReadInt();
                playerParam.sex = reader.ReadByte();
                playerParam.camp = reader.ReadInt();
                playerParam.isOwner = reader.ReadByte();
                playerParam.uuid = reader.ReadInt();

                playerList.Add(playerParam);
            }
        }
    }
}

public struct C2SStartGameMessage
{
    public int roomIndex;
    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(roomIndex);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        roomIndex = reader.ReadInt();
    }
}

public struct S2CStartGameMessage
{
    public byte isStartGame;
    public byte playerCount;
    public List<PlayerBirthParam> birthParamList;
    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteByte(isStartGame);

        writer.WriteByte(playerCount);

        for (int i = 0; i < playerCount; i++)
        {
            PlayerBirthParam birthParam = birthParamList[i];
            writer.WriteString(birthParam.name, 64);
            writer.WriteByte(birthParam.isLoadFinish);
            writer.WriteByte(birthParam.camp);
            writer.WriteInt(birthParam.px);
            writer.WriteInt(birthParam.pz);
        }


        return writer;
    }

    public void Message(BytesReader reader)
    {
        isStartGame = reader.ReadByte();
        playerCount = reader.ReadByte();

        for (int i = 0; i < playerCount; i++)
        {
            PlayerBirthParam birthParam = new PlayerBirthParam();
            birthParam.name = reader.ReadString(64);
            birthParam.name = birthParam.name.Replace("\0", "");
            birthParam.isLoadFinish = reader.ReadByte();
            birthParam.camp = reader.ReadByte();
            birthParam.px = reader.ReadInt();
            birthParam.pz = reader.ReadInt();

            birthParamList.Add(birthParam);
        }


    }
}

public struct C2SStartBattleMessage
{
    public int uuid;
    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
    }
}

public struct S2CStartBattleMessage
{
    public byte isStartBattle;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteByte(isStartBattle);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        isStartBattle = reader.ReadByte();
    }
}

public struct C2SPlayerMoveMessage
{
    public int uuid;
    public int fx;  //方向x;
    public int fy;  //方向y;
    public int fz;  //方向z;
    public int mx;  //移动x*100;
    public int my;  //移动y*100;
    public int mz;  //移动z*100;
    public int px;  //位置x*100;
    public int py;  //位置y*100;
    public int pz;  //位置z*100;


    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);
        writer.WriteInt(fx);
        writer.WriteInt(fy);
        writer.WriteInt(fz);
        writer.WriteInt(mx);
        writer.WriteInt(my);
        writer.WriteInt(mz);
        writer.WriteInt(px);
        writer.WriteInt(py);
        writer.WriteInt(pz);


        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
        fx = reader.ReadInt();
        fy = reader.ReadInt();
        fz = reader.ReadInt();
        mx = reader.ReadInt();
        my = reader.ReadInt();
        mz = reader.ReadInt();
        px = reader.ReadInt();
        py = reader.ReadInt();
        pz = reader.ReadInt();
    }
}

public struct S2CPlayerMoveMessage
{
    public int uuid;
    public int fx;  //方向x;
    public int fy;  //方向y;
    public int fz;  //方向z;
    public int mx;  //移动x*100;
    public int my;  //移动y*100;
    public int mz;  //移动z*100;
    public int px;  //位置x*100;
    public int py;  //位置y*100;
    public int pz;  //位置z*100;


    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);
        writer.WriteInt(fx);
        writer.WriteInt(fy);
        writer.WriteInt(fz);
        writer.WriteInt(mx);
        writer.WriteInt(my);
        writer.WriteInt(mz);
        writer.WriteInt(px);
        writer.WriteInt(py);
        writer.WriteInt(pz);


        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
        fx = reader.ReadInt();
        fy = reader.ReadInt();
        fz = reader.ReadInt();
        mx = reader.ReadInt();
        my = reader.ReadInt();
        mz = reader.ReadInt();
        px = reader.ReadInt();
        py = reader.ReadInt();
        pz = reader.ReadInt();
    }
}

public struct C2SPlayerSkillMessage
{
    public int uuid;
    public int skillId;     //释放技能id;
    public int ax;          //技能朝向x*100;
    public int ay;          //技能朝向y*100;
    public int az;          //技能朝向z*100;
    public int px;          //位置x*100;
    public int py;          //位置y*100;
    public int pz;          //位置z*100;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);
        writer.WriteInt(skillId);
        writer.WriteInt(ax);
        writer.WriteInt(ay);
        writer.WriteInt(az);
        writer.WriteInt(px);
        writer.WriteInt(py);
        writer.WriteInt(pz);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
        skillId = reader.ReadInt();
        ax = reader.ReadInt();
        ay = reader.ReadInt();
        az = reader.ReadInt();
        px = reader.ReadInt();
        px = reader.ReadInt();
        px = reader.ReadInt();
    }
}

public struct S2CPlayerSkillMessage
{
    public int uuid;
    public int skillId;     //释放技能id;
    public int ax;          //技能朝向x*100;
    public int ay;          //技能朝向y*100;
    public int az;          //技能朝向z*100;
    public int px;          //位置x*100;
    public int py;          //位置y*100;
    public int pz;          //位置z*100;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);
        writer.WriteInt(skillId);
        writer.WriteInt(ax);
        writer.WriteInt(ay);
        writer.WriteInt(az);
        writer.WriteInt(px);
        writer.WriteInt(py);
        writer.WriteInt(pz);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
        skillId = reader.ReadInt();
        ax = reader.ReadInt();
        ay = reader.ReadInt();
        az = reader.ReadInt();
        px = reader.ReadInt();
        px = reader.ReadInt();
        px = reader.ReadInt();
    }
}

public struct C2SPlayerHitMessage
{
    public int uuid;
    public int hit;         //受伤;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);
        writer.WriteInt(hit);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
        hit = reader.ReadInt();
    }
}

public struct S2CPlayerHitMessage
{
    public int uuid;
    public int hit;         //受伤;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);
        writer.WriteInt(hit);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
        hit = reader.ReadInt();
    }
}

public struct C2SPlayerAddBuffMessage
{
    public int uuid;
    public int buffId;         //buffId;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);
        writer.WriteInt(buffId);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
        buffId = reader.ReadInt();
    }
}

public struct S2CPlayerAddBuffMessage
{
    public int uuid;
    public int buffId;         //buffId;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);
        writer.WriteInt(buffId);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
        buffId = reader.ReadInt();
    }
}

public struct C2SPlayerRemoveBuffMessage
{
    public int uuid;
    public int buffId;         //buffId;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);
        writer.WriteInt(buffId);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
        buffId = reader.ReadInt();
    }
}

public struct S2CPlayerRemoveBuffMessage
{
    public int uuid;
    public int buffId;         //buffId;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);
        writer.WriteInt(buffId);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
        buffId = reader.ReadInt();
    }
}

public struct S2CPlayerRefreshBuffMessage
{
    public int buffCount;
    public List<RefreshBuffParam> buffIdList;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(buffCount);

        for (int i = 0; i < buffCount; i++)
        {
            RefreshBuffParam buffParam = buffIdList[i];
            writer.WriteInt(buffParam.buffId);
            writer.WriteInt(buffParam.px);
            writer.WriteInt(buffParam.pz);
        }

        return writer;
    }

    public void Message(BytesReader reader)
    {
        buffCount = reader.ReadInt();
        buffIdList = new List<RefreshBuffParam>();
        for (int i = 0; i < buffCount; i++)
        {
            RefreshBuffParam buffParam = new RefreshBuffParam();
            buffParam.buffId = reader.ReadInt();
            buffParam.px = reader.ReadInt();
            buffParam.pz = reader.ReadInt();
            buffIdList.Add(buffParam);

        }
    }
}

public struct C2SPlayerDieMessage
{
    public int uuid;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
    }
}

public struct S2CPlayerDieMessage
{
    public int uuid;

    public BytesWriter Message(BytesWriter writer)
    {
        writer.WriteInt(uuid);

        return writer;
    }

    public void Message(BytesReader reader)
    {
        uuid = reader.ReadInt();
    }
}


//---------------------------------下面是数据结构;

public class RoomParam
{
    public string roomName;         //房间名字;
    public int roomIndex;           //房间ID;
    public byte roomType;           //房间类型; 1单人赛 2组队赛
}

public class PlayerParam
{
    public string playerName;   //名字;
    public int level;           //等级;
    public byte sex;            //性别;
    public int camp;            //正营;
    public byte isOwner;        //是否是房主;
    public int uuid;            //玩家唯一标识ID;
}

public class RefreshBuffParam
{
    public int buffId;          //buffid;
    public int px;              //位置x*100;
    public int pz;              //位置z*100;
}

public class PlayerBirthParam
{
    public string name;
    public byte isLoadFinish;
    public byte camp;
    public int px;
    public int pz;
}