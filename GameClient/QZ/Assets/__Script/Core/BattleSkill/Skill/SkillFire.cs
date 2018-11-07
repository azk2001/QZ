using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillFire : SkillBase
{
    private float delayExplodeTime = 3.0f; //延迟爆炸时间;
    private float mStrength = 0;           //炸弹威力;
    private float delayTime = 0;            //延迟爆炸时间;
    private float showTime = 0;             //爆炸显示时间;
    private float detectionTime = 0;		//监测时间;
	private int bombEffectId;				//爆炸特效ID;
	private bool isFire = false;			//释放已经释放;

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
