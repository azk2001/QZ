using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSkillBall : SkillBase
{
    private float delayExplodeTime = 3.0f; //延迟爆炸时间;
    private float mStrength = 0;           //炸弹威力;
    private float delayTime = 0;            //延迟爆炸时间;
    private float showTime = 0;             //爆炸显示时间;
    private float detectionTime = 0;		//监测时间;
	private int bombEffectId;				//爆炸特效ID;
	private bool isFire = false;			//释放已经释放;
    protected override bool OnBegin()
    {
		if (mGameUnit.curBombNum >= mGameUnit.gameUnitData.bombNum)
			return false;

		Vector2 tempMarkPosition = GetNewMark (markPosition, Vector2.zero,0, 0,true);

		if (tempMarkPosition.Equals (Vector2.zero))
			return false;

		mStrength = mGameUnitData.strength;
		delayTime = float.Parse(textParam["delayTime"]);
		showTime = float.Parse(textParam["showTime"]);
		detectionTime = float.Parse(textParam["detectionTime"]);
		bombEffectId = int.Parse(textParam["bombEffectId"]);

		isFire = false;

        return base.OnBegin();
    }

	protected override float BeginEvent()
	{
		mGameUnit.curBombNum++;


		string prefabName = c_mapResPath.GetThis(skill.prefabId).name;
		
		effectTransfrom = BattleEffectRoot.instance.SpwanPrefab(prefabName);
		effectTransfrom.position = standardPosition;
		
		BombController bombController = effectTransfrom.GetComponent<BombController>();
		if (bombController == null)
		{
			bombController = effectTransfrom.gameObject.AddComponent<BombController>();
		}
		bombController.SetTrigger(true);
		bombController.skillStaticId = mSkillStaticId;

		MapMarkValue markValue = MapManager.instance.GetMapItemMark (markPosition);
		
		if ((markValue & MapMarkValue.element)!=0)
		{
			MapElementItem element = MapManager.instance.GetElementItem (markPosition);
			if (element.mapElement.isPlaySkill == 1) {
				bombController.SetRenderer (false);
			}

			if(element.mapElement.isDestory == 1)
			{
				isSetMark=true;
				MapManager.instance.SetMapItemMark (markPosition,true,MapMarkValue.bomb);
			}
		} else {
			isSetMark=true;
			MapManager.instance.SetMapItemMark (markPosition,true,MapMarkValue.bomb);
		}

		TimeManager.Begin(delayTime, OnFire);

		return -1;
	}

    //直接释放;
	protected override void OnStraightFire()
	{
		TimeManager.End(OnFire);
		OnFire();
		base.OnStraightFire ();
	}
	
    protected override float OnFire()
    {
		if (isFire == true)
			return -1;

		isFire = true;

		BattleEffectRoot.instance.DeSpwan(effectTransfrom);

		int length = (int)mStrength;

		int upLength = length, downLength = length, leftLength = length, rigthLength = length;

		upLength = GetItemLength(markPosition, Vector2.up, length);
		downLength = GetItemLength(markPosition, Vector2.down, length);
		leftLength = GetItemLength(markPosition, Vector2.left, length);
		rigthLength = GetItemLength(markPosition, Vector2.right, length);

		Debug.Log ("upLength:" + upLength + " downLength:" + downLength + " leftLength:" + leftLength + " rigthLength:" + rigthLength);
		
		string bombEffectName = c_mapResPath.GetThis(bombEffectId).name;

		Transform transEffect = BattleEffectRoot.instance.SpwanPrefab(bombEffectName, showTime);
        transEffect.position = standardPosition;
        SkillController effectController = transEffect.GetComponent<SkillController>();
        if (effectController != null)
        {
            effectController.Play(markPosition, upLength, downLength, leftLength, rigthLength, detectionTime);
           // effectController.OnColliderEvent = OnColliderEventEnter;
        }
		OnFileMarkPosiotion (markPosition);
		OnFileCollider (markPosition,Vector2.up, upLength);
		OnFileCollider (markPosition,Vector2.down, downLength);
		OnFileCollider (markPosition,Vector2.left, leftLength);
		OnFileCollider (markPosition,Vector2.right, rigthLength);

        OnEnd(mGameUnit);

        return base.OnFire();
    }

    private void OnColliderEventEnter(Collider other)
    {
        UnitController unitController = other.transform.GetComponent<UnitController>();
        BombController bombController = other.transform.GetComponent<BombController>();
        BuffController buffController = other.transform.GetComponent<BuffController>();
        MapElementItem elementItem = other.transform.GetComponent<MapElementItem>();
        SkillController effectController = other.transform.GetComponent<SkillController>();

        if (bombController != null)
        {
			int skillStaticId = bombController.skillStaticId;
			SkillBase bs = BattleSkillManager.instance.GetBattleSkill(skillStaticId);
			if (bs != null)
			{
				bs.StraightFire();
			}
		}
		
		if (buffController != null)
		{
            int buffStaticId = buffController.buffStaticId;
            BuffBase bb = BuffManager.instance.GetBuff(buffStaticId);
            if (bb != null && bb.isInvincible==false)
            {
                bb.RemoveBuff();
            }
        }

        if (elementItem != null)
        {
            if (elementItem.mapElement.isDestory == 1)
            {
                elementItem.curDestroyNum--;
				if (elementItem.curDestroyNum < 1)
                {
					int buffTypeId = cs_elementDrop.GetElemntDropIdToProbability(elementItem.mapElement.dropId);

					if(buffTypeId != 0 &&  mGameUnit.mUnitController.gameUintId == Client.clientId)
					{
						GameClient.C2SCreateBuff c2scb = new GameClient.C2SCreateBuff();
						c2scb.clientId = Client.clientId;
						c2scb.buffType = buffTypeId;
						c2scb.markX = (int)elementItem.position.x;
						c2scb.markY = (int)elementItem.position.y;

						GameClient.NetEvent.SendCreateBuff(c2scb);
					}

					MapManager.instance.DestroyElement(elementItem);
				}

				Transform boomEffect = BattleEffectRoot.instance.SpwanPrefab("BoomFileEffect_1",1);
				boomEffect.position = elementItem.transform.position;
            }
        }

        if (unitController != null)
        {
            GameUnit gu = GameUnitManager.instance.GetGameUnit(unitController.gameUintId);

			if (gu != null && unitController.gameUintId == Client.clientId && gu.isInvincible == false)
            {
                gu.gameUnitData.hp--;
                if (gu.gameUnitData.hp < 1)
                {
//					//碰撞只信任自己
//					if (mGameUnit.mGameUintId == Client.clientId)
//					{
//						GameClient.C2SPlayerDeadStart c2spd = new GameClient.C2SPlayerDeadStart ();
//						c2spd.clientId = Client.clientId;
//						GameClient.NetEvent.SendPlayerDeadStart (c2spd);
//						
//						PlayerController.instance.OnRemovePlayerController();
//					}

                    gu.StartDead(mGameUnit);
                }
				Transform boomEffect = BattleEffectRoot.instance.SpwanPrefab("BoomFileEffect_1",1);
				boomEffect.position = gu.mUnitController.transformCaChe.position;
            }

			if(gu.isInvincible ==true)
			{
				UIBattleRoot.instance.PlayUIBattleLable (gu.mUnitController.transformCaChe,Vector3.up, UIBattleLable.eLableType.mianyi);
			}
        }
    }


    protected override void OnEnd(GameUnit actor)
    {
		if (isSetMark == true) {
			MapManager.instance.SetMapItemMark (markPosition,false,MapMarkValue.bomb);
		}

		actor.OnSkillEnd();
        base.OnEnd(actor);
    }
}
