using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;
[CustomEditor(typeof(UIScrollView))]
public class UIScrollViewEditor : ScrollRectEditor
{
    private SerializedProperty _needTips;
    private SerializedProperty _topOrLeft;
    private SerializedProperty _downOrRight;
    private SerializedProperty _Horizontal;
    private SerializedProperty _Vertical;
    private SerializedProperty _select;
    //private SerializedProperty _Content;
    private ScrollRect _scrollRect;
    protected override void OnEnable()
    {
        base.OnEnable();
        _needTips = serializedObject.FindProperty("needTips");
        _topOrLeft = serializedObject.FindProperty("topOrLeft");
        _downOrRight = serializedObject.FindProperty("downOrRight");
        _Horizontal = serializedObject.FindProperty("m_Horizontal");
        _Vertical = serializedObject.FindProperty("m_Vertical");
        _select = serializedObject.FindProperty("selectFrame");
        //_Content = serializedObject.FindProperty("m_Content");
        _scrollRect = (ScrollRect)target;
    }
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(_needTips);
        EditorGUILayout.PropertyField(_topOrLeft);
        EditorGUILayout.PropertyField(_downOrRight);
        EditorGUILayout.PropertyField(_select);
        if (_needTips.boolValue)
        {
            Vector2 _vector2 = _Vertical.boolValue ? new Vector2(0.5f, 1f) : new Vector2(0f, 0.5f);
            _scrollRect.content.anchorMin = _vector2;
            _scrollRect.content.anchorMax = _vector2;
            _scrollRect.content.pivot = _vector2;
            _scrollRect.content.anchoredPosition = Vector2.zero;
        }
        serializedObject.ApplyModifiedProperties();

    }
}
