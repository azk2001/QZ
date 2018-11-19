using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum eBattleEvent
{
    addBuff = 1000,
    removeBuff,
    addBomb,
    removeBomb,
    playerStartDead,
    playerStopDead,
    playerEndDead,
    playerRemove,
    dizzinessStart,
    dizzinessEnd,
}

public class GameUnit
{
    public UnitController mUnitController = null;
    public PlayerBasicsData basicsData = null;
    public BattleUnitData baseUnitData = null;
    public BattleUnitData runUnitData = null;
    public int uuid = 0;
    public List<BuffBase> buffs = new List<BuffBase>();     //当前身上的所有buff;
    public List<SkillBase> skills = new List<SkillBase>();  //当前身上的所有技能ID;

    public Transform prefabModel = null;

    public bool isInvincible = false;	//是否无敌;
    public bool isDeading = false;      //是否在死亡过程中;

    private GameUnit killGameUnit = null;

    public void Init(int uuid, PlayerBasicsData _basicsData, BattleUnitData data)
    {
        this.uuid = uuid;
        basicsData = _basicsData;
        baseUnitData = (BattleUnitData)data.Clone();
        runUnitData = (BattleUnitData)data.Clone();

        if (_basicsData.sex == 1)
        {
            prefabModel = BattleUnitRoot.Instance.SpwanPrefab("PlayerNan1");
        }
        else
        {
            prefabModel = BattleUnitRoot.Instance.SpwanPrefab("PlayerNv1");
        }

        mUnitController = prefabModel.GetComponent<UnitController>();
        mUnitController.Init();
        mUnitController.uuid = uuid;
        mUnitController.transform.localEulerAngles = Vector3.zero;

        SetSpeed(data.speed);

        Reset();

        AddEventTrigger();
    }


    public void Reset()
    {

        SetColor(Color.white, 0);
        SetAlpha(1);

        killGameUnit = null;

        buffs.Clear();
    }

    public void AddEventTrigger()
    {

    }

    public void RemoveEventTrigger()
    {

    }

    public void SetSpeed(int speed)
    {
        baseUnitData.speed = speed;
        mUnitController.moveSpeed = speed;
    }

    public void SetColor(Color color, float val = 0.5f)
    {
        mUnitController.SetColor(color, val);
    }

    public void SetAlpha(float val)
    {
        mUnitController.SetAlpha(val);
    }

    public void ShowRenderer(bool isShow)
    {
        mUnitController.ShowRenderer(isShow);
    }

    //播放动画
    public void PlayAnimation(string aniname)
    {
        mUnitController.PlayAnimation(aniname);
    }

    //设置角色朝着指定方向移动，并且播放移动动画;
    public void MoveDirection(Vector3 dir)
    {
        mUnitController.MoveDirection(dir * 0.5f);
    }


    public void SetPosition(Vector3 position)
    {
        mUnitController.SetPosition(position);
    }

    public void SetForward(Vector3 forward)
    {
        mUnitController.SetForward(forward);
    }

    public void OnUpdate(float deltaTime)
    {
        if (skills.Count > 0)
        {
            for (int i = skills.Count - 1; i >= 0; i--)
            {
                skills[i].Update(deltaTime);
            }
        }

    }

    public void OnHit(GameUnit killGameUnit)
    {
        runUnitData.life -= killGameUnit.runUnitData.harm;
    }

    public void OnTriggerEnter(Collider other)
    {
        //碰撞到了buff;
        BuffController buffController = other.GetComponent<BuffController>();
        if (buffController != null)
        {
            BuffBase buffBase = BuffManager.Instance.GetBuff(buffController.uuid);
            if (buffBase != null)
            {
                AddBuff(buffBase);
            }
        }
    }

    public void AddBuff(BuffBase buff)
    {
        buffs.Add(buff);
        buff.BeginBuff(this);
    }

    public void RemoveBuff(BuffBase buff)
    {
        buffs.Remove(buff);
    }

    public bool OnSkill(int skillIndex,Vector3 forward)
    {
        int val = 0;
        bool isFire = true;
        switch (skillIndex)
        {
            case 0:
                {
                    val = 0;
                    isFire = false;
                }
                break;
            case 1:
                {
                    val = 0;
                    isFire = true;
                }
                break;
            case 2:
                {
                    val = 1;
                    isFire = true;
                }
                break;
            case 3:
                {
                    val = 2;
                    isFire = true;
                }
                break;
        }

        if (mUnitController.playerState == UnitController.PlayerState.roll)
            return false;

        if (isFire == true)
        {
            skills[val].Begin(this,mUnitController.transformCaChe.position, forward);
        }
        else
        {
            skills[val].End();
        }

        return true;
    }

    //翻滚
    public void RollPoint(Vector3 toPoint)
    {
        mUnitController.RollPosition(toPoint);
    }

    public void OnSkillEnd(SkillBase skillBase)
    {
        skills.Remove(skillBase);
    }

    //角色死亡;
    public void OnDead(GameUnit killUnit)
    {
        mUnitController.playerState = UnitController.PlayerState.die;

        isDeading = true;
        this.killGameUnit = killUnit;

        PlayAnimation("die");
        TimeManager.Instance.Begin(1, OnDestory);
    }

    //在GameUnitManager里面调用回收;
    public float OnDestory()
    {
        isDeading = false;

        TimeManager.Instance.End(OnDestory);
        BattleUnitRoot.Instance.DeSpwan(prefabModel);

        Reset();

        return -1;
    }

}
