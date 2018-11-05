using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSkillInvincible : SkillBase
{
	private float invincibleTime = 0;		//无敌时间;
    protected override bool OnBegin()
    {
		invincibleTime = float.Parse(textParam["invincibleTime"]);

        return base.OnBegin();
    }

	protected override float BeginEvent()
	{
		mGameUnit.isInvincible =true;

		return -1;
	}

	private float OnFile()
	{
		OnEnd (mGameUnit);
	 	return -1;
	}

    protected override void OnEnd(GameUnit actor)
    {
		BattleEffectRoot.instance.DeSpwan(effectTransfrom);
		mGameUnit.SetColor (Color.white,0);
		mGameUnit.isInvincible =false;
        base.OnEnd(actor);
    }
}
