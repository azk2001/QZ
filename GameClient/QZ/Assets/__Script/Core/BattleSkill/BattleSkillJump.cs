using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSkillJump : SkillBase
{
	private int jumpMarkPosition= 0;		//无敌时间;
	private Vector3 endPosition = Vector3.zero;
    protected override bool OnBegin()
    {

		return base.OnBegin();
	}
	
	protected override float BeginEvent()
	{		
	

		return -1;
	}

	private float OnFile()
	{
		
	 	return -1;
	}

    protected override void OnEnd(GameUnit actor)
    {
        base.OnEnd(actor);
    }
}
