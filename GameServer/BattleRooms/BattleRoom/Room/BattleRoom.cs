using System;
using System.Collections.Generic;

namespace GameServer
{
    class BattleRoom : RoomBase
    {
      
        public int sceneId = 10000;                     //当前副本场景ID;
        protected bool isPinging = false;           //是否在ping中;
        protected float pingOutTime = 0;
        protected bool isStartRunRoom = false;      //是否是第一次运行这个房间;

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

        //战斗房间开始运行;
        public override void StartScene()
        {
            isStartRunRoom = true;

            battleCore = BattleCore.CreateBattleCore(1);
            battleCore.Init(sceneId, roomIndex);

            foreach(NetPlayer sPlayer in netPlayerList)
            {
                PlayerInScene(sPlayer);
            }
            
            base.StartScene();
        }

        //战斗房间结束运行;
        public override void EndScene()
        {

            GameUnitManager.RemoveRoomAllGameUnit(roomIndex);

            netPlayerList.Clear();

            battleCore.End();

            base.EndScene();
        }

        public override void Update(float deltaTime)
        {
            if (isRun == false)
                return;

            battleCore.Update(deltaTime);

            base.Update(deltaTime);
        }

        /// <summary>
        /// 检查所有场景是否加载完成
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="isLoaded"></param>
        /// <returns></returns>
        public override bool allPlayerReady()
        {
            bool allReady = base.allPlayerReady();

            if(allReady)
            {
                StartBattle();
            }

            return allReady;
        }
        
        public override void SetPlayerReady(NetPlayer netPlayer, bool isReady)
        {
            if (netPlayer!=null)
            {
                netPlayer.SetRoomState(ePlayerRoomState.readyed);
            }
        }

        /// <summary>
        /// 角色加入战斗;
        /// </summary>
        public override void PlayerInScene(NetPlayer netPlayer)
        {
            battleCore.InScene(netPlayer);

            base.PlayerInScene(netPlayer);
        }

        /// <summary>
        /// 角色离开战斗;
        /// </summary>
        public override void PlayerLeaveScene(NetPlayer netPlayer)
        {
            if (netPlayer != null)
            {
                netPlayer.SetRoomState(ePlayerRoomState.outGame);//玩家主动离线;
                battleCore.LeaveScene(netPlayer);

                netPlayerList.Remove(netPlayer);
            }            
        }

        /// <summary>
        /// 场景加载完成后调用,开启战斗
        /// </summary>
        public override void StartBattle()
        {
            isStartRunRoom = false;
            battleCore.StartBattle();
        }
    }
}
