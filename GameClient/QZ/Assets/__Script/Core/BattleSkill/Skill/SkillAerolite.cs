using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//天空向下砸陨石
class SkillAerolite : SkillBase
{
    public float moveSpeed;         //移动速度;
    public float dropTime;          //掉落下来时间;
    public string dropEffectName;   //掉落特效名字;
    public string fireEffectName;   //爆炸特效名字;

    public override void OnConfig(int _skillId)
    {
        base.OnConfig(_skillId);
        
        moveSpeed = textParam["moveSpeed"].ToFloat(0);
        dropTime = textParam["dropTime"].ToFloat(0);

        dropEffectName = textParam["dropEffectName"];
        fireEffectName = textParam["fireEffectName"];
    }


    protected override bool OnBegin(Vector3 position, Vector3 forward)
    {

        base.OnBegin(position, forward);

        return true;
    }

    protected override float OnFire()
    {
        return base.OnFire();
    }
    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    protected override void OnEnd(GameUnit actor)
    {
        base.OnEnd(actor);
    }
}
