using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatePrefab 
{
    
    /// <summary>
    /// Creates the map prefab.
    /// </summary>
    /// <returns>The map prefab.</returns>
    /// <param name="resPath">Res path.</param>
    /// <param name="name">Name.</param>
    /// <param name="parent">Parent.</param>
    public static GameObject CreateMapPrefab(string resPath,string name, GameObject parent = null,string tag = "Untagged")
    {
        GameObject  prefab = Resources.Load(resPath+name, typeof(GameObject)) as GameObject;
		GameObject backmap = GameObject.Instantiate(prefab) as GameObject;
        backmap.name = name;
        backmap.tag = tag;
        if (parent != null)
        {
            AddParent(parent, backmap);
        }
        return backmap;
    }

	public static void AddParent(GameObject parent,GameObject children )
	{
		children.transform.parent = parent.transform;
		children.transform.localPosition = Vector3.zero;
		children.transform.localScale = Vector3.one;
		children.transform.localEulerAngles = Vector3.zero;
	}
}
