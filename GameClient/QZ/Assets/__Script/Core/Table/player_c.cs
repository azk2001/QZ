using System;
using System.Collections.Generic;
using UnityEngine;

public class player_c
{
    private static readonly Dictionary<int, player_c> gInfoDic = new Dictionary<int, player_c>();
    private static readonly List<player_c> gInfoList = new List<player_c>();

    public readonly int playerId;
    public readonly string icon;
    public readonly string resName;
    public readonly int life;
    public readonly int speed;
    public readonly int shield;
    public readonly int harm;
    public readonly int skillId;

    private player_c(TabFileData.LineData file)
    {
        playerId = file.GetContentInt("playerId");
        icon = file.GetContentStr("icon");
        resName = file.GetContentStr("resName");
        life = file.GetContentInt("life");
        speed = file.GetContentInt("speed");
        shield = file.GetContentInt("shield");
        harm = file.GetContentInt("harm");
        skillId = file.GetContentInt("skillId");

    }


    public static player_c Get(int playerId)
    {
        if (gInfoDic.ContainsKey(playerId))
            return gInfoDic[playerId];
        Debug.LogError("player_c 未能找到id: " + playerId.ToString());
        return null;
    }


    public static List<player_c> GetList()
    {
        return gInfoList;
    }


    public static void LoadTxt(TabFileData file)
    {
        List<TabFileData.LineData> list = file.GetLineData();
        for (int i = 0, max = list.Count; i < max; i++)
        {
            TabFileData.LineData data = list[i];
            int id = data.GetContentInt("playerId");
            if (gInfoDic.ContainsKey(id))
            {
                Debug.LogError("player_c配置表id重复:" + id);
            }
            else
            {
                player_c info = new player_c(data);
                gInfoDic.Add(id, info);
                gInfoList.Add(info);
            }
        }
    }
}