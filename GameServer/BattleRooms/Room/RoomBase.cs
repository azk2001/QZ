using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer
{
    public enum ePlayerRoomState
    {
        inLobby=0,          //玩家在大厅;
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

    /// <summary>
    /// 战斗房间管理
    /// </summary>
    public class RoomBase
    {
        public List<int> netPlayerList = new List<int>();           //房间里面的玩家信息;   uuid
        public RoomState roomState = RoomState.leisure;             //当前房间状态

        public BattleCore battleCore = null;                        //当前副本战役流程;
        public int roomIndex = 0;       //房间索引;
        public int sceneId = 0;         //当前副本场景ID;

        public int maxPlayerNum = 0;        //最大可承载玩家数
        public int curPlayerNum = 0;        //当前玩家数量;

        public bool isRun = false;      //当前房间是否在运行;

        public DateTime roomStartTime = DateTime.MinValue;      //房间战斗开始时间;
        public DateTime roomEndTime = DateTime.MinValue;        //房间战斗结束时间;

        protected int mainPlayerUUID ;          //当前战斗中的主驱动AI玩家UUID;
        protected DateTime pingTime;
        protected float dataTime = 0;
        protected bool isPinging = false;       //是否在ping中;
        protected float pingOutTime = 0;
        protected bool isStartRunRoom = false; //是否是第一次运行这个房间;


        private int limitTime = 180;

        //房间信息初始化;
        public virtual void Init(int roomIndex)
        {
            this.roomIndex = roomIndex;
            isRun = false;

            netPlayerList.Clear();

            MyDebug.WriteLine("CreateRoom:" + roomIndex);
        }

        //房间战斗初始化;
        public virtual void Start(int sceneId)
        {
            this.sceneId = sceneId;

            isStartRunRoom = true;

            isRun = true;

            battleCore.Start(this, sceneId, roomIndex);

            roomStartTime = DateTime.Now;
            roomEndTime = DateTime.Now.AddSeconds(limitTime);
        }

        //房间结束
        public virtual void End()
        {
            isRun = false;

            GameUnitManager.RemoveRoomAllGameUnit(roomIndex);

            netPlayerList.Clear();

            battleCore.End();
        }

        //房间心跳函数
        public void Update(float deltaTime)
        {
            if (isRun == false)
                return;

            battleCore.Update(deltaTime);

            UpdateFPS(deltaTime);
        }

        /// <summary>
        /// 用继承方式来刷新是否检查AI的切换
        /// </summary>
        public virtual void UpdateFPS(float deltaTime)
        {
            dataTime += deltaTime;

        }

        //ping回调检查是否切换AI主机的驱动
        public void ReceiveCheckMainPlayer(int uuid)
        {
            isPinging = false;
            pingOutTime = 0;

            if (mainPlayerUUID == uuid)
            {
                //如果主机玩家的Ping大于两秒就切换ai;
                if ((DateTime.Now - pingTime).TotalSeconds > 1)
                {
                    ChangeMainPlayer(false);
                }
            }
        }

        /// <summary>
        /// 重新交换主机;
        /// </summary>
        /// <param name="isCheckMain">是否忽略上一个主机不检查</param>
        protected void ChangeMainPlayer(bool isCheckMain)
        {
           
        }

        /// <summary>
        /// 角色加入房间数据
        /// </summary>
        /// <param name="netPlayer"></param>
        public virtual void PlayerInRoom(NetPlayer netPlayer)
        {
            int uuid = netPlayer.uuid;

            if (netPlayerList.Contains(uuid) == false)
            {
                netPlayerList.Add(uuid);
            }

            netPlayer.roomIndex = roomIndex;
            netPlayer.SetRoomState(ePlayerRoomState.inRoom);

            MyDebug.WriteLine("PlayerInRoom roomIndex:" + roomIndex + "  uid:" + netPlayer.uuid);
        }

        /// <summary>
        /// 玩家退出房间
        /// </summary>
        /// <param name="netPlayer"></param>
        public virtual void PlayerOutRoom(NetPlayer netPlayer)
        {
            int uuid = netPlayer.uuid;

            if (netPlayerList.Contains(uuid) == true)
            {
                netPlayerList.Remove(uuid);
            }

            netPlayer.roomIndex = 0;
            netPlayer.SetRoomState(ePlayerRoomState.inLobby);



        }

        /// <summary>
        /// 移除玩家,玩家主动离线;
        /// </summary>
        /// <param name="outType">离开副本类型 1：主动离开场景  2:掉线离开</param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public virtual void PlayerQuitRoom(int outType, int uuid)
        {
            if (netPlayerList.Contains(uuid))
            {
                NetPlayer netPlayer = NetPlayerManager.GetNetPlayer(uuid);
                if (netPlayer != null)
                {
                    if (outType == 1)
                    {
                        netPlayer.SetRoomState(ePlayerRoomState.outGame);//玩家主动离线;
                    }
                    else if (outType == 2)
                    {
                        if(netPlayer.roomState != ePlayerRoomState.outGame)
                        {
                            netPlayer.SetRoomState(ePlayerRoomState.offLine);//玩家主动离线;
                        }
                    }

                    battleCore.LeaveScene(netPlayer.uuid);
                }
            }
        }

        //设置场景加载完成
        public virtual void SetSceneLoaded(int uuid, bool isLoaded)
        {
            if (netPlayerList.Contains(uuid))
            {
                if (isLoaded == true)
                {
                    NetPlayer netPlayer = NetPlayerManager.GetNetPlayer(uuid);
                    netPlayer.SetRoomState(ePlayerRoomState.sceneLoaded);
                }
            }
        }

        /// <summary>
        /// 检查所有场景是否加载完成
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="isLoaded"></param>
        /// <returns></returns>
        public bool CheckAllSceneLoaded()
        {
            ePlayerRoomState roomState = ePlayerRoomState.sceneLoaded;

            List<NetPlayer> netRoomPlayer = RoomManager.GetBattleRoomPlayerList(roomIndex);

            foreach (NetPlayer item in netRoomPlayer)
            {
                if (item.roomState == ePlayerRoomState.outGame || item.roomState == ePlayerRoomState.offLine)
                    continue;

                roomState = item.roomState;

                if (roomState != ePlayerRoomState.sceneLoaded)
                    break;
            }

            bool isSceneLoaded = (roomState == ePlayerRoomState.sceneLoaded);

            return isSceneLoaded;
        }

        //设置玩家已经准备好
        public virtual void SetPlayerReady(int uuid, bool isReady)
        {
            if (netPlayerList.Contains(uuid))
            {
                if (isReady == true)
                {
                    NetPlayer netPlayer = NetPlayerManager.GetNetPlayer(uuid);
                    netPlayer.SetRoomState(ePlayerRoomState.fighting);
                }
            }
        }

        /// <summary>
        /// 检查所有玩家是否已经准备好了，包括场景加载完成，角色初始化完成
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="isReady"></param>
        /// <returns></returns>
        public bool CheckAllPlayerReady()
        {
            ePlayerRoomState roomState = ePlayerRoomState.fighting;

            List<NetPlayer> netRoomPlayer = RoomManager.GetBattleRoomPlayerList(roomIndex);

            foreach (NetPlayer item in netRoomPlayer)
            {
                if (item.roomState == ePlayerRoomState.outGame || item.roomState == ePlayerRoomState.offLine)
                    continue;

                roomState = item.roomState;
                if (roomState != ePlayerRoomState.fighting)
                    break;
            }

            bool isPlayerLoaded = (roomState == ePlayerRoomState.fighting);

            if (isPlayerLoaded == true)
            {
                roomStartTime = DateTime.Now;
                roomEndTime = DateTime.Now.AddSeconds(limitTime);
            }

            return isPlayerLoaded;
        }

        /// <summary>
        /// 角色加入战斗;
        /// </summary>
        public virtual void PlayerInScene(NetPlayer netPlayer)
        {
            battleCore.InScene(netPlayer);
        }

        /// <summary>
        /// 角色离开战斗;
        /// </summary>
        public virtual void PlayerLeaveScene(NetPlayer netPlayer)
        {
            if (netPlayerList.Contains(netPlayer.uuid))
            {
                if (netPlayer != null)
                {
                    netPlayer.SetRoomState(ePlayerRoomState.outGame);//玩家主动离线;
                    battleCore.LeaveScene(netPlayer.uuid);
                }

                netPlayerList.Remove(netPlayer.uuid);
            }
        }

        /// <summary>
        /// 场景加载完成后调用,开启战斗
        /// </summary>
        public virtual void StartBattleCore()
        {
            isStartRunRoom = false;

            battleCore.isRun = true;
        }
      
    }
}
