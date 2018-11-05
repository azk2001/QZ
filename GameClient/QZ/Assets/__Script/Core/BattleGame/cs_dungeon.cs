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
	
	public static void Serialize(Table table)
	{
		int idIndex = table.TryGetColIndex("id");
		int gameTimeIndex = table.TryGetColIndex("gameTime");
		int startTimeIndex =  table.TryGetColIndex("startTime");
		int sceneNameIndex =  table.TryGetColIndex("sceneName");
		int mapSizeIndex = table.TryGetColIndex("mapSize");
		int cameraAstrictIndex = table.TryGetColIndex("cameraAstrict");
		int cameraMoveDistanceIndex =  table.TryGetColIndex("cameraMoveDistance");
		int cameraMoveDepthIndex = table.TryGetColIndex("cameraMoveDepth");
		int cameraStopDistanceIndex =  table.TryGetColIndex("cameraStopDistance");
		int propProbabilityIndex = table.TryGetColIndex("propProbability");
		int cameraStopDepthIndex = table.TryGetColIndex("cameraStopDepth");
		for (int i = 1, rowHeight = table.rowHeight; i < rowHeight; ++i)
		{
			cs_dungeon p = new cs_dungeon();
			p.id = table.TryGetInt(i, idIndex);
			p.gameTime = table.TryGetInt(i, gameTimeIndex);
			p.startTime = table.TryGetInt(i, startTimeIndex);
			p.sceneName=  table.TryGetString(i, sceneNameIndex);
			p.mapSize = table.TryGetVector2(i, mapSizeIndex,',');
			p.cameraAstrict = table.TryGetVector4(i, cameraAstrictIndex,',');
			p.cameraMoveDistance =  table.TryGetVector2(i, cameraMoveDistanceIndex,',');
			p.cameraMoveDepth = table.TryGetInt(i, cameraMoveDepthIndex);
			p.cameraStopDistance =  table.TryGetVector4(i, cameraStopDistanceIndex,',');
			p.propProbability = table.TryGetInt(i, propProbabilityIndex);
			p.cameraStopDepth = table.TryGetInt(i, cameraStopDepthIndex);
			if (gInfoDic.ContainsKey(p.id))
				Debug.LogError("cs_dungeon table key repeat, p.id: " + p.id);
			else
				gInfoDic.Add(p.id, p);
		}
	}
}
