using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlrSkillDragPlayer : SkillBase {

	private float maxLength = 0;
	private float showTime = 0;
	private int handEffectId = 0;
	private Transform selfTreans = null;

	private GameUnit hitGameUnit = null;
	private Transform frozenTrans = null;
	private Transform handEffect = null;

	protected override bool OnBegin ()
	{
		return base.OnBegin ();
	}

	protected override float BeginEvent ()
	{

		return base.BeginEvent ();
	}


	private float OnSkillEnd () {
		OnEnd (mGameUnit);
		return -1;
	}

	protected override void OnEnd (GameUnit actor)
	{
		BattleEffectRoot.instance.DeSpwan(effectTransfrom);
		BattleEffectRoot.instance.DeSpwan(handEffect);

		base.OnEnd (actor);
	}
}
