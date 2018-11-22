using System;
using System.Collections.Generic;
using UnityEngine;

public class dungeon_c
{
    private static readonly Dictionary<int, dungeon_c> gInfoDic = new Dictionary<int, dungeon_c>();
    private static readonly List<dungeon_c> gInfoList = new List<dungeon_c>();

    public readonly int mapId;
    public readonly string mapName;
    public readonly int mapType;
    public readonly string sceneName;
    public readonly int backMusic;
    public readonly string icon;
    public readonly int timeLimit;
    public readonly int lastMapId;
    public readonly int comReward;
    public readonly string victoryConditions;
    public readonly string starConditions;
    public readonly int mapConfig;


    private dungeon_c(TabFileData.LineData file)
    {
        mapId = file.GetContentInt("mapId");
        mapName = file.GetContentStr("mapName");
        mapType = file.GetContentInt("mapType");
        sceneName = file.GetContentStr("sceneName");
        backMusic = file.GetContentInt("backMusic");
        icon = file.GetContentStr("icon");
        timeLimit = file.GetContentInt("timeLimit");
        lastMapId = file.GetContentInt("lastMapId");
        comReward = file.GetContentInt("comReward");
        victoryConditions = file.GetContentStr("victoryConditions");
        starConditions = file.GetContentStr("starConditions");
        mapConfig = file.GetContentInt("mapConfig");
    }


    public static dungeon_c Get(int mapId)
    {
        if (gInfoDic.ContainsKey(mapId))
            return gInfoDic[mapId];
        Debug.LogError("dungeon_c 未能找到id: " + mapId.ToString());
        return null;
    }


    public static List<dungeon_c> GetList()
    {
        return gInfoList;
    }


    public static void LoadTxt(TabFileData file)
    {
        List<TabFileData.LineData> list = file.GetLineData();
        for (int i = 0, max = list.Count; i < max; i++)
        {
            TabFileData.LineData data = list[i];
            int id = data.GetContentInt("mapId");
            if (gInfoDic.ContainsKey(id))
            {
                Debug.LogError("dungeon_c配置表id重复:" + id);
            }
            else
            {
                dungeon_c info = new dungeon_c(data);
                gInfoDic.Add(id, info);
                gInfoList.Add(info);
            }
        }
    }
}
