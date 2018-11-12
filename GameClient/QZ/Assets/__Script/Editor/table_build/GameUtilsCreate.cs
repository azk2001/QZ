using UnityEngine;
using System.Collections;
using UnityEditor;

public static class GameUtilsCreate
{
    //配置表生成-------------------------------------------------------------------------------------------
    [MenuItem("Assets/Utils/CreateTable/Create")]
    static void TableToDicCS()
    {

        //选择文件
        string savepath = SelectPath();

        if (string.IsNullOrEmpty(savepath))
            return;

        Object[] objs = Selection.objects;
        for (int i = 0; i < objs.Length; ++i)
        {
            string path = AssetDatabase.GetAssetPath(objs[i]);

            //修改为绝对路径
            string datapath = Application.dataPath;
            datapath = datapath.Replace("Assets", string.Empty);

            TableBuilder.BuilderFileUTF8(datapath + path, savepath, "GameMain");
        }

        Debug.Log("配置表类创建完毕");

        AssetDatabase.Refresh();
    }


    //选取某路径进行保存
    static string SelectPath()//默认保存的路径
    {
        try
        {
            return EditorUtility.OpenFolderPanel("请选取一个存储路径", null, null);
        }
        catch
        {
            Debug.Log("选取取消");
            return string.Empty;
        }
    }

    //配置表生成-------------------------------------------------------------------------------------------
    [MenuItem("Assets/Utils/CreateTable/Create", true)]
    static public bool OnCheckTxt()
    {
        Object[] objs = Selection.objects;
        if (objs == null || objs.Length == 0)
            return false;

        for (int i = 0; i < objs.Length; ++i)
        {
            string path = AssetDatabase.GetAssetPath(objs[i]);
            if (!path.EndsWith(".txt"))
                return false;
        }
        return true;
    }
}


