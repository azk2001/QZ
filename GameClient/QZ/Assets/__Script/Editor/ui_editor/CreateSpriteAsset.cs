using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public static class CreateSpriteAsset
{
    [MenuItem("Assets/Create/Sprite Asset", false, 10)]
    static void main()
    {
        Object[] target = Selection.objects;
        for (int i = 0; i < target.Length; i++)
        {
            if (target[i].GetType() != typeof(Texture2D))
                return;
        }

        //整体路径
        string filePathWithName = AssetDatabase.GetAssetPath(target[0]);
        //带后缀的文件名
        string fileNameWithExtension = Path.GetFileName(filePathWithName);
        //不带后缀的文件名
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePathWithName);
        //不带文件名的路径
        string filePath = filePathWithName.Replace(fileNameWithExtension, "");

        string savePath = filePath + fileNameWithoutExtension + ".asset";
        
        SpriteAsset spriteAsset = AssetDatabase.LoadAssetAtPath(savePath, typeof(SpriteAsset)) as SpriteAsset;
        bool isNewAsset = spriteAsset == null ? true : false;
        if (isNewAsset)
        {
            spriteAsset = ScriptableObject.CreateInstance<SpriteAsset>();

            spriteAsset.spriteList = GetAssetSpriteInfor(target);
            spriteAsset.spriteNum = spriteAsset.spriteList.Count;

            AssetDatabase.CreateAsset(spriteAsset, savePath);
            AssetDatabase.SaveAssets();
        }
    }

    public static List<SpriteInfor> GetAssetSpriteInfor(Object[] objs)
    {
        List<SpriteInfor> dic = new List<SpriteInfor>();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].GetType() != typeof(Texture2D))
                continue;

            string filePath = UnityEditor.AssetDatabase.GetAssetPath(objs[i]);

            Object[] objects = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(filePath);

            List<SpriteInfor> _tempSprite = new List<SpriteInfor>();

            for (int j = 0; j < objects.Length; j++)
            {
                if (objects[j].GetType() != typeof(Sprite))
                    continue;

                SpriteInfor temp = new SpriteInfor();
                Sprite sprite = objects[j] as Sprite;
                temp.name = sprite.name;
                temp.sprite = sprite;
                temp.texture = (Texture2D)objs[i];
                dic.Add(temp);
            }
        }
        return dic;
    }

    public static string GetAtlasText(List<SpriteInfor> list)
    {
        string str = "";

        for (int i = 0; i < list.Count; i++)
        {
            string filePathWithName = AssetDatabase.GetAssetPath(list[i].texture);

            str += list[i].name + "|" + filePathWithName + "\t";

        }


        return str;
    }
}
