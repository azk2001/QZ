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

[CustomEditor(typeof(UIImage), true)]
[CanEditMultipleObjects]
public class UIImageEditor : ImageEditor
{
    UIImage _target = null;

    public override void OnInspectorGUI()
    {
        _target = target as UIImage;

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

        serializedObject.ApplyModifiedProperties();
    }

    private void ClickImageEvent(string spriteName)
    {
        _target.sprite = _target.spriteAtlas.GetSprite(spriteName);
      //  _target.SetNativeSize();
    }
}
