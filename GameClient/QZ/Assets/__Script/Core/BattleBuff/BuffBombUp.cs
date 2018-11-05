using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBombUp : BuffBase
{
	private int offsetBomb = 1;
	private float validTime = 0;
	private float dataTime = 0;
	private bool isAddBomb = false;
	protected override bool OnInitConfig (Dictionary<string,string> textParam)
	{
		offsetBomb=int.Parse(textParam["offsetBomb"]);
		validTime=float.Parse(textParam["validTime"]);

		isAddBomb = false;
		return base.OnInitConfig (textParam);
	}

	protected override bool OnBegin (GameUnit actor)
	{
		actor.gameUnitData.curAddBombNum++;
		dataTime = 0;
		if (actor.gameUnitData.curAddBombNum <= actor.gameUnitData.maxAddBombNum) 
		{
			actor.SetBomb (actor.gameUnitData.curAddBombNum);
			UIBattleRoot.instance.PlayUIBattleLable (actor.mUnitController.transformCaChe, Vector3.up, UIBattleLable.eLableType.paopao);
			isAddBomb = true;
		} else {
			actor.gameUnitData.curAddBombNum = actor.gameUnitData.maxAddBombNum;
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
		if (isAddBomb == true) 
		{
			actor.gameUnitData.curAddBombNum--;
			actor.SetBomb (actor.gameUnitData.curAddBombNum);
		}
		base.OnEnd (actor);
	}
}
