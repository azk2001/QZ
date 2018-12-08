using System;
using System.Collections.Generic;
public class skill_s
{
    private static readonly Dictionary<int, skill_s> gInfoDic = new Dictionary<int, skill_s>();
    private static readonly List<skill_s> gInfoList = new List<skill_s>();

    public readonly int skillId;
    public readonly string icon;
    public readonly float cdTime;
    public readonly float conjureRadius;  //施法半径
    public readonly float fireRadius;    //爆炸半径
    public readonly int type;
    public readonly string aniName;
    public readonly Dictionary<string, string> textParam = new Dictionary<string, string>();


    private skill_s(TabFileData.LineData file)
    {
        skillId = file.GetContentInt("skillId");
        icon = file.GetContentStr("icon");
        cdTime = file.GetContentFloat("cdTime");
        conjureRadius = file.GetContentFloat("conjureRadius");
        fireRadius = file.GetContentFloat("fireRadius");
        type = file.GetContentInt("type");
        aniName = file.GetContentStr("aniName");
        string[] param = file.GetContentStr("param").Split(';');
        for (int i = 0, max = param.Length; i < max; i++)
        {
            string[] str = param[i].Split('=');
            if (str.Length == 2)
            {
                textParam[str[0]] = str[1];
            }
        }
    }


    public static skill_s Get(int skillId)
    {
        if (gInfoDic.ContainsKey(skillId))
            return gInfoDic[skillId];
        Console.WriteLine("skill_c 未能找到id: " + skillId.ToString());
        return null;
    }


    public static List<skill_s> GetList()
    {
        return gInfoList;
    }


    public static void LoadTxt(TabFileData file)
    {
        List<TabFileData.LineData> list = file.GetLineData();
        for (int i = 0, max = list.Count; i < max; i++)
        {
            TabFileData.LineData data = list[i];
            int id = data.GetContentInt("skillId");
            if (gInfoDic.ContainsKey(id))
            {
                Console.WriteLine("skill_c配置表id重复:" + id);
            }
            else
            {
                skill_s info = new skill_s(data);
                gInfoDic.Add(id, info);
                gInfoList.Add(info);
            }
        }
    }
}