using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer
{
    public enum eBirthUnitType
    {
        birthPlayer = 0,        //关卡玩家出生;
        birthMonster,           //关卡事件怪物出生;
        birthEvent,             //关卡事件触发;
        birthObstruct,          //关卡地图元件出生;
    }

    public class GameUnitNetData
    {
        public int uid;                                         //唯一标示ID;
        public byte camp;                                       //正营;
        public int modleId;                                     //模型ID;
        public Transform transform = new Transform();           //初始化位置;
        public byte isAIStart;                                  //是否游戏开始启动AI;
        public int curLife;                                     //当前血量;
        public int maxLife;                                     //最大血量;

        public short wave;                                      //波
        public short group;                                     //组
        public string eventParam = "";                               //事件参数

        public BattleUnitData battleUnit = new BattleUnitData();

    }

    public enum eAIBehaviorState
    {
        stopMove, //停止移动;
        atkUnit,
        moveToUnit,
        modleDirectionUnit,
        setChatTips,
    }

    public class GameUnit
    {
        //初始化数据
        public int uid;//唯一标示ID;
        public int roomIndex;  //角色所在的房间索引;
        public byte camp;               //正营;

        public Transform transform = new Transform();    //初始化位置;

        public bool isReady = false;  //玩家是否准备好，可以战斗了;

        public byte isAIStart = 0; //是否启动自动战斗;

        //运行时数据
        public Vector3 position;    //游戏单位当前的位置点;扩大了1000倍的int类型;
        public Vector3 dir;         //当前角色移动方向
        public int angleY;          //游戏单位当前的旋转值;

        public SkillDamagePoint skillDamagePoint = new SkillDamagePoint(); //打击点信息;

        public bool isDeath = false;  //游戏单位是否死亡;

        public List<int> buffList = new List<int>();//当前玩家的buffId;

        public int monsterId;  //怪物ID;

        public int curLife = 0; //当前血量;
        public int maxLife = 0; //最大血量;

        public eBirthUnitType birthUnitType = eBirthUnitType.birthPlayer;

        public string eventValue ="";    //事件参数;
        
        public GameUnit(int uid)
        {
            this.uid = uid;

            isDeath = false;
            isReady = false;
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
            curLife += (int)(maxLife * (0.01f * percent));

            if (curLife > maxLife)
            {
                curLife = maxLife;
            }
        }

        public void Revive()
        {
            isDeath = false;
            curLife = maxLife;
        }

    }

    public class SkillDamagePoint
    {
        public byte isDot = 0;
        public byte hit = 0;
        public byte crit = 0;

        public int damageValue = 1;       // 伤害数值
        public int flawedValue = 1;
        public int suckLife = 0;

        public int buffID = 0;

        public int hitInfoId;          //受击点表Id;
        public int behaviourId;        //行为树表Id
    }
}
