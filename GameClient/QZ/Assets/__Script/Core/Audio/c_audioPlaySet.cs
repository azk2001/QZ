using System;
using System.Collections.Generic;
using UnityEngine;

public class c_audioPlaySet
{
	public int id { get; private set; }
	public int wave;
	public int group;
	public int pathId;
	public int value;
	
	public static bool ContainsThis(int id)
	{
		return gInfoDic.ContainsKey(id);
	}
	
	public static c_audioPlaySet GetThis(int id)
	{
		c_audioPlaySet cfg = gInfoDic.TryGetValue(id);
		if (cfg == null)
			Debug.LogError("c_audioPlaySet table found no id: " + id);
		return cfg;
	}
	
	public static readonly Dictionary<int, c_audioPlaySet> gInfoDic = new Dictionary<int, c_audioPlaySet>();
	
	public static void Serialize(Table table)
	{
		int idIndex = table.TryGetColIndex("id");
		int waveIndex = table.TryGetColIndex("wave");
		int groupIndex = table.TryGetColIndex("group");
		int pathIdIndex = table.TryGetColIndex("pathId");
		int valueIndex = table.TryGetColIndex("value");
		for (int i = 1, rowHeight = table.rowHeight; i < rowHeight; ++i)
		{
			c_audioPlaySet p = new c_audioPlaySet();
			p.id = table.TryGetInt(i, idIndex);
			p.wave = table.TryGetInt(i, waveIndex);
			p.group = table.TryGetInt(i, groupIndex);
			p.pathId = table.TryGetInt(i, pathIdIndex);
			p.value = table.TryGetInt(i, valueIndex);
			if (gInfoDic.ContainsKey(p.id))
				Debug.LogError("c_audioPlaySet table key repeat, p.id: " + p.id);
			else
				gInfoDic.Add(p.id, p);
		}
	}
	
}
