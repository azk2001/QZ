using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIGameObjectList))]
public class EditorUIGameObjectList : Editor 
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		UIGameObjectList _target = target as UIGameObjectList;

		if(GUILayout.Button("Print ObjectList Enum"))
		{
			if(_target.gameObjectList.Count >0)
			{

				string text = "private enum eObjectIndex\n";
				text+="{\n";
				for(int i=0,max = _target.gameObjectList.Count;i<max;i++)
				{
					text+="\t"+ _target.gameObjectList[i].name+",\n";
				}

				text+="}";

				Debug.Log(text);
			}
		}

		if(GUILayout.Button("Print UILableList Enum"))
		{
			if(_target.uiLableList.Count >0)
			{
				
				string text = "private enum eUILableIndex\n";
				text+="{\n";
				for(int i=0,max = _target.uiLableList.Count;i<max;i++)
				{
					text+="\t"+ _target.uiLableList[i].name+",\n";
				}
				
				text+="}";
				
				Debug.Log(text);
			}
		}

        if (GUILayout.Button("Format Position"))
        {
            RectTransform[] rectList=_target.GetComponentsInChildren<RectTransform>(true);
            for (int i = 0, max = rectList.Length; i < max; i++)
            {
                RectTransform rect = rectList[i];
                rect.anchoredPosition = new Vector2(Mathf.RoundToInt(rect.anchoredPosition.x), Mathf.RoundToInt(rect.anchoredPosition.y));
                rect.sizeDelta = new Vector2(Mathf.RoundToInt(rect.sizeDelta.x), Mathf.RoundToInt(rect.sizeDelta.y));
                rect.localScale = Vector3.one;
            }
        }
	}
}
