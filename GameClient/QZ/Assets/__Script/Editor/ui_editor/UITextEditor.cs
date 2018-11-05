using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

[CustomEditor(typeof(UIText), true)]
[CanEditMultipleObjects]
public class UITextEditor : GraphicEditor
{
    private UIText _target;
    SerializedProperty m_Text;
    SerializedProperty m_FontData;
    GUIContent[] txtStyles;
    private ContentSizeFitter sizeFitter;

    private int lastStyle = -1;

    protected override void OnEnable()
    {

        base.OnEnable();
        m_Text = serializedObject.FindProperty("m_Text");
        m_FontData = serializedObject.FindProperty("m_FontData");
        _target = (UIText)target;

    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        _target.txtId = EditorGUILayout.IntField("文本ID", _target.txtId);
        _target.styleId = EditorGUILayout.IntField("文本样式ID_textcolor表", _target.styleId);

        if (_target.styleId != lastStyle && Application.isPlaying == false)
        {
            _target.SetStyle(_target.styleId);
        }


        EditorGUILayout.PropertyField(m_Text);
        EditorGUILayout.PropertyField(m_FontData);
        AppearanceControlsGUI();

        _target.lineCount = EditorGUILayout.IntField("行最多显示几个汉字", _target.lineCount);
        _target.isLineCount = EditorGUILayout.Toggle("是否启用字数限制", _target.isLineCount);


        RaycastControlsGUI();
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        lastStyle = _target.styleId;

    }

}
