using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//甩球;
public class BattleSkillThrowBall : SkillBase
{
    private float delayExplodeTime = 3.0f; //延迟爆炸时间;
    private float mStrength = 0;            //炸弹威力;
    private float delayTime = 0;            //延迟爆炸时间;
    private float showTime = 0;             //爆炸显示时间;
    private float detectionTime = 0;		//监测时间;
	private int moveMarkPosition = 0;		//移动格子数量;
	private int bombEffectId=0;				//爆炸特效ID;

	private Vector2 markToPosition = Vector3.zero;

    protected override bool OnBegin()
    {
		
        return base.OnBegin();
    }

	protected override float BeginEvent()
	{
		mGameUnit.curBombNum++;
	

		return -1;
	}
	//直接释放;
	protected override void OnStraightFire()
	{
		
		OnFire();
		base.OnStraightFire ();
	}

    protected override float OnFire()
    {
	
        return base.OnFire();
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
