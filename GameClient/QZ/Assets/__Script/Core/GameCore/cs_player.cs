using System;
using System.Collections.Generic;
using UnityEngine;

public class cs_player
{
	public int   id { get; private set; }
	public int prefabId;
	public string icon;
	public int[] skills;
	public int HP;
	public float startSpeed;
	public int   startBombNum;
	public int   startStrength;
	public int   maxAddSpeedNum;
	public int   maxAddBombNum;
	public int   maxAddStrengthNum;
	public int   maxAddHPNum;
	public static bool ContainsThis(int id)
	{
		return gInfoDic.ContainsKey(id);
	}
	
	public static cs_player GetThis(int id)
	{
        cs_player cfg = null;
        gInfoDic.TryGetValue(id,out cfg);
		if (cfg == null)
			Debug.LogError("cs_player table found no id: " + id);
		return cfg;
	}
	
	public static readonly Dictionary<int, cs_player> gInfoDic = new Dictionary<int, cs_player>();
	
	
}