using System;
using System.Collections.Generic;
using UnityEngine;

public class skill_c
{
    public int skillId;
	public string icon;
	public float cdTime;
    public int type;
    public string aniName;
    public string param;
    public Dictionary<string,string> textParam;

    private skill_c(TabFileData.LineData file)
    {
        skillId = file.GetContentInt("skillId");
        icon = file.GetContentStr("icon");
        cdTime = file.GetContentFloat("cdTime");
        aniName = file.GetContentStr("aniName");
        type = file.GetContentInt("type");
        string[] tempParam = file.GetContentStr("param").Split(';');

        textParam = new Dictionary<string, string>();
        foreach (string str in tempParam)
        {
            string[] s = str.Split('=');
            if(s.Length == 2)
            {
                textParam[s[0]] = s[1];
            }
        }

    }

    public static bool ContainsThis(int skillId)
    {
        return gInfoDic.ContainsKey(skillId);
    }

    public static skill_c GetThis(int skillId)
    {
        skill_c cfg = null;
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
                skill_c info = new skill_c(data);
                gInfoDic.Add(id, info);
            }
        }
    }

    public static readonly Dictionary<int, skill_c> gInfoDic = new Dictionary<int, skill_c>();

}
