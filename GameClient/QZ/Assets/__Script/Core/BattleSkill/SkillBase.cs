using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public enum eOriginalType
{
    none,
    player,
    element,
    buff,
    bomb,
}

public class SkillBase
{
    public int uuid = 0;			            //技能唯一标示ID;
    public GameUnit gameUnit = null;            //释放者对象;
    public cs_skill skill = null;               //技能配置参数;
    public Dictionary<string, string> textParam;
    public Transform effectTransfrom = null;
    
    public bool Begin(int _uuid, GameUnit _gameUnit, Vector3 _position, int _skillId)
    {
        uuid = _uuid;
        gameUnit = _gameUnit;
        skill = cs_skill.GetThis(_skillId);

        return OnBegin();
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
        return -1;
    }

    protected virtual void OnUpdate(float deltaTime)
    {

    }


    protected virtual void OnEnd(GameUnit actor)
    {
        SkillManager.Instance.RemoveSkill(this);
    }
}

