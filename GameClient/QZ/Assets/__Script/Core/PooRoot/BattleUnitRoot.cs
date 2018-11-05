using UnityEngine;
using System.Collections;

public class BattleUnitRoot :MonoBehaviour
{
	private string spwanPoolName = "BattleUnitRoot";

	private static BattleUnitRoot _instance = null;
    
	public static BattleUnitRoot instance 
	{
		get 
		{
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

	void Awake()
	{
		_instance = this;


	}

	void Start()
	{

	}


	public Transform SpwanPrefab (string prefabName)
	{
		Transform trans = spwanPool.SpwanPrefab (prefabName);

		return trans;
	}

	public Transform SpwanPrefab(string prefabName,float deSpwanTime)
	{
		Transform trans = spwanPool.SpwanPrefab (prefabName,deSpwanTime);

		return trans;
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
