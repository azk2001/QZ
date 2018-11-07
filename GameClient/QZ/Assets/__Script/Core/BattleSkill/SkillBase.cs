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
    public int mSkillStaticId = 0;			//技能唯一标示ID;
    public GameUnit mGameUnit = null;
    public Vector3 standardPosition = Vector3.zero;
    public Vector2 markPosition = Vector2.zero;
    public cs_skill skill = null;
    public GameUnitData mGameUnitData = null;
    public Dictionary<string, string> textParam;
    public Transform effectTransfrom = null;

    public bool isSetMark = false;

    public bool OnBegin(int skillStaticId, GameUnit gameUnit, Vector3 position, int skillId, GameUnitData gameUnitData)
    {
        isSetMark = false;

        mSkillStaticId = skillStaticId;
        mGameUnit = gameUnit;


        return OnBegin();
    }

    public void StraightFire()
    {
        OnStraightFire();
    }



    protected virtual bool OnBegin()
    {
        return true;
    }

    protected virtual float BeginEvent()
    {
        return -1;
    }

    protected virtual float OnFire()
    {
        ///爆炸声音
        AudioManager.instance.Play(AudioPlayIDCollect.ad_bomb, effectTransfrom.localPosition);

        return -1;
    }

    //直接释放;
    protected virtual void OnStraightFire()
    {

    }

    protected virtual void OnEnd(GameUnit actor)
    {
        SkillManager.Instance.SkillEnd(this);
    }
}

