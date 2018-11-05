using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpwanPool : MonoBehaviour {

	public string spwanPoolName = "";
	public string layerName = "";

	public Dictionary<string,PoolPrefab> poolPrefabDic = new Dictionary<string,PoolPrefab>();

	private Dictionary<Transform,string> spwanPrefabName = new Dictionary<Transform, string>();

	public List<GameObject> startSpwanList = new List<GameObject>();
	public List<int> startSpwanCount = new List<int>();
	void Awake ()
	{
		for (int i=0,max = startSpwanList.Count; i<max; i++) 
		{
			AddPoolPrefab(startSpwanList[i].transform,startSpwanCount[i]);
			startSpwanList[i].SetActive(false);
		}

		PoolManager.instance.AddSpwanPool (spwanPoolName,this);
	}

	public Transform GetSpwanPrefab(string prefabName)
	{
		for (int i=0,max = startSpwanList.Count; i<max; i++) {
			if(startSpwanList[i].name.Equals(prefabName))
			{
				return startSpwanList[i].transform;
			}
		}
		return null;
	}

	public void AddPoolPrefab(Transform basics,int spwanCount)
	{
		string spwanName = basics.name;
		PoolPrefab pp = null;
		if (poolPrefabDic.ContainsKey (spwanName) == true)
		{
			pp =poolPrefabDic[spwanName];
			pp.AddPwanPrefab(basics);

			return;
		}

		pp = new PoolPrefab ();
		pp.AddPoolPrefab (basics,this.transform,spwanCount,layerName);

		poolPrefabDic [spwanName] = pp;
	}

	public Transform SpwanPrefab (string prefabName )
	{
		if (poolPrefabDic.ContainsKey (prefabName) == false)
		{
			Debug.LogError(spwanPoolName+" 里面没有找到名字："+prefabName);
			return null;
		}

		Transform t = poolPrefabDic [prefabName].SpwanPrefab ();
		spwanPrefabName [t] = prefabName;
		t.gameObject.SetActive (true);
		return t;
	}

	public Transform SpwanPrefab (string prefabName ,float deSpwanTime)
	{
		if (poolPrefabDic.ContainsKey (prefabName) == false)
		{
			Debug.LogError(spwanPoolName+" 里面没有找到名字："+prefabName);
			return null;
		}
		
		Transform t = poolPrefabDic [prefabName].SpwanPrefab ();
		spwanPrefabName [t] = prefabName;

		StartCoroutine (DeSpwanPrefabTime (t,deSpwanTime));
		t.gameObject.SetActive (true);
		return t;
	}

	private IEnumerator DeSpwanPrefabTime(Transform trans,float deSpwanTime)
	{
		yield return new WaitForSeconds (deSpwanTime);
		DeSpwanPrefab (trans);
	}

	public void DeSpwanPrefab (Transform trans)
	{
		if (spwanPrefabName.ContainsKey (trans) == false)
		{
			Debug.LogError(spwanPoolName+" 里面没有找到名字："+trans);
			return;
		}

		string prefabName = spwanPrefabName [trans];
		poolPrefabDic [prefabName].DeSpwanPrefab (trans);
		trans.parent =this.transform;
		trans.localScale = Vector3.one;

		if (layerName.Equals ("") == false) {
			ResResources.SetLayer(trans.gameObject,layerName);
		}

		spwanPrefabName.Remove (trans);

	}

	public void DeSpwanAllPrefab ()
	{
		foreach (var spwan in poolPrefabDic) {
			spwan.Value.DeSpwanAllPrefab();
		}

		spwanPrefabName.Clear ();
	}


	public void DestroyAllSpwanPrefab()
	{
		foreach (string prefabName in poolPrefabDic.Keys)
		{
			DestroySpwanPrefab(prefabName);
		}
		GameObject.Destroy (this.gameObject);
	}

	public void DestroySpwanPrefab(string prefabName)
	{
		if (poolPrefabDic.ContainsKey (prefabName) == false)
		{
			Debug.LogError(spwanPoolName+" 里面没有找到名字："+prefabName);
			return;
		}

		poolPrefabDic[prefabName].DestroyAll ();
		poolPrefabDic.Remove (prefabName);
	}
}
