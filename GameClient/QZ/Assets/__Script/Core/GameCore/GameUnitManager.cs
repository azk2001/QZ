using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameUnitData : ICloneable
{

    public int hp;          //玩家初始HP;
    public float speed;		//移动速度;
    public float atk;	    //攻击力;
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
            gameUnit.basicsData = _basicsData;
            gameUnit.gameUnitData = data;
        }
        else
        {
            gameUnit = new GameUnit();
            gameUnit.Init(uuid, _basicsData, data);

            gameUintDic[uuid] = gameUnit;
        }


        return gameUnit;
    }

    public GameUnit CreateLocalGameUnit(PlayerBasicsData _basicsData, GameUnitData data)
    {
        int gameUintId = gameUintStaticId;

        GameUnit gameUnit = null;

        if (gameUintDic.ContainsKey(gameUintId) == true)
        {
            gameUnit = gameUintDic[gameUintId];
            gameUnit.basicsData = _basicsData;
            gameUnit.gameUnitData = data;
        }
        else
        {
            gameUnit = new GameUnit();
            gameUnit.Init(gameUintId, _basicsData, data);

            gameUintDic[gameUintId] = gameUnit;
        }
        return gameUnit;
    }

    public void RemoveAllGameUnit()
    {
        foreach (var gu in gameUintDic)
        {
            gu.Value.RemoveEventTrigger();
            gu.Value.Reset();
        }
        gameUintDic.Clear();
    }

    public void RemoveGameUnit(int gameUintId)
    {
        if (gameUintDic.ContainsKey(gameUintId) == true)
        {
            gameUintDic.Remove(gameUintId);
        }

    }

    public GameUnit GetGameUnit(int gameUintId)
    {
        if (gameUintDic.ContainsKey(gameUintId) == true)
        {
            return gameUintDic[gameUintId];
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
