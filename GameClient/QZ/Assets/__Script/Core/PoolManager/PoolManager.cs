using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager{

	public Dictionary<string,SpwanPool> spwanPoolDic = new Dictionary<string, SpwanPool>();

	private static PoolManager _instance = null;

	public static PoolManager instance {
		get {
			if(_instance == null)
			{
				_instance = new PoolManager();
			}
			return _instance;
		}
	}

	public void AddSpwanPool (string spwanPoolName,SpwanPool spwanPool) 
	{
		if (spwanPoolDic.ContainsKey (spwanPoolName) == true) 
		{
			Debug.Log("已经有一个同样的名字了,请检查:"+spwanPoolName);
			return;
		}

		spwanPoolDic [spwanPoolName] = spwanPool;
	}

	public SpwanPool GetSpwanPool(string spwanPoolName) 
	{
		if (spwanPoolDic.ContainsKey (spwanPoolName) == false) 
		{
			Debug.Log("没有找到你所要删除的名字,请检查:"+spwanPoolName);
			return null;
		}
		return spwanPoolDic [spwanPoolName];
	}

	public void RemoveSpwanPool (string spwanPoolName) 
	{
		if (spwanPoolDic.ContainsKey (spwanPoolName) == false) 
		{
			Debug.Log("没有找到你所要删除的名字,请检查:"+spwanPoolName);
			return;
		}

		SpwanPool pool = spwanPoolDic [spwanPoolName];
		pool.DestroyAllSpwanPrefab ();
		spwanPoolDic.Remove (spwanPoolName);
	}

	public void DeSpwanAll()
	{
		foreach(var spwan in spwanPoolDic)
		{
			spwan.Value.DeSpwanAllPrefab();
		}
	}
}
