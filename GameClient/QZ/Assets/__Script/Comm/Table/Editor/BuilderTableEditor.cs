using UnityEngine;
using System.Collections;
using UnityEditor;

public class BuilderTableEditor : MonoBehaviour
{
    public static bool TableToCS(bool isAuto, bool isDic, bool isDeserialize)
    {
        //选择文件
        string savepath = SelectPath();

        if (string.IsNullOrEmpty(savepath))
            return false;

        Object[] objs = Selection.objects;
        for (int i = 0; i < objs.Length; ++i)
        {
            string path = AssetDatabase.GetAssetPath(objs[i]);

            //修改为绝对路径
            string datapath = Application.dataPath;
            datapath = datapath.Replace("Assets", string.Empty);

            TableBuilder.BuilderFileUTF8(datapath + path, savepath, isAuto, isDic, isDeserialize, "MXGame");
        }

        return true;
    }

    public static bool TableToCSBool()
    {
        object[] objs = Selection.objects;

        for (int i = 0; i < objs.Length; ++i)
        {
            string path = AssetDatabase.GetAssetPath((UnityEngine.Object)objs[i]);
            if (!path.EndsWith(".txt"))
                return false;
        }

        return true;
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
}