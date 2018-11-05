using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//陷阱技能;
public class BattleSkillTrap : SkillBase {

	private float showTime = 0;
	private float trapTime = 0;
	private int trapEffectId = 0;
	public GameObject triggerObject = null;
	protected override bool OnBegin ()
	{
		
		return base.OnBegin ();
	}

	protected override float BeginEvent ()
	{
		
		return base.BeginEvent ();
	}

	private void OnColliderEvent(Collider col)
	{
		
	}

	private float OnSkillPlayEnd () {
	
		return -1;
	}
	
	protected override void OnEnd (GameUnit actor)
	{
	
		base.OnEnd (actor);
	}
}
