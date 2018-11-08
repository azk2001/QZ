using System;
using System.Collections.Generic;

namespace BattleServer
{
    class BattleRoom : RoomBase
    {
        public BattleCore battleCore = null;                        //当前副本战役流程;
        public int sceneId = 0;         //当前副本场景ID;

        public DateTime roomStartTime = DateTime.MinValue;      //房间战斗开始时间;
        public DateTime roomEndTime = DateTime.MinValue;        //房间战斗结束时间;
        private int limitTime = 180;
        
        protected int mainPlayerUUID;          //当前战斗中的主驱动AI玩家UUID;
        protected DateTime pingTime;
        protected bool isPinging = false;       //是否在ping中;
        protected float pingOutTime = 0;
        protected bool isStartRunRoom = false; //是否是第一次运行这个房间;

        public override void Init(int roomIndex)
        {
            base.Init(roomIndex);
        }

        public override bool PlayerInRoom(NetPlayer netPlayer)
        {
           return base.PlayerInRoom(netPlayer);
        }

        public override bool PlayerOutRoom(NetPlayer netPlayer)
        {
            return base.PlayerOutRoom(netPlayer);
        }

        public override void Start(int sceneId)
        {
            this.sceneId = sceneId;

            isStartRunRoom = true;
            
            battleCore.Start(this, sceneId, roomIndex);

            roomStartTime = DateTime.Now;
            roomEndTime = DateTime.Now.AddSeconds(limitTime);

            base.Start(sceneId);
        }

        public override void End()
        {

            GameUnitManager.RemoveRoomAllGameUnit(roomIndex);

            netPlayerList.Clear();

            battleCore.End();

            base.End();
        }

        public override void Update(float deltaTime)
        {
            if (isRun == false)
                return;

            battleCore.Update(deltaTime);

            base.Update(deltaTime);
        }

        /// <summary>
        /// 移除玩家,玩家主动离线;
        /// </summary>
        /// <param name="outType">离开副本类型 1：主动离开场景  2:掉线离开</param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public virtual void PlayerQuitRoom(int outType, NetPlayer netPlayer)
        {
            if (netPlayerList.Contains(netPlayer))
            {
                if (netPlayer != null)
                {
                    if (outType == 1)
                    {
                        netPlayer.SetRoomState(ePlayerRoomState.outGame);//玩家主动离线;
                    }
                    else if (outType == 2)
                    {
                        if (netPlayer.roomState != ePlayerRoomState.outGame)
                        {
                            netPlayer.SetRoomState(ePlayerRoomState.offLine);//玩家主动离线;
                        }
                    }

                    battleCore.LeaveScene(netPlayer.uuid);
                }
            }
        }

        //设置场景加载完成
        public virtual void SetSceneLoaded(NetPlayer netPlayer, bool isLoaded)
        {
            if (netPlayer!=null)
            {
                netPlayer.SetRoomState(ePlayerRoomState.sceneLoaded);
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
        public virtual void SetPlayerReady(NetPlayer netPlayer, bool isReady)
        {
            if (netPlayer!=null)
            {
                netPlayer.SetRoomState(ePlayerRoomState.fighting);
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
            if (netPlayer != null)
            {
                netPlayer.SetRoomState(ePlayerRoomState.outGame);//玩家主动离线;
                battleCore.LeaveScene(netPlayer.uuid);

                netPlayerList.Remove(netPlayer);
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
