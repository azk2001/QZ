using System;
using System.Collections;
using System.Collections.Generic;

//开始触发类型;
public enum eWaveTriggerType
{
	trigger,		//trigger触发;    服务器没有碰撞触发 不可用
	radius,			//半径触发;
	lastStartTime,	//上一波怪物出身后触发时间记时;
	lastOutTime,    //上一波怪物杀死后触发时间记时;
}
namespace GameServer
{
    public class ElementWave
    {
        public int curWave = 0;                 //波;
                                                //public int lastWave = 0;				//关联上一波;
        public int loopNum = 1;                 //循环次数;

        public int sponsorWave = 0;             //触碰触发波;
        public int sponsorGroup = 0;            //触碰触发组;

        public eWaveTriggerType elementTriggerType = eWaveTriggerType.trigger;      //触发trigger;

        public WaveBirthTrigger triggerGameObject = null;         //触发Trigger;


        public float radius = 0;            //半径触发;
        public float lastStartTime = 0;     //上一波怪物出身后触发时间记时;
        public float lastOutTime = 0;       //上一波怪物杀死后触发时间记时;

        //怪物组;
        public List<ElementGroup> elementGroupList = new List<ElementGroup>();

        public Transform transform = new Transform();

        //----------------------------------------------------------------------------
        //游戏运行用;
        public DateTime curBirthTime = DateTime.MaxValue;       //当前组出生的时间;
        public DateTime curEndTime = DateTime.MaxValue;         //当前组打完的时间;

        //循环用;
        public DateTime curLoopBirthTime = DateTime.MaxValue;   //当前组循环出生的时间;
        public DateTime curLoopEndTime = DateTime.MaxValue;     //当前组循环结束的时间;

        public bool isFininsh = false;  //设置当前波是否完成;

        public void Init()
        {

        }

        void Start()
        {

        }


        void Update()
        {

        }
    }
}

