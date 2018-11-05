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

public enum eHorseCarType
{
	none,
	horseCar,
	max,
}

public class GameUnit
{
    public UnitController mUnitController = null;
    public GameUnitParam gameUnitParam = null;
    public GameUnitData gameUnitData = null;
    public int mGameUintId = 0;
    public List<BuffBase> buffs = new List<BuffBase>(); //当前身上的所有buff;
    public List<int> skills = new List<int>();      //当前身上的所有技能ID;

    public int curBombNum = 0;  //当前释放炸弹个数;

  
    public Transform prefabModel = null;
	public Transform horseCarModel = null;

	public HorseCarController mHorseCarController = null;

    public bool isInvincible = false;	//是否无敌;
    public int campId = 0;  //阵营ID;
    public bool isDeading = false;	//是否在死亡过程中;

	private Transform deadEffect = null;
	private GameUnit killGameUnit= null;

	private Vector2 _markPosition = Vector2.zero;

	private eHorseCarType horseCarType = eHorseCarType.none;

    public void Init(int gameUintId, GameUnitParam param, GameUnitData data)
    {
        mGameUintId = gameUintId;
        gameUnitParam = param;
        gameUnitData = data;
		horseCarType = (eHorseCarType)data.horseType;

        mUnitController = prefabModel.GetComponent<UnitController>();
        mUnitController.Init();
        mUnitController.gameUintId = gameUintId;
        mUnitController.transform.localEulerAngles = Vector3.zero;

		cs_player horseCarParam= cs_player.GetThis (gameUnitData.horseCarId);

        campId = data.campId;
        SetSpeed(data.speed);

		Reset ();

		AddEventTrigger ();
    }


	public void Reset()
	{
		PlayAnimation("runTree",horseCarType);
		PlayRunAnimation (Vector3.zero);

		mUnitController.teamShadow.gameObject.SetActive (false);

		SetColor(Color.white, 0);
		SetAlpha(1);

		mUnitController.isMove = true;
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
	
	public void SetSkillStrength(float strength)
    {
        gameUnitData.strength = strength;
    }

    public void SetBomb(int bombNum)
    {
        gameUnitData.bombNum = bombNum;
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
		mUnitController.ShowRenderer (isShow);
	}

	public void PlayAnimation(string aniname,eHorseCarType horseCarType)
	{
		mUnitController.PlayAnimation (aniname,horseCarType);
	}

	public void PlayRunAnimation(Vector3 dir)
	{
		mUnitController.RunAnimation (dir);
		mUnitController.MoveDirection(dir*0.5f);
	}

    public void OnUpdate()
    {
        if (buffs.Count > 0)
        {
            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                buffs[i].UpdateBuff(this);
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
            BuffBase buffBase = BuffManager.instance.GetBuff(buffController.buffStaticId);
            if (buffBase != null)
            {
                AddBuff(buffBase);
            }
        }

        //碰撞到了死亡了的角色;
        DeadEffectItem deadEffectItem = other.GetComponent<DeadEffectItem>();
        if (deadEffectItem != null)
        {
            GameUnit gameUnit = GameUnitManager.instance.GetGameUnit(deadEffectItem.mUnitController.gameUintId);
			if (gameUnit!=null && gameUnit != this)
            {
                if (gameUnit.campId == campId)
                {
                    gameUnit.isDeading = false;
                    AudioManager.instance.Play(AudioPlayIDCollect.ad_rescue, prefabModel.transform.localPosition);
                    gameUnit.StopDead();
                    gameUnit.SetSpeed(gameUnitData.speed);
                }
                else
                {
                    gameUnit.EndDead();
                }
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
        bool isPlay = BattleSkillManager.instance.PlayLocalSkill(this, v3, skillId, gameUnitData);
        if (isPlay == false)
            return false;

        cs_skill skill = cs_skill.GetThis(skillId);

        //客服端直接释放;
		PlayAnimation(skill.aniName,horseCarType);

        //碰撞只信任自己
        AudioManager.instance.Play(AudioPlayIDCollect.ad_attck, v3);
        

        return true;
    }

    public void OnSkillEnd()
    {
        curBombNum--;
    }

	//不能控制;
	public void DizzinessTime(float dizzinessTime)
	{


	}

	private float DizzinessEnd()
	{

		return -1;
	}

	public void StartDead(GameUnit killUnit)
    {
        
    }

	public void StopDead()
	{
		isDeading = false;
		mUnitController.isMove = true;
		killGameUnit = null;

		PlayAnimation("runTree",horseCarType);

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

		RemoveEventTrigger ();
		
		PlayAnimation("deadend",horseCarType);

		TimeManager.Instance.End (OnDelayDead);
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

		GameUnitManager.instance.RemoveGameUnit(mGameUintId);
        BattleUnitRoot.instance.DeSpwan(prefabModel);

		Reset ();

        return -1;
    }
}
