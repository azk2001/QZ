using System;
using System.Collections.Generic;

using UnityEngine;


public class cs_buff
{
    public int      buffId { get; private set; }
    public int      prefabId;
    public float    showTime;                   //buff显示时间;
	public float	invincibleTime;				//buff生效时长;
    public Dictionary<string,string> textParam;

    public static bool ContainsThis(int buffId)
    {
        return gInfoDic.ContainsKey(buffId);
    }

    public static cs_buff GetThis(int buffId)
    {
        cs_buff cfg = gInfoDic.TryGetValue(buffId);
        if (cfg == null)
            Debug.LogError("cs_buff table found no buffId: " + buffId);
        return cfg;
    }

    public static readonly Dictionary<int, cs_buff> gInfoDic = new Dictionary<int, cs_buff>();

   
}
