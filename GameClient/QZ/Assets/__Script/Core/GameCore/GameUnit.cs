using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public GameUnitParam gameUnitParam = null;
    public GameUnitData gameUnitData = null;
    public int mGameUintId = 0;
    public List<BuffBase> buffs = new List<BuffBase>();     //当前身上的所有buff;
    public List<SkillBase> skills = new List<SkillBase>();  //当前身上的所有技能ID;

    public Transform prefabModel = null;
    
    public bool isInvincible = false;	//是否无敌;
    public int campId = 0;              //阵营ID;
    public bool isDeading = false;      //是否在死亡过程中;

    private Transform deadEffect = null;
    private GameUnit killGameUnit = null;
    
    public void Init(int gameUintId, GameUnitParam param, GameUnitData data)
    {
        mGameUintId = gameUintId;
        gameUnitParam = param;
        gameUnitData = data;

        prefabModel = BattleUnitRoot.Instance.SpwanPrefab("PlayerNan1");

        mUnitController = prefabModel.GetComponent<UnitController>();
        mUnitController.Init();
        mUnitController.gameUintId = gameUintId;
        mUnitController.transform.localEulerAngles = Vector3.zero;

        campId = data.campId;

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

    public void SetSpeed(float speed)
    {
        gameUnitData.speed = speed;
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

    public void PlayAnimation(string aniname)
    {
        mUnitController.PlayAnimation(aniname);
    }

    public void PlayRunAnimation(Vector3 dir)
    {
        mUnitController.MoveDirection(dir * 0.5f);

        if(dir.magnitude >0)
        {
            mUnitController.PlayAnimation("IsRun", true);
        }
        else
        {
            mUnitController.PlayAnimation("IsRun", false);
        }

    }

    public void SetForward(Vector3 forward)
    {
        mUnitController.SetForward(forward);
    }

    public void OnUpdate(float deltaTime)
    {
        if(skills.Count >0)
        {
            for (int i = skills.Count - 1; i >= 0; i--)
            {
                skills[i].Update(deltaTime);
            }
            
        }

    }

    public void OnCollisionStartEvent(Collider collision)// 当进入碰撞器
    {
        if (collision != null)
        {

        }
    }

    public void OnCollisionEndEvent(Collider collision)// 当离开碰撞器
    {
        if (collision != null)
        {

        }
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

    public void OnTriggerExit(Collider other)
    {

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

    public bool OnSkill(int skillId)
    {
        //获取释放成功技能;
        Vector3 v3 = mUnitController.transformCaChe.position;
        bool isPlay = SkillManager.Instance.AddLocalSkill(this, v3, skillId);
        if (isPlay == false)
            return false;

        cs_skill skill = cs_skill.GetThis(skillId);

        //客服端直接释放;
        PlayAnimation(skill.aniName);

        //碰撞只信任自己
        AudioManager.instance.Play(AudioPlayIDCollect.ad_attck, v3);

        return true;
    }

    public void OnSkillEnd()
    {

    }

    public void StartDead(GameUnit killUnit)
    {

    }

    public void StopDead()
    {
        isDeading = false;
        killGameUnit = null;

        PlayAnimation("runTree");

        if (deadEffect != null)
        {
            BattleEffectRoot.instance.DeSpwan(deadEffect);
            deadEffect = null;
        }

    }

    public float EndDead()
    {
        TimeManager.Instance.End(EndDead);

        buffs.Clear();

        mUnitController.StopMove();

        RemoveEventTrigger();

        PlayAnimation("die");

        TimeManager.Instance.End(OnDelayDead);
        TimeManager.Instance.Begin(1.5f, OnDelayDead);

        if (deadEffect != null)
        {
            BattleEffectRoot.instance.DeSpwan(deadEffect);
            deadEffect = null;
        }

        return -1;
    }

    public float OnDelayDead()
    {
        isDeading = false;

        GameUnitManager.Instance.RemoveGameUnit(mGameUintId);
        BattleUnitRoot.Instance.DeSpwan(prefabModel);

        Reset();

        return -1;
    }
}
