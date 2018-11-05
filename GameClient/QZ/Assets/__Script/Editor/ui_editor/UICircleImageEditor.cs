using System.Linq;
using UnityEngine;
using UnityEditor.AnimatedValues;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;
using UnityEngine.U2D;

/// <summary>
/// Editor class used to edit UI Sprites.
/// </summary>

[CustomEditor(typeof(CircleImage), true)]
[CanEditMultipleObjects]
public class UICircleImageEditor : Editor
{
    private CircleImage _target = null;

    private SerializedObject serialized = null;

    public void OnEnable()
    {
        _target = target as CircleImage;

        serialized = new SerializedObject(_target);
        
    }

    public override void OnInspectorGUI()
    {
     
        EditorGUILayout.BeginHorizontal();
        _target.spriteAtlas = (SpriteAsset)EditorGUILayout.ObjectField("图集名字:", _target.spriteAtlas, typeof(SpriteAsset), true);

        if (_target.spriteAtlas!=null)
        {
            if (GUILayout.Button("查找图片"))
            {
                SpriteSelector comp = ScriptableWizard.DisplayWizard<SpriteSelector>("Select a Sprite");
                comp.mCallback = ClickImageEvent;
                comp.spriteAsset = _target.spriteAtlas;
            }
        }

        EditorGUILayout.EndHorizontal();

        

        base.OnInspectorGUI();

        //一定要添加这个函数，不然修关闭项目后数据会丢失
        serialized.ApplyModifiedProperties();

        return;

        //_target.fillAmount = EditorGUILayout.FloatField("圆形或扇形填充比例:", _target.fillAmount);
        //_target.fill = EditorGUILayout.Toggle("是否填充圆形:", _target.fill);
        //_target.thickness = EditorGUILayout.FloatField("圆环宽度:", _target.thickness);
        //_target.segements = EditorGUILayout.IntField("圆形:", _target.segements);
        //_target.fillPercent = EditorGUILayout.FloatField("图集名字:", _target.fillPercent);


    }

    private void ClickImageEvent(string spriteName)
    {
        _target.sprite = _target.spriteAtlas.GetSprite(spriteName);
      //  _target.SetNativeSize();
    }
}
