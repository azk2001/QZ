using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffBase : BattleBase
{
	public Transform buffPrefab = null;
	public bool isInvincible = false;
	public BuffController buffController = null;

	public int mBuffStaticId = 0;
	public int buffType = 0;
	public Vector3 position;
	public Vector2 markPosition;
	private bool isRun = false;
	private bool isUpdate = true;		//是否在检查Buff;
	private cs_buff mBuffParam = null;
	private float buffDataTime = 0;
	private float invincibleDataTime = 0;

	public bool InitConfig(int buffStaticId,cs_buff buffParam,Vector3 position )
	{
		mBuffParam = buffParam;
		mBuffStaticId = buffStaticId;
		buffDataTime = 0;

		isUpdate = true;
		buffType = mBuffParam.buffId;
		string prefabName = c_mapResPath.GetThis (buffParam.prefabId).name;

		buffPrefab = BattleBuffRoot.instance.SpwanPrefab (prefabName);
		buffController = buffPrefab.GetComponent<BuffController> ();
		buffController.buffStaticId = buffStaticId;
		buffController.buffTypeId = buffType;
		buffPrefab.position = position;

		this.position = position;
		markPosition = MapManager.instance.PositionToMark (position);

		isInvincible = true;
		invincibleDataTime = 0;

		MapManager.instance.SetMapItemMark (markPosition,true,MapMarkValue.buff);

		return OnInitConfig(mBuffParam.textParam);
	}
	
	public bool BeginBuff( GameUnit actor)
	{
		RemoveBuff ();
	
		return OnBegin( actor );
	}
	
	public void EndBuff(GameUnit actor)
	{
		OnEnd(actor);
	}
	
	public void UpdateBuff(GameUnit actor)
	{
		if (isRun == true)
		{
			UpdatePerDelta (actor);
		}
	}

	public void Update(float deltaTime)
	{
		invincibleDataTime += deltaTime;

		if (invincibleDataTime >= mBuffParam.invincibleTime) {
			isInvincible=false;
		}

		if (isUpdate == false)
			return;

		buffDataTime += deltaTime;
		if (buffDataTime > mBuffParam.showTime)
		{
			RemoveBuff();
		}

		OnUpdate (deltaTime);
	}

	public void RemoveBuff()
	{
		isUpdate = false;
		BattleBuffRoot.instance.DeSpwan (buffPrefab);
		BuffManager.instance.RemoveBuff (mBuffStaticId);

		MapManager.instance.SetMapItemMark (markPosition,false,MapMarkValue.buff);

	}

	protected virtual bool OnBegin(GameUnit actor)
	{
		isRun = true;
		return true;
	}
	
	protected virtual void OnEnd(GameUnit actor)
	{
		isRun = false;
	
		if (isUpdate == true) 
		{
			RemoveBuff ();
			actor.RemoveBuff(this);
		}
	}

	protected virtual void UpdatePerDelta(GameUnit actor)
	{

	}

	protected virtual bool OnInitConfig(Dictionary<string,string> textParam)
	{
		return true;
	}

	protected virtual void OnUpdate(float deltaTime)
	{

	}


}
