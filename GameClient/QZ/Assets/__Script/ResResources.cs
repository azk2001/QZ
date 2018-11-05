using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResResources
{
	public static GameObject AddEditorMapItem(string resPath,Transform transParent)
	{
		GameObject o = new GameObject ("地图原件");

		if (Application.isPlaying == false) {
			CapsuleCollider collider = o.GetComponent<CapsuleCollider> ();
			if (collider == null) {
				collider = o.AddComponent<CapsuleCollider> ();
			}
			collider.radius = 0.5f;
			collider.height = 4;
		}

		o.transform.parent = transParent;
		o.transform.localScale = Vector3.one;
		o.transform.localEulerAngles = Vector3.zero;

		return o;
	}

	public static GameObject AddEditorMapPlayer(Transform transParent)
	{
		GameObject o = new GameObject ("地图玩家");
		

		if (Application.isPlaying == false) 
		{
			GameObject g = GameObject.CreatePrimitive (PrimitiveType.Cube);
			g.transform.parent = o.transform;
			g.transform.localPosition = Vector3.zero;
            
		}

		return o;
	}

	public static GameObject LoadAssetRes(string path,Transform transParent)
	{
		Object o =  Resources.Load (path,typeof(GameObject));
		GameObject g = null;
		if(o != null)
		{
			g = GameObject.Instantiate(o) as GameObject;
			if(transParent!= null)
			{
				g.transform.parent = transParent;
				g.transform.localPosition = Vector3.zero;
				g.transform.localScale = Vector3.one;
				g.transform.localEulerAngles = Vector3.zero;
			}
		}
		return g;
	}

	public static void SetLayer(GameObject go, string layerName)
	{
        Debug.LogError( layerName);

        int layer = LayerMask.NameToLayer (layerName);

        Debug.LogError(layer + "   " + layerName);

		Transform[] layerTrans = go.GetComponentsInChildren<Transform> (true);

		//遍历当前物体及其所有子物体 
		for(int i=0,max = layerTrans.Length;i<max;i++)
		{ 
			layerTrans[i].gameObject.layer = layer;//更改物体的Layer层  
		}  

	}
}
