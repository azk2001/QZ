using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : SingleClass<SkillManager>
{
    private static int _skillStaticId = int.MaxValue;
    public static int skillStaticId
    {
        get
        {
            _skillStaticId--;
            return _skillStaticId;
        }
    }
    public Dictionary<int, System.Type> skillType = new Dictionary<int, System.Type>()
    {
        {1,typeof(BattleSkillFire)},		//普通 释放炸弹;
	};


    public Dictionary<int, SkillBase> allBattleSkillDic = new Dictionary<int, SkillBase>();
    public Dictionary<Vector2, SkillBase> markPositionBattleSkill = new Dictionary<Vector2, SkillBase>();

    public bool PlayLocalSkill(GameUnit gameUnit, Vector3 position, int skillId, GameUnitData gameUnitData)
    {
        bool isPlay = false;
        int staticId = SkillManager.skillStaticId;

        int skillTypeId = cs_skill.GetThis(skillId).type;

        SkillBase bt = (SkillBase)(System.Activator.CreateInstance(skillType[skillTypeId]));
        if (bt != null)
        {
            isPlay = bt.OnBegin(staticId, gameUnit, position, skillId, gameUnitData);
        }

        return isPlay;
    }

    public bool PlayServerSkill(int staticId, GameUnit gameUnit, Vector3 position, int skillId, GameUnitData gameUnitData)
    {
        bool isPlay = false;

        int skillTypeId = cs_skill.GetThis(skillId).type;
        SkillBase bt = (SkillBase)(System.Activator.CreateInstance(skillType[skillTypeId]));
        if (bt != null)
        {
            isPlay = bt.OnBegin(staticId, gameUnit, position, skillId, gameUnitData);
        }

        return isPlay;
    }


    public SkillBase GetBattleSkill(int skillStaticId)
    {
        if (allBattleSkillDic.ContainsKey(skillStaticId) == true)
        {
            return allBattleSkillDic[skillStaticId];
        }
        return null;
    }
    public SkillBase GetBattleSkill(Vector2 markPosition)
    {
        if (markPositionBattleSkill.ContainsKey(markPosition) == true)
        {
            return markPositionBattleSkill[markPosition];
        }
        return null;
    }

    public List<SkillBase> GetAllBattleSkill()
    {
        return new List<SkillBase>(allBattleSkillDic.Values);
    }

    public void SkillEnd(SkillBase skill)
    {
        int skillStaticId = skill.mSkillStaticId;
        Vector2 markPosition = skill.markPosition;

        allBattleSkillDic.Remove(skillStaticId);
        markPositionBattleSkill.Remove(markPosition);
    }
}
