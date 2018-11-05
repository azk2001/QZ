using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStrengthUp : BuffBase
{
	private float offsetStrength = 1f;
	private float validTime = 0;
	private float dataTime = 0;
	private bool isAddStrength = false;
	protected override bool OnInitConfig (Dictionary<string,string> textParam)
	{
		offsetStrength=float.Parse(textParam["offsetStrength"]);
		validTime=float.Parse(textParam["validTime"]);
		isAddStrength = false;
		return base.OnInitConfig (textParam);
	}

	protected override bool OnBegin (GameUnit actor)
	{
		actor.gameUnitData.curAddStrengthNum++;
		dataTime = 0;

		if (actor.gameUnitData.curAddStrengthNum <= actor.gameUnitData.maxAddStrengthNum) {
			actor.SetSkillStrength (actor.gameUnitData.strength + offsetStrength);
			UIBattleRoot.instance.PlayUIBattleLable (actor.mUnitController.transformCaChe, Vector3.up, UIBattleLable.eLableType.wuli);
			isAddStrength = true;
		} else {
			actor.gameUnitData.curAddStrengthNum = actor.gameUnitData.maxAddStrengthNum;
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
		if (isAddStrength == true) {
			actor.SetSkillStrength (actor.gameUnitData.strength - offsetStrength);
			actor.gameUnitData.curAddStrengthNum--;
		}

		base.OnEnd (actor);
	}
}
