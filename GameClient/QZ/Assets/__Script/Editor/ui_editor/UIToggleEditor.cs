﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

[CustomEditor(typeof(UIToggle), true)]
[CanEditMultipleObjects]
public class UIToggleEditor : SelectableEditor
{
    SerializedProperty m_OnValueChangedProperty;
    SerializedProperty m_TransitionProperty;
    SerializedProperty m_GraphicProperty;
    SerializedProperty m_GroupProperty;
    SerializedProperty m_IsOnProperty;

    protected override void OnEnable()
    {
        base.OnEnable();

        m_TransitionProperty = serializedObject.FindProperty("toggleTransition");
        m_GraphicProperty = serializedObject.FindProperty("graphic");
        m_GroupProperty = serializedObject.FindProperty("m_Group");
        m_IsOnProperty = serializedObject.FindProperty("m_IsOn");
        m_OnValueChangedProperty = serializedObject.FindProperty("onValueChanged");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.PropertyField(m_IsOnProperty);
        EditorGUILayout.PropertyField(m_TransitionProperty);
        EditorGUILayout.PropertyField(m_GraphicProperty);
        EditorGUILayout.PropertyField(m_GroupProperty);

        EditorGUILayout.Space();

        UIToggle _target = target as UIToggle;

        // Draw the event notification options
        EditorGUILayout.PropertyField(m_OnValueChangedProperty);

        serializedObject.ApplyModifiedProperties();
    }
}