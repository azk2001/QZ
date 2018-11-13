using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer
{
    public class BattleCore
    {

        public int roomIndex = 0;                           //房间ID;
        public dungeon_b dungeonInfo;                       //关卡ID;
        public ElementManager elementManager = null;        //当前这场战斗的刷怪器控制器;
        public VictoryCondition victoryCondition = null;    //副本通关条件;
        public RoomBase roomBase = null;                    //房间;
        protected float runDeltaTime = 0;
        protected int dungeonRunTime = 0;                     //副本运行时间;

        public bool isRun = false;

        public ElementGroup playerElement = null; //角色出生点配置信息;

        public virtual void Start(RoomBase roomBase, int dungeonId, int roomIndex)
        {
            this.roomIndex = roomIndex;
            this.roomBase = roomBase;
            this.roomIndex = roomIndex;

            victoryCondition = new VictoryCondition();

            elementManager = new ElementManager();
            elementManager.Init(this, roomIndex);

            dungeonRunTime = 0;

            dungeonInfo = dungeon_b.Get(dungeonId);

            LoadSaveMap.LoadMap(dungeonInfo.mapConfig, elementManager);

            victoryCondition.Start(this);
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

        public virtual void RefreshRunTime(float deltaTime)
        {
            runDeltaTime += deltaTime;

            if (runDeltaTime < 1)
                return;

            runDeltaTime -= 1;

            dungeonRunTime += 1;

            victoryCondition.OnDungeonRunTime(dungeonRunTime);

        }

        public virtual bool ReduceLife(int atkUid, int killUid, int dataLife)
        {
            GameUnit gameUnit = GameUnitManager.GetGameUnit(killUid);

            if (gameUnit.isDeath == true)
                return false;

            gameUnit.runUnitData.life -= dataLife;

            victoryCondition.OnUnitReduceLife(killUid);

            if (gameUnit.runUnitData.life <= 0)
            {
                GameUnitDeath(atkUid, killUid);
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
        /// <param name="atkUid"></param>
        /// <param name="killUid"></param>
        public virtual void GameUnitDeath(int atkUid, int killUid)
        {
            return;
        }

        /// <summary>
        /// 游戏单位进入场景;
        /// </summary>
        /// <param name="netPlayer"></param>
        public virtual void InScene(NetPlayer netPlayer)
        {
            return;
        }

        /// <summary>
        /// 游戏单位离开游戏（暂时规划成主动离开游戏）
        /// </summary>
        public virtual void LeaveScene(int uid)
        {

        }

        public virtual void End()
        {
            isRun = false;
        }
    }
}
