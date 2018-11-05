using UnityEngine;
using System.Collections;

public class UIBattleRoot 
{
	private string spwanPoolName = "UIBattleRoot";

	private static UIBattleRoot _instance = null;

	public static UIBattleRoot instance 
	{
		get 
		{
			if(_instance == null)
			{
				_instance = new UIBattleRoot();
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
