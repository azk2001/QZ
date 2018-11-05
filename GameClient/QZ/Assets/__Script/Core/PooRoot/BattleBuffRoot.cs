using UnityEngine;
using System.Collections;

public class BattleBuffRoot 
{
	private string spwanPoolName = "BattleBuffRoot";

	private static BattleBuffRoot _instance = null;

	public static BattleBuffRoot instance 
	{
		get 
		{
			if(_instance == null)
			{
				_instance = new BattleBuffRoot();
			}

			return _instance;
		}
	}

	private SpwanPool _spwanPool = null;
	public SpwanPool spwanPool 
	{
		get {
			if(_spwanPool == null)
			{
				_spwanPool = PoolManager.instance.GetSpwanPool(spwanPoolName);
			}
			return _spwanPool;
		}
	}

	public Transform SpwanPrefab (string prefabName)
	{
		return spwanPool.SpwanPrefab (prefabName);
	}

	public Transform SpwanPrefab(string prefabName,float deSpwanTime)
	{
		return spwanPool.SpwanPrefab (prefabName,deSpwanTime);
	}

	public void DeSpwan(Transform trans)
	{
		spwanPool.DeSpwanPrefab (trans);
	}
	public void DeSpwanAll()
	{
		spwanPool.DeSpwanAllPrefab ();
	}
}
