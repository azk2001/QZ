using System;
using System.Collections.Generic;

using UnityEngine;

public class cs_dungeon
{
	public int id { get; private set; }
	public int gameTime;
	public int startTime;
	public string sceneName;
	public Vector2 mapSize;
	public Vector4 cameraAstrict;
	public Vector2 cameraMoveDistance;
	public int cameraMoveDepth;
	public Vector4 cameraStopDistance;
	public int propProbability;
	public int cameraStopDepth;

	public static bool ContainsThis(int id)
	{
		return gInfoDic.ContainsKey(id);
	}
	
	public static cs_dungeon GetThis(int id)
	{
		cs_dungeon cfg = gInfoDic.TryGetValue(id);
		if (cfg == null)
			Debug.LogError("cs_dungeon table found no id: " + id);
		return cfg;
	}
	
	public static readonly Dictionary<int, cs_dungeon> gInfoDic = new Dictionary<int, cs_dungeon>();

}
