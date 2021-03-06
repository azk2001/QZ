using System.Collections;
using System.Collections.Generic;
using System;

//原件类型;
public enum eElementType
{
    monster = 1,        //出生怪物;
    player,         //玩家出生点;	
    eventTrigger,	//事件触发;
    obstruct,       //阻挡;
}

//承接上一个触发类型;
public enum eElementTriggerType
{
    lastOutTime,
    lastBossHp,
    lastStartTime,
    curStartTime,
    curOutTime,
    lastElementNum,
}
namespace GameServer
{
    public class ElementGroup
    {
        public eElementType elementType = eElementType.monster;  //写入怪物的类型;
        public int curWave = 0;                 //波;
        public int curGroup = 1;                //组;
        public bool isNeglect = false;          //是否忽略，不计入战斗结束;
        public int loopNum = 1;                 //循环次数;
        public int lastGroup = 0;               //触发继承上一组怪物的编号;

        public Transform transform = new Transform();    //当前位置;

        public eElementTriggerType elementTriggerType = eElementTriggerType.lastOutTime;   //唤醒trigger;

        public bool aiStart = true;           //是否开始就启动AI;

        public GroupBirthTrigger triggerGameObject = null;//事件触发游戏物体;
        public float lastOutTime = 0;               //触发继承上一组怪物的结束时间;
        public int lastBossHp = 0;                  //触发继承上一组怪物的剩余血量(百分比);
        public float lastStartTime = 0;             //触发继承上一组怪物的出生时间;
        public float curStartTime = 0;              //继承当前组怪物开始的时间;
        public float curOutTime = 0;                //继承上一组怪物事件开始的时间;
        public int lastElementNum = 0;              //上一波怪物数量;

        //所有有怪物的存放器;
        public List<ElementParam> elementParamList = new List<ElementParam>();

        //----------------------------------------------------------------------------
        //游戏运行用;
        public DateTime curBirthTime = DateTime.MaxValue;     //当前组出生的时间;
        public DateTime curEndTime = DateTime.MaxValue;       //当前组打完的时间;

        //循环用;
        public DateTime curLoopBirthTime = DateTime.MaxValue;     //当前组循环出生的时间;
        public DateTime curLoopEndTime = DateTime.MaxValue;       //当前组循环出生的时间;

        public int curBossHP = 100;            //当前怪物组的HP,Boss才能用(百分比);

        public bool isFinish = false;          //当前组是否完成;

        public void Init()
        {

        }

    }
}

