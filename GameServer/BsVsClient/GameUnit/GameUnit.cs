using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer
{
    public enum GameUnitType
    {
        none,
        player,
        monster,
    }

    public class GameUnit
    {
        //初始化数据
        public int uuid;//唯一标示ID;
        public int roomIndex;  //角色所在的房间索引;

        public PlayerBasicsData basicsData = null;
        public BattleUnitData baseUnitData = null;
        public BattleUnitData runUnitData = null;

        public GameUnitType unitType = GameUnitType.none;

        //运行时数据
        public Vector3 position;    //游戏单位当前的位置点;扩大了1000倍的int类型;

        public Transform transform;

        public bool isDeath = false;  //游戏单位是否死亡;

        public int modleId;         //模型ID;

        public List<int> buffList = new List<int>();//当前玩家的buffId;

        public GameUnit(int uuid,int roomIndex)
        {
            this.roomIndex = roomIndex;
            this.uuid = uuid;
        }

        public void Init( BattleUnitData unitData, PlayerBasicsData basicsData)
        {
            this.basicsData = basicsData;
            this.baseUnitData = (BattleUnitData)unitData.Clone();
            this.runUnitData = (BattleUnitData)unitData.Clone();

            isDeath = false;

            Revive();
        }

        public void AddBuff(int buffId)
        {
            buffList.Add(buffId);
        }

        public void RemoveBuff(int buffId)
        {
            if (buffList.Contains(buffId))
                buffList.Remove(buffId);
        }

        public void AddLife(int percent)
        {
            runUnitData.life += percent;

            if (runUnitData.life > baseUnitData.life)
            {
                runUnitData.life = baseUnitData.life;
            }
        }

        public void OnHit(GameUnit atkGameUnit)
        {
            runUnitData.life -= atkGameUnit.runUnitData.harm;
        }

        public void Revive()
        {
            isDeath = false;
            runUnitData.life = baseUnitData.life;
        }

    }
}
