using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillFire : SkillBase
{
    public override void OnConfig(int _skillId)
    {
        base.OnConfig(_skillId);
    }

    protected override bool OnBegin()
    {
        OnFire();
        TimeManager.Instance.Begin(0.3f, OnFire);

        return base.OnBegin();
    }

    protected override float OnFire()
    {
        Vector3 forward = gameUnit.mUnitController.transform.forward;
        Transform fireTrans = BattleEffectRoot.Instance.SpwanPrefab("Fire1");
        SkillController skillController = fireTrans.GetComponent<SkillController>();
        skillController.OnFire(forward, 30, 2);
        skillController.OnColliderEnter = OnColliderEventEnter;
        fireTrans.position = gameUnit.mUnitController.transform.position + gameUnit.mUnitController.transform.forward + Vector3.up;
        base.OnFire();

        return 0.3f;
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
