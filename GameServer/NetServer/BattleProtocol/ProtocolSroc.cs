﻿using System;
using System.Collections.Generic;

namespace GameServer
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
        public NetPlayer netPlayer; //角色信息

        public void Message(BytesReader reader)
        {
            isCreate = reader.ReadInt();
            NetPlayer np = new NetPlayer();
            np.SetBytes(reader);
            netPlayer = np;

        }

        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteInt(isCreate);
            netPlayer.GetBytes(writer);

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
        public int playerCount;                 //玩家个数;
        public List<NetPlayer> playerList;    //房间里面的角色信息;

        public BytesWriter Message(BytesWriter writer)
        {
            roomParam.Message(writer);

            writer.WriteInt(playerList.Count);
           
            for (int i = 0,max = playerList.Count; i < max; i++)
            {
                playerList[i].GetBytes(writer);
            }
            return writer;
        }

        public void Message(BytesReader reader)
        {
            roomParam.Message(reader);
            playerCount = reader.ReadInt();

            playerList = new List<NetPlayer>();
            for(int i=0;i<playerCount;i++)
            {
                NetPlayer netPlayer = new NetPlayer();
                netPlayer.SetBytes(reader);

                playerList.Add(netPlayer);
            }

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
        public List<NetPlayer> playerList;      //房间里面的角色信息;

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
                    NetPlayer nPlayer = new NetPlayer();
                    nPlayer.GetBytes(writer);
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

                playerList = new List<NetPlayer>();
                for (int i = 0, max = playerCount; i < max; i++)
                {
                    NetPlayer netPlayer = new NetPlayer();
                    netPlayer.SetBytes(reader);

                    playerList.Add(netPlayer);
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
        public List<NetPlayer> netPlayerList;
        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteByte(isStartGame);

            writer.WriteByte(playerCount);

            for (int i = 0; i < playerCount; i++)
            {
                NetPlayer birthParam = netPlayerList[i];
                writer = birthParam.GetBytes(writer);
            }

            return writer;
        }

        public void Message(BytesReader reader)
        {
            isStartGame = reader.ReadByte();
            playerCount = reader.ReadByte();

            netPlayerList = new List<NetPlayer>();
            for (int i = 0; i < playerCount; i++)
            {
                NetPlayer birthParam = new NetPlayer();
                birthParam.SetBytes(reader);
                netPlayerList.Add(birthParam);
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

    public struct C2SPlayerRollMessage
    {
        public int uuid;
        public int sx;  //开始点x*100;
        public int sy;  //开始点y*100;
        public int sz;  //开始点z*100;
        public int ex;  //结束点x*100;
        public int ey;  //结束点y*100;
        public int ez;  //结束点z*100;

        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteInt(uuid);
            writer.WriteInt(sx);
            writer.WriteInt(sy);
            writer.WriteInt(sz);
            writer.WriteInt(ex);
            writer.WriteInt(ey);
            writer.WriteInt(ez);

            return writer;
        }

        public void Message(BytesReader reader)
        {
            uuid = reader.ReadInt();
            sx = reader.ReadInt();
            sy = reader.ReadInt();
            sz = reader.ReadInt();
            ex = reader.ReadInt();
            ey = reader.ReadInt();
            ez = reader.ReadInt();
        }
    }

    public struct S2CPlayerRollMessage
    {
        public int uuid;
        public int sx;  //开始点x*100;
        public int sy;  //开始点y*100;
        public int sz;  //开始点z*100;
        public int ex;  //结束点x*100;
        public int ey;  //结束点y*100;
        public int ez;  //结束点z*100;

        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteInt(uuid);
            writer.WriteInt(sx);
            writer.WriteInt(sy);
            writer.WriteInt(sz);
            writer.WriteInt(ex);
            writer.WriteInt(ey);
            writer.WriteInt(ez);

            return writer;
        }

        public void Message(BytesReader reader)
        {
            uuid = reader.ReadInt();
            sx = reader.ReadInt();
            sy = reader.ReadInt();
            sz = reader.ReadInt();
            ex = reader.ReadInt();
            ey = reader.ReadInt();
            ez = reader.ReadInt();
        }
    }

    public struct C2SPlayerSkillMessage
    {
        //会先同步位置在同步技能;
        public int uuid;
        public int skillIndex;     //释放技能id;
        public int px;             //位置x*100;
        public int py;             //位置y*100;
        public int pz;             //位置z*100;
        public int fx;             //方向x*100;
        public int fy;             //方向y*100;
        public int fz;             //方向z*100;


        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteInt(uuid);
            writer.WriteInt(skillIndex);
            writer.WriteInt(px);
            writer.WriteInt(py);
            writer.WriteInt(pz);
            writer.WriteInt(fx);
            writer.WriteInt(fy);
            writer.WriteInt(fz);

            return writer;
        }

        public void Message(BytesReader reader)
        {
            uuid = reader.ReadInt();
            skillIndex = reader.ReadInt();
            px = reader.ReadInt();
            py = reader.ReadInt();
            pz = reader.ReadInt();
            fx = reader.ReadInt();
            fy = reader.ReadInt();
            fz = reader.ReadInt();
        }
    }

    public struct S2CPlayerSkillMessage
    {
        //会先同步位置在同步技能;
        public int uuid;
        public int skillIndex;     //释放技能id;
        public int px;             //位置x*100;
        public int py;             //位置y*100;
        public int pz;             //位置z*100;
        public int fx;             //方向x*100;
        public int fy;             //方向y*100;
        public int fz;             //方向z*100;


        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteInt(uuid);
            writer.WriteInt(skillIndex);
            writer.WriteInt(px);
            writer.WriteInt(py);
            writer.WriteInt(pz);
            writer.WriteInt(fx);
            writer.WriteInt(fy);
            writer.WriteInt(fz);

            return writer;
        }

        public void Message(BytesReader reader)
        {
            uuid = reader.ReadInt();
            skillIndex = reader.ReadInt();
            px = reader.ReadInt();
            py = reader.ReadInt();
            pz = reader.ReadInt();
            fx = reader.ReadInt();
            fy = reader.ReadInt();
            fz = reader.ReadInt();
        }
    }

    public struct C2SPlayerHitMessage
    {
        public int hitUUID;
        public int atkUUID;

        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteInt(hitUUID);
            writer.WriteInt(atkUUID);

            return writer;
        }

        public void Message(BytesReader reader)
        {
            hitUUID = reader.ReadInt();
            atkUUID = reader.ReadInt();
        }
    }

    public struct S2CPlayerHitMessage
    {
        public int hitUUID;
        public int atkUUID;

        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteInt(hitUUID);
            writer.WriteInt(atkUUID);

            return writer;
        }

        public void Message(BytesReader reader)
        {
            hitUUID = reader.ReadInt();
            atkUUID = reader.ReadInt();
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

    public struct S2CRefreshBuffMessage
    {
        public int buffCount;
        public List<RefreshBuffParam> buffList;

        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteInt(buffCount);

            for (int i = 0; i < buffCount; i++)
            {
                RefreshBuffParam buffParam = buffList[i];
                writer.WriteInt(buffParam.buffId);
                writer.WriteInt(buffParam.px);
                writer.WriteInt(buffParam.pz);
            }

            return writer;
        }

        public void Message(BytesReader reader)
        {
            buffCount = reader.ReadInt();
            buffList = new List<RefreshBuffParam>();
            for (int i = 0; i < buffCount; i++)
            {
                RefreshBuffParam buffParam = new RefreshBuffParam();
                buffParam.buffId = reader.ReadInt();
                buffParam.px = reader.ReadInt();
                buffParam.pz = reader.ReadInt();
                buffList.Add(buffParam);

            }
        }
    }

    public struct C2SPlayerDieMessage
    {
        public int hitUUID;
        public int killUUID;

        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteInt(hitUUID);
            writer.WriteInt(killUUID);

            return writer;
        }

        public void Message(BytesReader reader)
        {
            hitUUID = reader.ReadInt();
            killUUID = reader.ReadInt();
        }
    }

    public struct S2CPlayerDieMessage
    {
        public int hitUUID;
        public int atkUUID;

        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteInt(hitUUID);
            writer.WriteInt(atkUUID);

            return writer;
        }

        public void Message(BytesReader reader)
        {
            hitUUID = reader.ReadInt();
            atkUUID = reader.ReadInt();
        }
    }
    

    public class S2CGameFinishMessage
    {
        public class Finish1V1
        {
        }

        public int roomIndex;
        public int loseCamp;   //失败正营Id
        public int dungeonId;  //关卡id

        public Finish1V1 finish1v1 = new Finish1V1();  // 1v1参数

        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteInt(loseCamp);
            writer.WriteInt(dungeonId);

            dungeon_s dungeon = dungeon_s.Get(dungeonId);

            switch(dungeon.mapType)
            {
                case eMapType.pvpfight1V1:

                    break;
                case eMapType.pvpFightChaos:

                    break;
            }

            return writer;
        }

        public void Message(BytesReader reader)
        {
            loseCamp = reader.ReadInt();
            dungeonId = reader.ReadInt();

            dungeon_s dungeon = dungeon_s.Get(dungeonId);

            switch (dungeon.mapType)
            {
                case eMapType.pvpfight1V1:

                    break;
                case eMapType.pvpFightChaos:

                    break;
            }
        }
    }
    //---------------------------------下面是数据结构;

    public class RoomParam
    {
        public string roomName;         //房间名字;
        public int roomIndex;           //房间ID;
        public byte roomType;           //房间类型; 1单人赛 2组队赛

        public BytesWriter Message(BytesWriter writer)
        {
            writer.WriteString(roomName, 64);
            writer.WriteInt(roomIndex);
            writer.WriteByte(roomType);

            return writer;
        }

        public void Message(BytesReader reader)
        {
            roomName = reader.ReadString(64);
            roomName = roomName.Replace("\0", "");
            roomIndex = reader.ReadInt();
            roomType = reader.ReadByte();
        }

    }

    public class RefreshBuffParam
    {
        public int buffId;          //buffid;
        public int px;              //位置x*100;
        public int pz;              //位置z*100;
    }
}
