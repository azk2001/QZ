using System;
using System.Collections.Generic;

public class sfx_s
{
    private static readonly Dictionary<int, sfx_s> gInfoDic = new Dictionary<int, sfx_s>();
    private static readonly List<sfx_s> gInfoList = new List<sfx_s>();

    public readonly int id;
    public readonly string path;
    public readonly string fileName;


    private sfx_s(TabFileData.LineData file)
    {
        id = file.GetContentInt("id");
        path = file.GetContentStr("path");
        fileName = file.GetContentStr("fileName");
    }


    public static sfx_s Get(int id)
    {
        if (gInfoDic.ContainsKey(id))
            return gInfoDic[id];
        Console.WriteLine("sfx_c 未能找到id: " + id.ToString());
        return null;
    }


    public static List<sfx_s> GetList()
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
                Console.WriteLine("sfx_c配置表id重复:" + id);
            }
            else
            {
                sfx_s info = new sfx_s(data);
                gInfoDic.Add(id, info);
                gInfoList.Add(info);
            }
        }
    }
}
