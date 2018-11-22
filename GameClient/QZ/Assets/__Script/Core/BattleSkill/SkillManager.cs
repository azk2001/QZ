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

    public void AddSkill(GameUnit gameUnit,int skillId)
    {
        int uuid = SkillManager.uuid;

        int skillTypeId = skill_c.Get(skillId).type;

        SkillBase sb = (SkillBase)(System.Activator.CreateInstance(skillType[skillTypeId]));
        if (sb != null)
        {
            sb.OnConfig(skillId);
            gameUnit.skills.Add(sb);
        }
    }
}
