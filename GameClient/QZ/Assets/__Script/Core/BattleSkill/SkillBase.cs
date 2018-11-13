using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SkillBase
{
    public GameUnit gameUnit = null;                //释放者对象;
    public cs_skill skill = null;                   //技能配置参数;
    public Dictionary<string, string> textParam;
    public Transform effectTransfrom = null;

    public virtual void OnConfig(int _skillId)
    {
        skill = cs_skill.GetThis(_skillId);
        textParam = skill.textParam;
    }

    public bool Begin(GameUnit _gameUnit)
    {
        gameUnit = _gameUnit;

        return OnBegin();
    }

    public void End()
    {
        OnEnd(gameUnit);
    }

    public void Update(float deltaTime)
    {
        OnUpdate(deltaTime);
    }

    protected virtual bool OnBegin()
    {
        return true;
    }

    protected virtual float OnFire()
    {
        //获取释放成功技能;
        Vector3 v3 = gameUnit.mUnitController.transformCaChe.position;

        //客服端直接释放;
        gameUnit.mUnitController.PlayAnimation(skill.aniName);
        gameUnit.mUnitController.PlayAnimation("IsFire", true);

        //碰撞只信任自己
        AudioManager.Instance.Play(AudioPlayIDCollect.ad_attck, v3);

        return -1;
    }

    //所有伤害统一入口;
    protected virtual void OnHit(GameUnit hitGameUnit)
    {
        //伤害只信任自己;
        if(hitGameUnit.uuid == BattleProtocol.UUID)
        {
            C2SPlayerHitMessage c2SPlayerHit = new C2SPlayerHitMessage();
            c2SPlayerHit.hitUUID = hitGameUnit.uuid;
            c2SPlayerHit.killUUID = this.gameUnit.uuid;

            BattleProtocolEvent.SendPlayerHit(c2SPlayerHit);
        }
    }

    protected virtual void OnUpdate(float deltaTime)
    {

    }

    protected virtual void OnEnd(GameUnit actor)
    {
        if(gameUnit != null)
        {
            gameUnit.mUnitController.PlayAnimation("IsFire", false);
        }
        
    }
}

