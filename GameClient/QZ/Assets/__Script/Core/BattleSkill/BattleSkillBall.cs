using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSkillBall : SkillBase
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
		if (isFire == true)
			return -1;

		isFire = true;


        return base.OnFire();
    }

    private void OnColliderEventEnter(Collider other)
    {
        UnitController unitController = other.transform.GetComponent<UnitController>();
        BombController bombController = other.transform.GetComponent<BombController>();
        BuffController buffController = other.transform.GetComponent<BuffController>();
        SkillController effectController = other.transform.GetComponent<SkillController>();

        if (bombController != null)
        {
			int skillStaticId = bombController.skillStaticId;
			SkillBase bs = BattleSkillManager.instance.GetBattleSkill(skillStaticId);
			if (bs != null)
			{
				bs.StraightFire();
			}
		}
		
		if (buffController != null)
		{
            int buffStaticId = buffController.buffStaticId;
            BuffBase bb = BuffManager.instance.GetBuff(buffStaticId);
            if (bb != null && bb.isInvincible==false)
            {
                bb.RemoveBuff();
            }
        }

     
    }


    protected override void OnEnd(GameUnit actor)
    {
	
		actor.OnSkillEnd();
        base.OnEnd(actor);
    }
}
