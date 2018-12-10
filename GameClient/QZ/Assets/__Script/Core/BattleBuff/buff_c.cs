using System;
using System.Collections.Generic;
using UnityEngine;

public class buff_c
{
    private static readonly Dictionary<int, buff_c> gInfoDic = new Dictionary<int, buff_c>();
    private static readonly List<buff_c> gInfoList = new List<buff_c>();

    public readonly int buffId;
    public readonly string icon;
    public readonly string prefabName;
    public readonly int showTime; 
    public readonly int type;
    public readonly Dictionary<string, string> textParam;

    private buff_c(TabFileData.LineData file)
    {
        buffId = file.GetContentInt("buffId");
        icon = file.GetContentStr("icon");
        prefabName = file.GetContentStr("prefabName");
        showTime = file.GetContentInt("showTime");
        type = file.GetContentInt("type");
        string[] param = file.GetContentStr("param").Split(';');

        textParam = new Dictionary<string, string>();

        for(int i=0,max = param.Length;i<max;i++)
        {
            string[] str = param[i].Split('=');
            if(str.Length ==2)
            {
                textParam[str[0]] = str[1];
            }
        }

    }


    public static buff_c Get(int buffId)
    {
        if (gInfoDic.ContainsKey(buffId))
            return gInfoDic[buffId];
        Debug.LogError("buff_c 未能找到id: " + buffId.ToString());
        return null;
    }


    public static List<buff_c> GetList()
    {
        return gInfoList;
    }


    public static void LoadTxt(TabFileData file)
    {
        List<TabFileData.LineData> list = file.GetLineData();
        for (int i = 0, max = list.Count; i < max; i++)
        {
            TabFileData.LineData data = list[i];
            int id = data.GetContentInt("buffId");
            if (gInfoDic.ContainsKey(id))
            {
                Debug.LogError("buff_c配置表id重复:" + id);
            }
            else
            {
                buff_c info = new buff_c(data);
                gInfoDic.Add(id, info);
                gInfoList.Add(info);
            }
        }
    }
}