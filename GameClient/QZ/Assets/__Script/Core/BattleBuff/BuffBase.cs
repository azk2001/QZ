using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffBase
{
	public Transform buffPrefab = null;
	public BuffController buffController = null;

	public int uuid = 0;
	public int buffType = 0;
	public Vector3 position;
	private bool isRun = false;
	private cs_buff buffParam = null;

    private float takeDataTime = 0;

    private GameUnit actor = null;  //触发者的游戏对象;

    //开始创角buff;
    public bool CreateBuff(int uuid,cs_buff buffParam,Vector3 position )
	{
		this.buffParam = buffParam;
		this.uuid = uuid;

        buffType = this.buffParam.buffId;

		buffController = buffPrefab.GetComponent<BuffController> ();
		buffController.uuid = uuid;
		buffController.buffTypeId = buffType;
		buffPrefab.position = position;

		this.position = position;

        TimeManager.Instance.Begin(this.buffParam.showTime, this.RemoveBuff);
		return OnInitConfig(this.buffParam.textParam);
	}
	
    //buff开始生效;
	public bool BeginBuff(GameUnit actor)
	{
        this.actor = actor;

        RemoveBuff ();
	
		return OnBegin( actor );
	}
	
    //buff效果结束;
	public void EndBuff(GameUnit actor)
	{
		OnEnd(actor);
	}
	
    //buff触发生效，执行心跳;
	public void Update(float deltaTime)
	{
        if (isRun == false)
            return;
        
        takeDataTime += deltaTime;

        if(takeDataTime > buffParam.invincibleTime)
        {
            EndBuff(this.actor);
        }

        OnUpdate(deltaTime);
	}

    //删除原始场景上的buff显示特效;
    public float RemoveBuff()
	{
		BattleBuffRoot.Instance.DeSpwan (buffPrefab);
		BuffManager.Instance.RemoveBuff (uuid);

        return -1;
	}

    //buff开始生效;
    protected virtual bool OnBegin(GameUnit actor)
	{
		isRun = true;
		return true;
	}

    //buff效果结束;
    protected virtual void OnEnd(GameUnit actor)
	{
		isRun = false;

        RemoveBuff();
        actor.RemoveBuff(this);
    }

	protected virtual void OnUpdate(float deltaTime)
	{

	}

	protected virtual bool OnInitConfig(Dictionary<string,string> textParam)
	{
		return true;
	}
    
}
