using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnitParam
{
	public int playerId = 0;
	
}
public class GameUnitData
{
	public float speed;		//移动速度;
	public int bombNum;		//释放炸弹个数;
	public float strength;	//炸弹力量;
	public int hp;			//玩家初始HP;
	public int campId;		//阵营ID;

	public List<int> skills;//身上的的技能ID;

	public int curAddSpeedNum =0; 
	public int maxAddSpeedNum = 0;
	public int curAddBombNum =0; 
	public int maxAddBombNum =0;
	public int curAddStrengthNum = 0;
	public int maxAddStrengthNum = 0;
	public int maxHp = 5;

	public string name;
	public int playerId;

	public int horseType;
	public int horseCarId;
}

public class GameUnitManager  
{
	public Dictionary<int,GameUnit> gameUintDic = new Dictionary<int, GameUnit> ();

	private static GameUnitManager _instance = null;
	public static GameUnitManager instance 
	{
		get {
			if(_instance == null)
			{
				_instance = new GameUnitManager();
			}
			return _instance;
		}
	}

	private static int _gameUintStaticId = int.MaxValue;
	private static int gameUintStaticId {
		get {
			_gameUintStaticId --;
			return _gameUintStaticId;
		}
	}

    public GameUnit CreateServerGameUnit(int gameUintId, GameUnitParam param, GameUnitData data)
    {
		GameUnit gameUnit = null;
		if (gameUintDic.ContainsKey (gameUintId) == true) 
		{
			gameUnit = gameUintDic[gameUintId];
			gameUnit.gameUnitParam = param;
			gameUnit.gameUnitData = data;
		}
		else
		{
        	gameUnit = new GameUnit();
        	gameUnit.Init(gameUintId, param, data);

			gameUintDic[gameUintId] = gameUnit;
		}

       
        return gameUnit;
    }

    public GameUnit CreateLocalGameUnit(GameUnitParam param,GameUnitData data)
	{
		int gameUintId = gameUintStaticId;

		GameUnit gameUnit = null;

		if (gameUintDic.ContainsKey (gameUintId) == true) 
		{
			gameUnit = gameUintDic[gameUintId];
			gameUnit.gameUnitParam = param;
			gameUnit.gameUnitData = data;
		}
		else
		{
			gameUnit = new GameUnit();
			gameUnit.Init(gameUintId, param, data);
			
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
		gameUintDic.Clear ();
	}

	public void RemoveGameUnit(int gameUintId )
	{
		if (gameUintDic.ContainsKey (gameUintId) == true) {
			gameUintDic.Remove (gameUintId);
		}

	}

	public GameUnit GetGameUnit(int gameUintId )
	{
		if (gameUintDic.ContainsKey (gameUintId) == true)
		{
			return gameUintDic [gameUintId];
		}
		return null;
	}

	public GameUnit GetGameUnit(Vector2 markPosition)
	{
		return null;
	}

	public List<GameUnit> GetAllGameUnit()
	{
		return new List<GameUnit>(gameUintDic.Values);
	}

}
