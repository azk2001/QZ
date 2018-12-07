using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillFire : SkillBase
{
    public string muzzleEffectName; //枪口特效名字;
    public string bulletEffectName; //子弹特效名字;
    public float moveSpeed;         //飞行速度;
    public float showTime;          //飞行时间;

    public override void OnConfig(int _skillId)
    {
        base.OnConfig(_skillId);

        muzzleEffectName = textParam["muzzleEffectName"];
        bulletEffectName = textParam["bulletEffectName"];
        moveSpeed = textParam["moveSpeed"].ToFloat(0) ;
        showTime = textParam["showTime"].ToFloat(0);
    }

    protected override bool OnBegin(Vector3 position, Vector3 forward)
    {
        //抬抢时间
        //OnFire();
       
        TimeManager.Instance.Begin(skill.cdTime, OnFire);

        return base.OnBegin(position,forward);
    }

    protected override float OnFire()
    {
        Transform muzzleTrans = BattleEffectRoot.Instance.SpwanPrefab(muzzleEffectName,2);
        muzzleTrans.SetParent(gameUnit.mUnitController.transformCaChe);
        muzzleTrans.Reset();
        muzzleTrans.transform.position = gameUnit.mUnitController.fireTrans.position;


        Vector3 forward = CameraLookPlayer.Instance.GetFireForward();
        Transform bulletTrans = BattleEffectRoot.Instance.SpwanPrefab(bulletEffectName);
        SkillController skillController = bulletTrans.GetComponent<SkillController>();
        skillController.OnFire(forward, moveSpeed, showTime);
        skillController.OnColliderEnter = OnColliderEventEnter;
        bulletTrans.position = gameUnit.mUnitController.fireTrans.position;
        

        base.OnFire();

        return skill.cdTime;
    }

    private void OnColliderEventEnter(Collider other)
    {
        UnitController unitController = other.GetComponent<UnitController>();  //获取是不是碰撞到了角色

        if(unitController !=null)
        {
            GameUnit hitGameUnit = GameUnitManager.Instance.GetGameUnit(unitController.uuid);
            if(hitGameUnit!=null)
            {
                OnHit(hitGameUnit);
            }
        }
    }


    protected override void OnEnd(GameUnit actor)
    {
        TimeManager.Instance.End(OnFire);

        base.OnEnd(actor);
    }
}
