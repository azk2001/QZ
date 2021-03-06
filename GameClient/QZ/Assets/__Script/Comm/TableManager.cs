﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TableManager : SingleClass<TableManager>
{

    //其实界面加载配置表
    private static Dictionary<string, int> loadStartList = new Dictionary<string, int>()
        {
            {"skill_c", 1},
            {"c_sfx",1},
            {"dungeon_c",1},
            {"player_c",1},
        };


    private bool isLoadGameList = false;

    public void Init()
    {
        textAssetList = Resources.LoadAll<UnityEngine.Object>("Config");
        isLoadGameList = false;

        AnalysisStartTable();
    }

    private UnityEngine.Object[] textAssetList = null;
    private int index = 0;

    public void AnalysisStartTable()
    {
        index = 0;

        for (int i = 0, max = textAssetList.Length; i < max; i++)
        {
            string name = textAssetList[i].name;

            if (loadStartList.ContainsKey(name) == true)
            {
                TextAsset textAsset = textAssetList[i] as TextAsset;

                TabFileData data = TabFile.parse(textAsset.text, loadStartList[name], name);

                LoadTxt(name, data);

                index++;
            }

            if (index >= loadStartList.Count)
            {
                AnalysisGameTable();
                break;
            }
        }
    }

    public void AnalysisGameTable()
    {
        if (isLoadGameList == true)
            return;

        isLoadGameList = true;

        index = 0;
    }

    public void LoadTxt(string name, TabFileData data)
    {
        switch (name)
        {
            case "skill_c":
                skill_c.LoadTxt(data);
                break;
            case "c_sfx":
                c_sfx.LoadTxt(data);
                break;
            case "player_c":
                player_c.LoadTxt(data);
                break;
            case "dungeon_c":
                dungeon_c.LoadTxt(data);
                break;
            case "buff_c":
                break;
        }

    }
}