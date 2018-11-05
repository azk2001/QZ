using System;
using System.Collections.Generic;
using UnityEngine;

public class cs_skill
{
    public int skillId { get; private set; }
	public string icon;
    public int prefabId;
	public float skillCastDelay;
	public string aniName;
	public float cdTime;
    public int type;
	public Dictionary<string,string> textParam;
   
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

    public static readonly Dictionary<int, cs_skill> gInfoDic = new Dictionary<int, cs_skill>();

}
