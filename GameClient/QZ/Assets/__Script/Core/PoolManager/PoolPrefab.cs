using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolPrefab
{

	public Transform prefabParent = null;

	public int startPrefabCount = 5;

	public Transform basicsPrefab = null;

	public List<Transform> prefabList = new List<Transform>();		//总预设;
	private List<Transform> spwanList = new List<Transform>();		//已经使用了的预设;
	private List<Transform> deSpwanList = new List<Transform> ();  //已经回收了的预设;
	private string mLayerName = "";
	public void AddPoolPrefab(Transform basics,Transform transParent,int spwanCount = 5,string layerName = "")
	{
		startPrefabCount = spwanCount;
		basicsPrefab = basics;
		prefabParent = transParent;
		mLayerName = layerName;

		for (int i=0,max = startPrefabCount; i<max; i++) 
		{
			Transform trans = NewSpwanPrefab(basics.gameObject,mLayerName);
			prefabList.Add(trans);
			deSpwanList.Add(trans);
		}
	}

	public void AddPwanPrefab(Transform trans)
	{
		prefabList.Add(trans);
		deSpwanList.Add(trans);
	}

	public Transform SpwanPrefab()
	{
		Transform trans = null;
		if (deSpwanList.Count < 1)
		{
			trans = NewSpwanPrefab(basicsPrefab.gameObject,mLayerName);
			prefabList.Add(trans);
			spwanList.Add(trans);
			return trans;
		}

		for (int i = deSpwanList.Count-1; i>=0; i--)
		{
			trans = deSpwanList[i];

			spwanList.Add(trans);
			deSpwanList.Remove(trans);

			return trans;
		}
		return trans;
	}

	public bool DeSpwanPrefab(Transform trans)
	{
		spwanList.Remove (trans);
		deSpwanList.Add(trans);
		trans.gameObject.SetActive (false);
		trans.parent = prefabParent;
		return true;
	}

	public void DeSpwanAllPrefab()
	{
		for (int i=spwanList.Count-1; i>=0; i--) 
		{
			Transform trans = spwanList[i];
			spwanList.Remove (trans);
			deSpwanList.Add(trans);
			trans.gameObject.SetActive (false);
			trans.parent = prefabParent;
		}
	}

	public void DestroyAll()
	{
		for (int i=0,max=prefabList.Count; i<max; i++) 
		{
			if(prefabList[i]!=null)
			{
				GameObject.Destroy (prefabList[i].gameObject);
			}
		}
		GameObject.Destroy (basicsPrefab.gameObject);
	}

	private Transform NewSpwanPrefab(GameObject basics,string layerName)
	{
		GameObject go = GameObject.Instantiate(basics) as GameObject;
		go.transform.parent = prefabParent;
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = Vector3.zero;

		go.SetActive (false);
		if(layerName.Equals("")==false)
		{
			ResResources.SetLayer(go,layerName);
		}
		return go.transform;
	}
}
