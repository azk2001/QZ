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
	
	public static void Serialize(Table table)
	{
		int idIndex = table.TryGetColIndex("id");
		int prefabIdIndex = table.TryGetColIndex("prefabId");
		int iconIndex =  table.TryGetColIndex("icon");
		int skillsIndex = table.TryGetColIndex("skills");
		int HPIndex =  table.TryGetColIndex("HP");
		int startSpeedIndex = table.TryGetColIndex("startSpeed");
		int startBombNumIndex = table.TryGetColIndex("startBombNum");
		int startStrengthIndex = table.TryGetColIndex("startStrength");
		int maxAddSpeedNumIndex = table.TryGetColIndex("maxAddSpeedNum");
		int maxAddBombNumIndex = table.TryGetColIndex("maxAddBombNum");
		int maxAddStrengthNumIndex = table.TryGetColIndex("maxAddStrengthNum");
		int maxAddHPNumIndex =  table.TryGetColIndex("maxAddHPNum");
		for (int i = 1, rowHeight = table.rowHeight; i < rowHeight; ++i)
		{
			cs_player p = new cs_player();
			p.id = table.TryGetInt(i, idIndex);
			p.prefabId = table.TryGetInt(i, prefabIdIndex);
			p.icon = table.TryGetString(i, iconIndex);
			p.skills = table.TryGetIntArray(i, skillsIndex);
			p.HP = table.TryGetInt(i, HPIndex); 
			p.startSpeed = table.TryGetFloat(i, startSpeedIndex);
			p.startBombNum = table.TryGetInt(i, startBombNumIndex);
			p.startStrength = table.TryGetInt(i, startStrengthIndex);
			p.maxAddSpeedNum = table.TryGetInt(i, maxAddSpeedNumIndex);
			p.maxAddBombNum = table.TryGetInt(i, maxAddBombNumIndex);
			p.maxAddStrengthNum = table.TryGetInt(i, maxAddStrengthNumIndex);
			p.maxAddHPNum =  table.TryGetInt(i, maxAddHPNumIndex);
			if (gInfoDic.ContainsKey(p.id))
				Debug.LogError("cs_player table key repeat, p.id: " + p.id);
			else
				gInfoDic.Add(p.id, p);
		}
	}
	
}