using System;
using System.Collections.Generic;

public class player_s
{
    private static readonly Dictionary<int, player_s> gInfoDic = new Dictionary<int, player_s>();
    private static readonly List<player_s> gInfoList = new List<player_s>();

    public readonly int playerId;
    public readonly string icon;
    public readonly string resName;
    public readonly int life;
    public readonly int speed;
    public readonly int shield;
    public readonly int harm;


    private player_s(TabFileData.LineData file)
    {
        playerId = file.GetContentInt("playerId");
        icon = file.GetContentStr("icon");
        resName = file.GetContentStr("resName");
        life = file.GetContentInt("life");
        speed = file.GetContentInt("speed");
        shield = file.GetContentInt("shield");
        harm = file.GetContentInt("harm");
    }


    public static player_s Get(int playerId)
    {
        if (gInfoDic.ContainsKey(playerId))
            return gInfoDic[playerId];
        Console.WriteLine("player_c 未能找到id: " + playerId.ToString());
        return null;
    }


    public static List<player_s> GetList()
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
                Console.WriteLine("player_c配置表id重复:" + id);
            }
            else
            {
                player_s info = new player_s(data);
                gInfoDic.Add(id, info);
                gInfoList.Add(info);
            }
        }
    }
}