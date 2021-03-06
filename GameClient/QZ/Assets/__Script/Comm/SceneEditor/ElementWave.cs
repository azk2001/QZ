using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//开始触发类型;
public enum eWaveTriggerType
{
	trigger,		//trigger触发;
	radius,			//半径触发;
	lastStartTime,	//上一波怪物出身后触发时间记时;
	lastOutTime,    //上一波怪物杀死后触发时间记时;
}

public class ElementWave : MonoBehaviour
{
	public int curWave = 0;     			//波;
	//public int lastWave = 0;				//关联上一波;
	public int loopNum = 1;				    //循环次数;

	public int sponsorWave = 0;  	        //触碰触发波;
	public int sponsorGroup = 0;  	        //触碰触发组;

	public eWaveTriggerType elementTriggerType = eWaveTriggerType.trigger;      //触发trigger;

	public GameObject triggerGameObject = null;         //触发Trigger;


	public float radius = 0;			//半径触发;
	public float lastStartTime = 0;  	//上一波怪物出身后触发时间记时;
	public float lastOutTime = 0;  		//上一波怪物杀死后触发时间记时;

	//怪物组;
	public ElementGroup[] elementGroupList
	{
		get{
			return this.gameObject.GetComponentsInChildren<ElementGroup>();
		}
	}

	//----------------------------------------------------------------------------
	//游戏运行用;
	public float curBirthTime = 10000;     	//当前组出生的时间;
	public float curEndTime = 10000;     	//当前组打完的时间;

	//循环用;
	public float curLoopBirthTime = 10000; 	//当前组循环出生的时间;
	public float curLoopEndTime = 10000; 	//当前组循环结束的时间;

	public bool isFininsh = false;	//设置当前波是否完成;

	public void Init()
	{

	}

	void Start () {
	
	}


	void Update () {
	
	}
}
