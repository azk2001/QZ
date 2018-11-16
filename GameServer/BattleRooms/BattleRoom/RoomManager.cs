using System;
using System.Collections.Generic;

namespace GameServer
{
    /// <summary>
    /// 战斗服房间管理,一个游戏服务器对应一个战斗服务器
    /// </summary>
    public class RoomManager
    {
        private static Dictionary<int, RoomBase> mRoomList = new Dictionary<int, RoomBase>();       //房间列表

        private static int _roomIndex = 1;
        public static int ROOMINDEX
        {
            get
            {
                return _roomIndex++;
            }
        }

        public static void Init()
        {
            List<battleroominit_b> roomInitList = battleroominit_b.GetList();

            for (int i = 0, max = roomInitList.Count; i < max; i++)
            {
                battleroominit_b battleroominit = roomInitList[i];
            }
        }



        /// <summary>
        /// 网络玩家离开房间;
        /// </summary>
        public static void NetPlayerOutRoom()
        {

        }

        public static RoomBase AddRoom(RoomBase roomBase) 
        {
            int roomIndex = ROOMINDEX;
            roomBase.Init(roomIndex);
            mRoomList[roomIndex] = roomBase;

            return roomBase;
        }

        public static RoomBase GetRoomBase(int roomIndex)
        {
            if (mRoomList.ContainsKey(roomIndex) == true)
            {
                return mRoomList[roomIndex];
            }

            return null;
        }

        // 网络玩家进入房间;
        public static bool NetPlayerInRoom(int roomIndex,NetPlayer netPlayer)
        {
            RoomBase roomBase= GetRoomBase(roomIndex);

            return roomBase.PlayerInRoom(netPlayer);

        }

        // 网络玩家离开房间;
        public static bool NetPlayerOutRoom(int roomIndex, NetPlayer netPlayer)
        {
            RoomBase roomBase = GetRoomBase(roomIndex);

            return roomBase.PlayerOutRoom(netPlayer);
        }

        public static List<NetPlayer> GetBattleRoomPlayerList(int roomIndex)
        {
            RoomBase roomBase = GetRoomBase(roomIndex);
            if (roomBase != null)
            {
                return roomBase.netPlayerList;
            }
            return null;
        }
        
        public static void RemoveRoomBase(int roomIndex)
        {
            if (mRoomList.ContainsKey(roomIndex) == true)
            {
                RoomBase roomBase = mRoomList[roomIndex];
                roomBase.isRun = false;
                mRoomList.Remove(roomIndex);
            }
        }

        public static Dictionary<int, RoomBase> GetAllRoomList()
        {
            return mRoomList;
        }

    }
}
