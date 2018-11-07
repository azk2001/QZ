using System;
using System.Collections.Generic;
using UnityEngine;

public class cs_skill
{
    public int skillId;
	public string icon;
    public int prefabId;
	public float skillCastDelay;
	public string aniName;
	public float cdTime;
    public int type;
	public Dictionary<string,string> textParam;

    private cs_skill(TabFileData.LineData file)
    {
        skillId = file.GetContentInt("skillId");
        icon = file.GetContentStr("icon");
        prefabId = file.GetContentInt("prefabId");
        skillCastDelay = file.GetContentFloat("skillCastDelay");
        aniName = file.GetContentStr("aniName");
        cdTime = file.GetContentFloat("cdTime");
        type = file.GetContentInt("type");
        
    }

    public static bool ContainsThis(int skillId)
    {
        return gInfoDic.ContainsKey(skillId);
    }

    public static cs_skill GetThis(int skillId)
    {
        cs_skill cfg = null;
        gInfoDic.TryGetValue(skillId,out cfg);
        if (cfg == null)
            Debug.LogError("cs_skill table found no skillId: " + skillId);
        return cfg;
    }

    public static void LoadTxt(TabFileData file)
    {
        List<TabFileData.LineData> list = file.GetLineData();
        for (int i = 0, max = list.Count; i < max; i++)
        {
            TabFileData.LineData data = list[i];
            int id = data.GetContentInt("skillId");
            if (gInfoDic.ContainsKey(id))
            {
                Debug.LogError("npc_c配置表id重复:" + id);
            }
            else
            {
                cs_skill info = new cs_skill(data);
                gInfoDic.Add(id, info);
            }
        }
    }

    public static readonly Dictionary<int, cs_skill> gInfoDic = new Dictionary<int, cs_skill>();

}
