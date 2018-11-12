using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameUnitData : ICloneable
{

    public int hp;          //玩家初始HP;
    public int speed;		//移动速度;
    public int atk;	        //攻击力;
    public int defense;     //防御力;
    public int campId;      //阵营ID;

    public int maxHP = 0;
    public int maxSpeed = 0;
    public int maxAkt = 0;
    public int maxDefense = 0;

    public object Clone()
    {
        return this.MemberwiseClone();
    }

}

public class GameUnitManager : SingleClass<GameUnitManager>
{
    public Dictionary<int, GameUnit> gameUintDic = new Dictionary<int, GameUnit>();

    private static int _gameUintStaticId = int.MaxValue;
    private static int gameUintStaticId
    {
        get
        {
            _gameUintStaticId--;
            return _gameUintStaticId;
        }
    }

    public GameUnit CreateServerGameUnit(int uuid, PlayerBasicsData _basicsData, GameUnitData data)
    {
        GameUnit gameUnit = null;
        if (gameUintDic.ContainsKey(uuid) == true)
        {
            gameUnit = gameUintDic[uuid];
        }
        else
        {
            gameUnit = new GameUnit();
            gameUintDic[uuid] = gameUnit;
        }

        gameUnit.Init(uuid, _basicsData, data);

        return gameUnit;
    }

    public void RemoveAllGameUnit()
    {
        foreach (var gu in gameUintDic)
        {
            RemoveGameUnit(gu.Value);
        }
        gameUintDic.Clear();
    }

    public void RemoveGameUnit(GameUnit gameUnit)
    {
        RemoveGameUnit(gameUnit.uuid);
    }

    public void RemoveGameUnit(int uuid)
    {
        if (gameUintDic.ContainsKey(uuid) == true)
        {
            gameUintDic[uuid].OnDestory();
            gameUintDic.Remove(uuid);
        }
    }

    public GameUnit GetGameUnit(int uuid)
    {
        if (gameUintDic.ContainsKey(uuid) == true)
        {
            return gameUintDic[uuid];
        }
        return null;
    }

    public List<GameUnit> GetAllGameUnit()
    {
        return new List<GameUnit>(gameUintDic.Values);
    }

    public void OnUpdate(float deltaTime)
    {
        foreach(GameUnit gameUnit in gameUintDic.Values)
        {
            gameUnit.OnUpdate(deltaTime);
        }
    }

}
