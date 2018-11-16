using UnityEditor;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(EditorManager))]
public class EditorMapManager : Editor {
	
	public override void OnInspectorGUI() {
		EditorManager _target = (EditorManager)target;

		_target.elementPoints = EditorGUILayout.ObjectField("原件组", _target.elementPoints, typeof(GameObject), true) as GameObject;

	}
}
