using DG.Tweening;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(UIButton), true)]
[CanEditMultipleObjects]
public class UIButtonEditor : SelectableEditor
{
    SerializedProperty m_OnClickProperty;
    private Text _btnText;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_OnClickProperty = serializedObject.FindProperty("m_OnClick");
        // _btnText = ((UIButton)target).GetComponentInChildren<Text>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        UIButton _target = target as UIButton;

        if (_target != null)
        {
            _target.uiWrapper = (UIWrapper)EditorGUILayout.ObjectField("uiWrapper", _target.uiWrapper, typeof(UIWrapper), true);
            _target.btnName = (Text)EditorGUILayout.ObjectField("btnName", _target.btnName, typeof(Text), true);
            _target.msgParam = EditorGUILayout.IntField("msgParmm", _target.msgParam);

            _target.clickTweener = EditorGUILayout.Toggle("添加统一按键效果", _target.clickTweener);
        }

       
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_OnClickProperty);
        serializedObject.ApplyModifiedProperties();
    }
}
