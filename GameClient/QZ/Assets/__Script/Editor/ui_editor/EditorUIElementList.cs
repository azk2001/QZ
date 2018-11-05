using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIElementList))]
public class EditorUIElementList : Editor 
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

        UIElementList _target = target as UIElementList;

		if(GUILayout.Button("Print ElementList Enum"))
		{
			if(_target.elementList.Count >0)
			{

				string text = "private enum eElementIndex\n";
				text+="{\n"; 
				for(int i=0,max = _target.elementList.Count;i<max;i++)
				{
					text+="\t"+ _target.elementList[i].name+",\n";
				}

				text+="}";

				Debug.Log(text);
			}
		}
        
	}
}
