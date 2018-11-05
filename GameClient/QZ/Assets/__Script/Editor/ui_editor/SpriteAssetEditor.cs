using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(SpriteAsset))]
public class SpriteAssetEditor : Editor
{
    private SpriteAsset spriteAsset = null;

    private SerializedObject serialized = null;

    public void OnEnable()
    {
        spriteAsset = (SpriteAsset)target;

        serialized = new SerializedObject(spriteAsset);

        CheckTexture();
    }



    private void CheckTexture()
    {
        for (int i = spriteAsset.spriteList.Count - 1; i >= 0; i--)
        {
            if (spriteAsset.spriteList[i].texture == null)
            {
                spriteAsset.spriteList.RemoveAt(i);
            }
        }

        //一定要添加这个函数，不然修关闭项目后无法保存
        EditorUtility.SetDirty(spriteAsset);

        //一定要添加这个函数，不然修关闭项目后数据会丢失
        serialized.ApplyModifiedProperties();
    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.IntField("图片个数：", spriteAsset.spriteList.Count);
        
        foreach (var item in spriteAsset.spriteList)
        {
            GUILayout.BeginHorizontal("HelpBox");

            EditorGUILayout.Space();
            EditorGUILayout.ObjectField(item.name, item.texture, typeof(Texture2D), false);

            EditorGUILayout.Space();
            if (GUILayout.Button("Remove"))
            {
                spriteAsset.RemoveSprite(item.name);

                //一定要添加这个函数，不然修关闭项目后无法保存
                EditorUtility.SetDirty(spriteAsset);

                //一定要添加这个函数，不然修关闭项目后数据会丢失
                serialized.ApplyModifiedProperties();

                break;
            }

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add"))
        {
            AddSpriteList spriteList = EditorWindow.CreateInstance<AddSpriteList>();
            spriteList.OnClickEvent = OnClickEvent;
            spriteList.Show();

        }

    }

    private void OnClickEvent(List<Sprite> obj)
    {
        for (int i = 0; i < obj.Count; i++)
        {
            if (obj != null)
            {
                SpriteInfor spriteInfor = new SpriteInfor();
                spriteInfor.name = obj[i].name;
                spriteInfor.sprite = obj[i];
                spriteInfor.texture = obj[i].texture;
                spriteAsset.AddSprite(spriteInfor);
                
            }
        }

        spriteAsset.spriteNum = spriteAsset.spriteList.Count;

        //一定要添加这个函数，不然修关闭项目后无法保存
        EditorUtility.SetDirty(spriteAsset);

        //一定要添加这个函数，不然修关闭项目后数据会丢失
        serialized.ApplyModifiedProperties();

        AssetDatabase.SaveAssets();
    }
}

public class AddSpriteList : EditorWindow
{

    [SerializeField]//必须要加
    protected List<Sprite> _assetLst = new List<Sprite>();

    //序列化对象
    protected SerializedObject _serializedObject;

    //序列化属性
    protected SerializedProperty _assetLstProperty;

    public System.Action<List<Sprite>> OnClickEvent = null;
    private Vector2 scrollPos;

    protected void OnEnable()
    {
        //使用当前类初始化
        _serializedObject = new SerializedObject(this);
        //获取当前类中可序列话的属性
        _assetLstProperty = _serializedObject.FindProperty("_assetLst");
    }

    protected void OnGUI()
    {
        //更新
        _serializedObject.Update();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        //开始检查是否有修改
        EditorGUI.BeginChangeCheck();

        //显示属性
        //第二个参数必须为true，否则无法显示子节点即List内容
        EditorGUILayout.PropertyField(_assetLstProperty, true);

        if (GUILayout.Button("Add"))
        {
            if (OnClickEvent != null)
            {
                OnClickEvent(_assetLst);
            }

        }

        //结束检查是否有修改
        if (EditorGUI.EndChangeCheck())
        {//提交修改
            _serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.EndScrollView();
    }
}