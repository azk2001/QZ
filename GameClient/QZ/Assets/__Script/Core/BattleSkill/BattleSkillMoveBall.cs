using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//推球;
public class BattleSkillMoveBall : SkillBase
{
	private int moveMarkPosition = 0;		//移动格子数量;

    protected override bool OnBegin()
    {
		
        return false;
    }

	protected override float BeginEvent()
	{
		
		return -1;
	}
    protected override void OnEnd(GameUnit actor)
    {
		
        base.OnEnd(actor);
    }
}
