using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer
{
    public enum ePlayerRoomState
    {
        inLobby = 0,          //玩家在大厅;
        inRoom,             //在房间里面;（玩家进入房间）
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
        private List<NetPlayer> _netPlayerList = new List<NetPlayer>();           //房间里面的玩家信息;
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
        public BattleCore battleCore = null;                        //当前副本战役流程;

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

       

        //是否所有角色已经加载完成，可以战斗
        public virtual bool allPlayerReady()
        {
            ePlayerRoomState roomState = ePlayerRoomState.readyed;

            foreach (NetPlayer item in netPlayerList)
            {
                if (item.roomState == ePlayerRoomState.outGame || item.roomState == ePlayerRoomState.offLine)
                    continue;

                roomState = item.roomState;

                if (roomState != ePlayerRoomState.readyed)
                    break;
            }

            bool isSceneLoaded = (roomState == ePlayerRoomState.readyed);

            return isSceneLoaded;
        }

        //房间战斗初始化;
        public virtual void StartScene()
        {
            isRun = true;

            roomState = RoomState.fighting;
        }

        /// <summary>
        /// 角色从服务器上加入战斗,这个时候客服端还没有吧资源加载完成;
        /// </summary>
        public virtual void PlayerInScene(NetPlayer netPlayer)
        {

        }

        /// <summary>
        /// 角色主动离开战斗场景（掉线，主动离开）;
        /// </summary>
        public virtual void PlayerLeaveScene(NetPlayer netPlayer)
        {

        }

        /// <summary>
        /// 设置玩家准备就绪（客服端资源加载完成），可以开始战斗;
        /// </summary>
        /// <param name="netPlayer"></param>
        /// <param name="isReady"></param>
        public virtual void SetPlayerReady(NetPlayer netPlayer, bool isReady)
        {
            if (netPlayer != null)
            {
                netPlayer.SetRoomState(ePlayerRoomState.readyed);
            }
        }

        /// <summary>
        /// 场景加载完成后调用,开启战斗
        /// </summary>
        public virtual void StartBattle()
        {

        }

        //房间结束
        public virtual void EndScene()
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
