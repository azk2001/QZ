using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(UISlider), true)]
[CanEditMultipleObjects]
public class SliderEditor : SelectableEditor
{
    SerializedProperty m_Direction;
    SerializedProperty m_FillRect;
    SerializedProperty m_HandleRect;
    SerializedProperty m_MinValue;
    SerializedProperty m_MaxValue;
    SerializedProperty m_WholeNumbers;
    SerializedProperty m_Value;
    SerializedProperty dataValue;
    SerializedProperty m_OnValueChanged;

    SerializedProperty m_imageBackEffect;
    SerializedProperty m_imageNext;
    SerializedProperty fillImageList;
    SerializedProperty sliderNum;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_FillRect = serializedObject.FindProperty("m_FillRect");
        m_HandleRect = serializedObject.FindProperty("m_HandleRect");
        m_Direction = serializedObject.FindProperty("m_Direction");
        m_MinValue = serializedObject.FindProperty("m_MinValue");
        m_MaxValue = serializedObject.FindProperty("m_MaxValue");
        m_WholeNumbers = serializedObject.FindProperty("m_WholeNumbers");

        m_Value = serializedObject.FindProperty("m_Value");
        m_OnValueChanged = serializedObject.FindProperty("m_OnValueChanged");

        m_imageBackEffect = serializedObject.FindProperty("imageBackEffect");
        m_imageNext = serializedObject.FindProperty("nextImage");

        fillImageList = serializedObject.FindProperty("fillImageList");
        sliderNum = serializedObject.FindProperty("_sliderNum");
        dataValue = serializedObject.FindProperty("_dataValue");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        serializedObject.Update();

        EditorGUILayout.PropertyField(m_FillRect);
        EditorGUILayout.PropertyField(m_HandleRect);

        UISlider _target = target as UISlider;

        if (m_FillRect.objectReferenceValue != null || m_HandleRect.objectReferenceValue != null)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_Direction);
            if (EditorGUI.EndChangeCheck())
            {
                Slider.Direction direction = (Slider.Direction)m_Direction.enumValueIndex;
                foreach (var obj in serializedObject.targetObjects)
                {
                    Slider slider = obj as Slider;
                    slider.SetDirection(direction, true);
                }
            }

            EditorGUILayout.PropertyField(m_MinValue);
            EditorGUILayout.PropertyField(m_MaxValue);
            EditorGUILayout.PropertyField(m_WholeNumbers);
            //  EditorGUILayout.PropertyField(dataValue);

            EditorGUILayout.Slider(dataValue, m_MinValue.floatValue, m_MaxValue.floatValue);

             EditorGUILayout.Slider(m_Value, m_MinValue.floatValue, m_MaxValue.floatValue);

            bool warning = false;
            foreach (var obj in serializedObject.targetObjects)
            {
                Slider slider = obj as Slider;
                Slider.Direction dir = slider.direction;
                if (dir == Slider.Direction.LeftToRight || dir == Slider.Direction.RightToLeft)
                    warning = (slider.navigation.mode != Navigation.Mode.Automatic && (slider.FindSelectableOnLeft() != null || slider.FindSelectableOnRight() != null));
                else
                    warning = (slider.navigation.mode != Navigation.Mode.Automatic && (slider.FindSelectableOnDown() != null || slider.FindSelectableOnUp() != null));
            }

            if (warning)
                EditorGUILayout.HelpBox("The selected slider direction conflicts with navigation. Not all navigation options may work.", MessageType.Warning);
            //自定义代码--------------------------------------------------
            EditorGUILayout.PropertyField(m_imageBackEffect);

            EditorGUILayout.PropertyField(m_imageNext);

            EditorGUILayout.PropertyField(sliderNum);

            if (_target.sliderNum < 1) _target.sliderNum = 1;

            EditorGUILayout.PropertyField(fillImageList, true);
            //自定义代码--------------------------------------------------

            // Draw the event notification options
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_OnValueChanged);
        }
        else
        {
            EditorGUILayout.HelpBox("Specify a RectTransform for the slider fill or the slider handle or both. Each must have a parent RectTransform that it can slide within.", MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
