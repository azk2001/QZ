﻿using System;
using System.Collections.Generic;

public enum eGeneralMapType
{
    none = 0,
    mainCity = 1,               //主城
    pvpfightPK = 51,            //即时同步切磋副本;
    pvpFightTeam = 80,          //组队副本;
    pvpFightChaos = 81,         //多人副本乱斗模式;
    pvpfight1V1 =  82,          //1V1天梯
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
}

public enum eDungeonType
{
    common = 1, // 1是普通关卡，
    elite = 2,  //2是精英关卡，
    boss = 3,   //3是boss,
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

        return star;
    }
}

public class dungeon_b
{
    private static readonly Dictionary<int, dungeon_b> gInfoDic = new Dictionary<int, dungeon_b>();
    private static readonly List<dungeon_b> gInfoList = new List<dungeon_b>();
    private static readonly List<int> battleIdList = new List<int>();                                               //普通章节ID; 
    private static readonly Dictionary<int, List<dungeon_b>> battleDic = new Dictionary<int, List<dungeon_b>>();    //普通关卡副本;
    public readonly int mapId;
    public readonly string name;
    public readonly int levelLimit;
    public readonly eGeneralMapType generalMap;
    public readonly int linemaxPlayers;
    public readonly int birthPointType;
    public readonly int mapConfig;
    public readonly int timeLimit;
    public readonly List<StarCondition> victoryConditionsList;
    public readonly List<StarCondition> starConditionsList;

    private dungeon_b(tab_file_data.line_data file)
    {
        mapId = file.get_content_int("mapId");
        name = file.get_content_str("name");
        levelLimit = file.get_content_int("levelLimit");
        generalMap = (eGeneralMapType)file.get_content_int("generalMap");
        linemaxPlayers = file.get_content_int("linemaxPlayers");
        birthPointType = file.get_content_int("birthPointType");
        mapConfig = file.get_content_int("mapConfig");
        timeLimit = file.get_content_int("timeLimit");

        string victoryConditions = file.get_content_str("victoryConditions");
        string starConditions = file.get_content_str("starConditions");

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


    public static dungeon_b Get(int MapID)
    {
        if (gInfoDic.ContainsKey(MapID))
            return gInfoDic[MapID];
        MyDebug.WriteLine("dungeon_c 未能找到id: " + MapID.ToString());
        return null;
    }

    public static List<dungeon_b> GetList()
    {
        return gInfoList;
    }

    public static List<dungeon_b> GetChapterList(int chapterId)
    {
        if (battleDic.ContainsKey(chapterId) == true)
            return battleDic[chapterId];

        return null;
    }

    public static void LoadTxt(tab_file_data file)
    {
        List<tab_file_data.line_data> list = file.get_line_data();
        for (int i = 0, max = list.Count; i < max; i++)
        {
            tab_file_data.line_data data = list[i];
            int id = data.get_content_int("mapId");
            if (gInfoDic.ContainsKey(id))
            {
                MyDebug.WriteLine("dungeon_c配置表id重复:" + id);
            }
            else
            {
                dungeon_b info = new dungeon_b(data);
                gInfoDic.Add(id, info);
                gInfoList.Add(info);
            }
        }
    }
}
