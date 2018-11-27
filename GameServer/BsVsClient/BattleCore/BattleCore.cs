using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer
{
    public class BattleCore
    {

        public int roomIndex = 0;                               //房间ID;
        public dungeon_s dungeonInfo;                           //关卡ID;
        public ElementGroup playerElement = null;               //角色出生点配置信息;
        public VictoryCondition victoryCondition = null;        //副本通关条件;
        protected ElementManager elementManager = null;         //当前这场战斗的刷怪器控制器;
        protected float runDeltaTime = 0;
        protected int dungeonRunTime = 0;                       //副本运行时间;
        protected bool isRun = false;                          //房间是否运行;
        protected List<NetPlayer> netPlayerList = new List<NetPlayer>();//网络玩家列表;

        /// <summary>
        /// 初始化副本
        /// </summary>
        /// <param name="roomBase"></param>
        /// <param name="dungeonId"></param>
        /// <param name="roomIndex"></param>
        public virtual void Init(int dungeonId, int roomIndex)
        {
            this.roomIndex = roomIndex;

            victoryCondition = new VictoryCondition();

            elementManager = new ElementManager();
            elementManager.Init(this, roomIndex);

            dungeonRunTime = 0;

            dungeonInfo = dungeon_s.Get(dungeonId);

            LoadSaveMap.LoadMap(dungeonInfo.mapConfig, elementManager);

            victoryCondition.Start(this);

            isRun = false;
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        public virtual void StartBattle()
        {
            isRun = true;
        }

        /// <summary>
        /// 心跳函数
        /// </summary>
        /// <param name="dataTime"></param>
        public virtual void Update(float deltaTime)
        {
            if (isRun == false)
                return;

            elementManager.Update(deltaTime);
            RefreshRunTime(deltaTime);
        }

        /// <summary>
        /// 运行时间
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void RefreshRunTime(float deltaTime)
        {
            runDeltaTime += deltaTime;

            if (runDeltaTime < 1)
                return;

            runDeltaTime -= 1;
            dungeonRunTime += 1;

            victoryCondition.OnDungeonRunTime(dungeonRunTime);

        }

        /// <summary>
        /// 角色掉血
        /// </summary>
        /// <param name="atkUid"></param>
        /// <param name="killUid"></param>
        /// <param name="dataLife"></param>
        /// <returns></returns>
        public virtual bool ReduceLife(int hitUid, int atkUid)
        {
            GameUnit gameUnit = GameUnitManager.GetGameUnit(hitUid);

            if (gameUnit.isDeath == true)
                return false;

            gameUnit.OnHit(gameUnit);

            victoryCondition.OnUnitReduceLife(atkUid);

            BattleProtocolEvent.SendPlayerHit(atkUid, hitUid);

            if (gameUnit.runUnitData.life <= 0)
            {
                GameUnitDeath(hitUid, atkUid);
            }

            return true;
        }

        //地图配置玩家出生;
        public virtual void BirthPlayerEvent(ElementGroup element)
        {

        }

        public GameUnit CreateNetPlayer(NetPlayer netPlayer, ElementParam elementParam)
        {
            return null;
        }

        //地图配置怪物出生;
        public virtual List<GameUnit> BirthMonsterEvent(ElementGroup element)
        {
            return null;
        }

        public GameUnit CreateNetMonster(int uid, byte aiStart, ElementParam elementParam)
        {
            return null;
        }

        //地图配置事件触发;
        public virtual List<GameUnit> BirthEventEvent(ElementGroup element)
        {
            return null;
        }

        public GameUnit CreateNetEvent(int uid, byte aiStart, ElementParam elementParam)
        {
            return null;
        }

        //地图配置阻挡触发;
        public virtual List<GameUnit> BirthObstructEvent(ElementGroup element)
        {
            return null;
        }

        public GameUnit CreateObstructEvent(int uid, byte aiStart, ElementParam elementParam)
        {
            return null;
        }

        /// <summary>
        /// 游戏单位移动
        /// </summary>
        /// <param name="position"></param>
        public virtual void GameUnitMove(Vector3 position)
        {

        }

        /// <summary>
        /// 游戏单位死亡;
        /// </summary>
        /// <param name="atkUUID"></param>
        /// <param name="killUid"></param>
        public virtual void GameUnitDeath(int atkUUID, int hitUUID)
        {
            BattleProtocolEvent.SendPlayerDie(atkUUID, hitUUID);

            victoryCondition.OnUnitDeath(roomIndex, atkUUID, hitUUID);
        }

        /// <summary>
        /// 游戏单位进入场景;
        /// </summary>
        /// <param name="netPlayer"></param>
        public virtual void InScene(NetPlayer netPlayer)
        {
            if (netPlayerList.Contains(netPlayer) == false)
            {
                netPlayerList.Add(netPlayer);
            }
        }

        /// <summary>
        /// 游戏单位离开游戏（暂时规划成主动离开游戏）
        /// </summary>
        public virtual void LeaveScene(NetPlayer netPlayer)
        {

            if (netPlayerList.Contains(netPlayer) == true)
            {
                netPlayerList.Remove(netPlayer);
            }
        }

        public virtual void End()
        {
            isRun = false;
        }

        public static BattleCore CreateBattleCore(int battleType)
        {
            BattleCore battleCore = null;
            switch(battleType)
            {
                case 1:
                    {
                        battleCore = new SoloBattle();
                    }
                    break;
                case 2:
                    {

                    }
                    break;
            }
            return battleCore;
        }
    }
}
