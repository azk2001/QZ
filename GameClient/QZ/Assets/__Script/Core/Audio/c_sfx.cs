using System;
using System.Collections.Generic;
using UnityEngine;


public class c_sfx
{
    public int id { get; private set; }
    public string path;
    public string fileName;

    public c_sfx(TabFileData.LineData file)
    {
        id = file.GetContentInt("id");
        path = file.GetContentStr("path");
        fileName = file.GetContentStr("fileName");
    }

    public static bool ContainsThis(int id)
    {
        return gInfoDic.ContainsKey(id);
    }

    public static c_sfx GetThis(int id)
    {
        c_sfx cfg = gInfoDic.TryGetValue(id);
        if (cfg == null)
            Debug.LogError("c_sfx table found no id: " + id);
        return cfg;
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
                Debug.LogError("npc_c配置表id重复:" + id);
            }
            else
            {
                c_sfx info = new c_sfx(data);
                gInfoDic.Add(id, info);
            }
        }
    }

    public static readonly Dictionary<int, c_sfx> gInfoDic = new Dictionary<int, c_sfx>();

}
