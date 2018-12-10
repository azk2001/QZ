using System;
using System.Collections.Generic;
using UnityEngine;


public enum eMapType
{
    none = 0,
    mainCity = 1,              //主城
    pvpfight1V1 = 2,           //1V1;
    pvpFightChaos = 3,         //多人乱斗模式;
}

public enum eStarConditionType
{
    limitTime = 1,      //1规定时间;          param:80              规定事件单位秒;
    killMonster,        //2击杀指定怪物;      param:5|10001|10002   第一位填写需要击杀多少只，后面填写需要击杀的怪物ID，支持多个;
    killAllMonster,     //3击杀所有怪物;      param:                不用填写;
    existTime,          //4生存时间;          param:80              生成时间单位秒;
    endElementHp,       //5角色血量;          param：0|80           参数0表示玩家，其他模型填怪物ID;  80表示血量的百分比;
    protectMonster,     //6保护怪物血量;      param:10001|80        10001保护怪物ID, 80保护怪物的血量百分比; 
    ignoreMonster,      //7忽略怪物;          暂时没有做;
    killPlayer,         //8击杀对方玩家       param:10              敌人死亡次数
    killCamp,           //9击杀对方正营       param:10              敌人死亡次数
}

//星级条件
public class StarCondition
{
    public eStarConditionType starType = eStarConditionType.limitTime;
    public string param = "";
    public bool isShow = false;
    public string showText = "";  //界面显示文本;
    public string dungeonShowText = ""; //副本内显示文本;
    public bool isVictory = false; //是否十胜利条件;

    //--------------------运行时候赋值;
    public bool isFinish = false;  //是否已经完成;
    public int runParam = 0;        //运行时参数;

    public Dictionary<int, int> deathNum = new Dictionary<int, int>(); //记录击杀玩家次数
    public Dictionary<int, int> deathCamp = new Dictionary<int, int>(); //记录击杀正营次数
    public StarCondition Clone()
    {
        StarCondition star = new StarCondition();
        star.starType = this.starType;
        star.param = this.param;
        star.isShow = this.isShow;
        star.showText = this.showText;
        star.dungeonShowText = this.dungeonShowText;
        star.isVictory = this.isVictory;
        star.isFinish = this.isFinish;
        star.runParam = this.runParam;
        star.deathNum = this.deathNum;
        star.deathCamp = this.deathCamp;

        return star;
    }
}

public class dungeon_c
{
    private static readonly Dictionary<int, dungeon_c> gInfoDic = new Dictionary<int, dungeon_c>();
    private static readonly List<dungeon_c> gInfoList = new List<dungeon_c>();

    public readonly int mapId;
    public readonly string mapName;
    public readonly eMapType mapType;
    public readonly int mapConfig;
    public readonly string sceneName;
    public readonly int backMusic;
    public readonly string icon;
    public readonly int buffDropTime;
    public readonly int width;
    public readonly int height;
    public readonly int timeLimit;
    public readonly int lastMapId;
    public readonly string comReward;
    public readonly List<StarCondition> victoryConditionsList;
    public readonly List<StarCondition> starConditionsList;

    private dungeon_c(TabFileData.LineData file)
    {
        mapId = file.GetContentInt("mapId");
        mapName = file.GetContentStr("mapName");
        sceneName = file.GetContentStr("sceneName");
        icon = file.GetContentStr("icon");
        buffDropTime = file.GetContentInt("buffDropTime");
        width = file.GetContentInt("width");
        height = file.GetContentInt("height");
        timeLimit = file.GetContentInt("timeLimit");
        backMusic = file.GetContentInt("backMusic");
        mapType = (eMapType)file.GetContentInt("mapType");
        mapConfig = file.GetContentInt("mapConfig");
        comReward = file.GetContentStr("comReward");
        lastMapId = file.GetContentInt("lastMapId");
        string victoryConditions = file.GetContentStr("victoryConditions");
        string starConditions = file.GetContentStr("starConditions");

        victoryConditionsList = new List<StarCondition>();
        starConditionsList = new List<StarCondition>();

        string[] starText = victoryConditions.Split(';');
        for (int i = 0, max = starText.Length; i < max; i++)
        {
            string[] tempList = starText[i].Split(',');
            if (tempList.Length == 4)
            {
                StarCondition star = new StarCondition();

                star.starType = (eStarConditionType)tempList[0].ToInt(0);
                star.param = tempList[1];
                star.isShow = (tempList[2].ToInt(0) == 1);
                star.showText = tempList[3];
                star.dungeonShowText = tempList[3];
                star.isVictory = true;

                victoryConditionsList.Add(star);
            }
        }

        starText = starConditions.Split(';');
        for (int i = 0, max = starText.Length; i < max; i++)
        {
            string[] tempList = starText[i].Split(',');
            if (tempList.Length == 4)
            {
                StarCondition star = new StarCondition();

                star.starType = (eStarConditionType)tempList[0].ToInt(0);
                star.param = tempList[1];
                star.isShow = (tempList[2].ToInt(0) == 1);
                star.showText = tempList[3];
                star.dungeonShowText = tempList[3];
                star.isVictory = false;

                starConditionsList.Add(star);
            }
        }
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
