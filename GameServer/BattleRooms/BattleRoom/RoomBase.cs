using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer
{
    public enum ePlayerRoomState
    {
        inLobby = 0,          //玩家在大厅;
        inRoom,             //在房间里面;（玩家进入房间）
        sceneLoaded,        //场景加载完成;
        readyed,            //玩家准备完成，可以战斗了;
        fighting,           //战斗中;
        death,              //死亡;
        outGame,            //主动退出游戏;
        offLine,            //断线;
    }

    public enum RoomState
    {
        leisure,    //空闲
        ready,      //准备
        fighting,   //战斗中;
    }

    public enum RoomType
    {
        oneFighting,    //个人赛;
        teamFighting,   //组队赛;
    }

    /// <summary>
    /// 战斗房间管理
    /// </summary>
    public class RoomBase
    {
        private List<NetPlayer> _netPlayerList = new List<NetPlayer>();           //房间里面的玩家信息;   uuid
        public List<NetPlayer> netPlayerList
        {
            get
            {
                return _netPlayerList;
            }
        }

        public string roomName = "11.11天猫购物";                       //房间名字;
        public RoomState roomState = RoomState.leisure;             //当前房间状态;
        public RoomType roomType = RoomType.oneFighting;            //房间类型;

        public NetPlayer ownerPlayer;        //房主游戏对象;

        public int roomIndex = 0;           //房间索引;

        public int maxPlayerNum = 6;        //最大可承载玩家数

        public bool isRun = false;          //当前房间是否在运行;

        //房间信息初始化;
        public virtual void Init(int roomIndex)
        {
            this.roomIndex = roomIndex;
            isRun = false;

            netPlayerList.Clear();

            roomState = RoomState.leisure;

            MyDebug.WriteLine("CreateRoom:" + roomIndex);
        }

        /// <summary>
        /// 角色加入房间数据
        /// </summary>
        /// <param name="netPlayer"></param>
        public virtual bool PlayerInRoom(NetPlayer netPlayer)
        {
            if (netPlayerList.Count >= maxPlayerNum)
                return false;

            if (roomState == RoomState.fighting)
                return false;
            
            if (netPlayerList.Contains(netPlayer) == false)
            {
                netPlayerList.Add(netPlayer);
            }

            netPlayer.roomIndex = roomIndex;
            netPlayer.SetRoomState(ePlayerRoomState.inRoom);

            MyDebug.WriteLine("PlayerInRoom roomIndex:" + roomIndex + "  uid:" + netPlayer.uuid);

            roomState = RoomState.ready;

            return true;
        }

        /// <summary>
        /// 玩家退出房间
        /// </summary>
        /// <param name="netPlayer"></param>
        public virtual bool PlayerOutRoom(NetPlayer netPlayer)
        {

            if (roomState == RoomState.fighting)
                return false;
            
            if (netPlayerList.Contains(netPlayer) == true)
            {
                netPlayerList.Remove(netPlayer);
            }

            netPlayer.roomIndex = 0;
            netPlayer.SetRoomState(ePlayerRoomState.inLobby);

            return true;
        }

        //房间战斗初始化;
        public virtual void Start(int sceneId)
        {
            isRun = true;

            roomState = RoomState.fighting;
        }

        //房间结束
        public virtual void End()
        {
            roomState = RoomState.leisure;

            isRun = false;
        }

        //房间心跳函数
        public virtual void Update(float deltaTime)
        {
         
        }

    }
}
