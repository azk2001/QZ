using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : SingleClass<SkillManager>
{
    private static int _uuid = int.MaxValue;
    public static int uuid
    {
        get
        {
            _uuid--;
            return _uuid;
        }
    }

    public Dictionary<int, System.Type> skillType = new Dictionary<int, System.Type>()
    {
        {1,typeof(SkillFire)},		//普通 释放炸弹;
	};


    public Dictionary<int, SkillBase> allSkillBaseDic = new Dictionary<int, SkillBase>();

    public bool AddLocalSkill(GameUnit gameUnit, Vector3 position, int skillId)
    {
        bool isPlay = false;
        int uuid = SkillManager.uuid;

        int skillTypeId = cs_skill.GetThis(skillId).type;

        SkillBase sb = (SkillBase)(System.Activator.CreateInstance(skillType[skillTypeId]));
        if (sb != null)
        {
            isPlay = sb.Begin(uuid, gameUnit, position, skillId);
        }

        return isPlay;
    }

    public bool AddServerSkill(int uuid, GameUnit gameUnit, Vector3 position, int skillId)
    {
        bool isPlay = false;

        int skillTypeId = cs_skill.GetThis(skillId).type;
        SkillBase sb = (SkillBase)(System.Activator.CreateInstance(skillType[skillTypeId]));
        if (sb != null)
        {
            isPlay = sb.Begin(uuid, gameUnit, position, skillId);
        }

        return isPlay;
    }


    public SkillBase GetSkillBase(int uuid)
    {
        if (allSkillBaseDic.ContainsKey(uuid) == true)
        {
            return allSkillBaseDic[uuid];
        }
        return null;
    }

    public List<SkillBase> GetAllBattleSkill()
    {
        return new List<SkillBase>(allSkillBaseDic.Values);
    }

    public void RemoveSkill(SkillBase skill)
    {
        int uuid = skill.uuid;

        RemoveSkill(uuid);
    }

    public void RemoveSkill(int uuid)
    {
        if(allSkillBaseDic.ContainsKey(uuid))
        {
            allSkillBaseDic.Remove(uuid);
        }
    }
}
