using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpeedUp : BuffBase
{
	private float dataTime = 0;
	private float validTime = 10;
	private float offsetSpeed = 1f;
	private bool isAddSpeed = false;
	protected override bool OnInitConfig (Dictionary<string,string> textParam)
	{
		offsetSpeed=float.Parse(textParam["offsetSpeed"]);
		validTime=float.Parse(textParam["validTime"]);
		isAddSpeed =false;

		return base.OnInitConfig (textParam);
	}

	protected override bool OnBegin (GameUnit actor)
	{

		dataTime = 0;
		actor.gameUnitData.curAddSpeedNum++;

		if (actor.gameUnitData.curAddSpeedNum <= actor.gameUnitData.maxAddSpeedNum) {
			actor.SetSpeed (actor.mUnitController.moveSpeed + offsetSpeed);
			UIBattleRoot.instance.PlayUIBattleLable (actor.mUnitController.transformCaChe, Vector3.up, UIBattleLable.eLableType.sudu);

			isAddSpeed =true;
		} 
		else 
		{
			actor.gameUnitData.curAddSpeedNum = actor.gameUnitData.maxAddSpeedNum;
		}

		return base.OnBegin (actor);
	}

	protected override void UpdatePerDelta (GameUnit actor)
	{
		dataTime += Time.deltaTime;
		if (dataTime > validTime) 
		{
			OnEnd(actor);
		}
		base.UpdatePerDelta (actor);
	}

	protected override void OnEnd (GameUnit actor)
	{
		if (isAddSpeed == true) {
			actor.SetSpeed (actor.mUnitController.moveSpeed - offsetSpeed);
			actor.gameUnitData.curAddSpeedNum--;
		}

		base.OnEnd (actor);
	}
}
