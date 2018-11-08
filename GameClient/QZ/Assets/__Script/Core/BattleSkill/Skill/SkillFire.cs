using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillFire : SkillBase
{
    protected override bool OnBegin()
    {
        return base.OnBegin();
    }

	protected override float OnFire()
	{
		return -1;
	}

    private void OnColliderEventEnter(Collider other)
    {
     
    }


    protected override void OnEnd(GameUnit actor)
    {
	
		actor.OnSkillEnd();
        base.OnEnd(actor);
    }
}
