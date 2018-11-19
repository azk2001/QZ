using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class SkillBagBall : SkillBase
{
    public float moveSpeed;         //移动速度;
    public float showTime;          //移动最大时间;
    public string effectName;       //特效名字;

    public override void OnConfig(int _skillId)
    {
        base.OnConfig(_skillId);

        moveSpeed = textParam["moveSpeed"].ToFloat(0);
        showTime = textParam["showTime"].ToFloat(0);
        effectName = textParam["effectName"];
    }

    protected override bool OnBegin(Vector3 position,Vector3 forward)
    {
        


        base.OnBegin(position,forward);

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