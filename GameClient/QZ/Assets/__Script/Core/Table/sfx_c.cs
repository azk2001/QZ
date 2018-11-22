using System;
using System.Collections.Generic;
using UnityEngine;

public class sfx_c
{
    private static readonly Dictionary<int, sfx_c> gInfoDic = new Dictionary<int, sfx_c>();
    private static readonly List<sfx_c> gInfoList = new List<sfx_c>();

    public readonly int id;
    public readonly string path;
    public readonly string fileName;


    private sfx_c(TabFileData.LineData file)
    {
        id = file.GetContentInt("id");
        path = file.GetContentStr("path");
        fileName = file.GetContentStr("fileName");
    }


    public static sfx_c Get(int id)
    {
        if (gInfoDic.ContainsKey(id))
            return gInfoDic[id];
        Debug.LogError("sfx_c 未能找到id: " + id.ToString());
        return null;
    }


    public static List<sfx_c> GetList()
    {
        return gInfoList;
    }


    public static void LoadTxt(TabFileData file)
    {
        List<TabFileData.LineData> list = file.GetLineData();
        for (int i = 0, max = list.Count; i < max; i++)
        {
            TabFileData.LineData data = list[i];
            int id = data.GetContentInt("id");
            if (gInfoDic.ContainsKey(id))
            {
                Debug.LogError("sfx_c配置表id重复:" + id);
            }
            else
            {
                sfx_c info = new sfx_c(data);
                gInfoDic.Add(id, info);
                gInfoList.Add(info);
            }
        }
    }
}
