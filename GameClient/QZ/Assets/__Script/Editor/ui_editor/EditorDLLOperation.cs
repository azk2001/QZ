using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using DG.Tweening;
using UnityEngine.UI;

public class EditorDLLOperation : Editor
{
    private static BuildTargetGroup _buildTargetGroup = BuildTargetGroup.Android;
    public static BuildTargetGroup buildTargetGroup
    {
        get
        {
#if UNITY_ANDROID
			_buildTargetGroup = BuildTargetGroup.Android;
#elif UNITY_EDITOR
            _buildTargetGroup = BuildTargetGroup.Standalone;
#elif UNITY_IPHONE
			_buildTargetGroup = BuildTargetGroup.iPhone;
#endif
            return _buildTargetGroup;
        }
    }

    private static string dllPath = Application.dataPath + "/../GameMain/GameMain.dll";
    private static string dllUnityPath = Application.streamingAssetsPath + "/GameMain.dll";
    private static string projectPath = Application.dataPath + "/../../GameMain/GameMain/GameMain/";
    private static string projectUnityPath = Application.dataPath + "/___script/GameMain/";

    [MenuItem("Tools/MoveFile/DLLToUnity")]
    public static void DLLToUnity()
    {
        File.Copy(dllPath, dllUnityPath, true);
        AssetDatabase.Refresh();
    }

    //将版本移动到Unity
    [MenuItem("Tools/MoveFile/ProjectToUnity", true)]
    public static bool ProjectToUnityValidation() { return Directory.Exists(projectPath) && !Directory.Exists(projectUnityPath); }
    [MenuItem("Tools/MoveFile/ProjectToUnity", false)]
    private static void ProjectToUnity()
    {
        string Hong = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

        Hong = Hong.Replace("GameMain_DLL", "");

        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, Hong);

        MoveDir(projectPath, projectUnityPath);
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/MoveFile/UnityToProject", true)]
    public static bool UnityToProjectValidation() { return !Directory.Exists(projectPath) && Directory.Exists(projectUnityPath); }
    [MenuItem("Tools/MoveFile/UnityToProject", false)]
    private static void UnityToProject()
    {
        string Hong = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

        Hong = Hong + ";GameMain_DLL";

        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, Hong);

        MoveDir(projectUnityPath, projectPath);

        ListDirectory(projectPath);

        AssetDatabase.Refresh();
    }
    [MenuItem("Tools/MoveFile/DeleNeedlessDir", true)]
    public static bool DeleNeedlessDirValidation()
    {
        if (Directory.Exists(projectPath) && Directory.Exists(projectUnityPath))
        {
            return true;
        }
        return false;
    }
    [MenuItem("Tools/MoveFile/DeleNeedlessDir", false)]
    public static void DeleNeedlessDir()
    {
        bool atProjectPath = File.Exists(projectPath + "GameMain.cs");
        bool atUnityPath = File.Exists(projectUnityPath + "GameMain.cs");
        string tipStr = "";
        string delePath = "";
        if (atProjectPath)
        {
            tipStr = "gameMain源文件在project目录中,是否删除unity目录下的多余GameMain文件夹";
            delePath = projectUnityPath;
        }
        if (atUnityPath)
        {
            delePath = projectPath;
            tipStr = "gameMain源文件在unity目录中，是否删除project目录下的多余GameMain文件夹";
        }
        //都有GameMain.cs
        if (atUnityPath && atProjectPath)
        {
            UnityEditor.EditorUtility.DisplayDialog("请检查", "gameMain存在于两个文件夹，请手动检查删除", "无法自动删除");
            return;
        }
        if (UnityEditor.EditorUtility.DisplayDialog("确认", tipStr, "ok", "cancel"))
        {
            if (!Directory.Exists(delePath)) return;
            Directory.Delete(delePath, true);
        }
    }

    private static void ListDirectory(string path)
    {
        DirectoryInfo theFolder = new DirectoryInfo(@path);
        //遍历文件
        foreach (FileInfo nextFile in theFolder.GetFiles())
        {
            if (nextFile.Extension.Contains(".meta"))
            {
                File.Delete(nextFile.FullName);
            }

        }

        //遍历文件夹
        foreach (DirectoryInfo nextFolder in theFolder.GetDirectories())
        {
            ListDirectory(nextFolder.FullName);
        }
    }

    private static void MoveDir(string fromDir, string toDir)
    {
        if (!Directory.Exists(fromDir))
            return;

        CopyDir(fromDir, toDir);
        Directory.Delete(fromDir, true);
    }

    private static void CopyDir(string fromDir, string toDir)
    {
        if (!Directory.Exists(fromDir))
            return;

        if (!Directory.Exists(toDir))
        {
            Directory.CreateDirectory(toDir);
        }

        string[] files = Directory.GetFiles(fromDir);
        foreach (string formFileName in files)
        {
            string fileName = Path.GetFileName(formFileName);
            string toFileName = Path.Combine(toDir, fileName);
            File.Copy(formFileName, toFileName);
        }
        string[] fromDirs = Directory.GetDirectories(fromDir);
        foreach (string fromDirName in fromDirs)
        {
            string dirName = Path.GetFileName(fromDirName);
            string toDirName = Path.Combine(toDir, dirName);
            CopyDir(fromDirName, toDirName);
        }
    }

    [MenuItem("Assets/Utils/ClickEffect")]
    private static void SetButtonClickEffect()
    {
        if (EditorUtility.DisplayDialog("确认", "是否修改预设点击效果", "ok", "cancel"))
        {
            GameObject[] goArr = Selection.gameObjects;
            if (goArr != null)
            {
                for (int i = 0; i < goArr.Length; i++)
                {
                    GameObject go = goArr[i];
                    UIButton[] btns = go.GetComponentsInChildren<UIButton>(true);
                    for (int j = 0; j < btns.Length; j++)
                    {
                        btns[j].transition = Selectable.Transition.ColorTint;
                        Image targetImg = btns[j].GetComponent<Image>();
                        btns[j].targetGraphic = targetImg;
                    }
                    EditorUtility.SetDirty(go);
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

            }
        }
    }

    [MenuItem("Assets/Utils/ClickScaleEffect")]
    private static void SetButtonScaleEffect()
    {
        if (EditorUtility.DisplayDialog("确认", "是否修改预设点击效果", "ok", "cancel"))
        {
            GameObject[] goArr = Selection.gameObjects;
            if (goArr != null)
            {
                for (int i = 0; i < goArr.Length; i++)
                {
                    GameObject go = goArr[i];
                    UIButton[] btns = go.GetComponentsInChildren<UIButton>(true);
                    for (int j = 0; j < btns.Length; j++)
                    {
                        UIButton button = btns[i];
                        button.clickTweener = true;
                    }

                    EditorUtility.SetDirty(go);
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

            }
        }
    }


    [MenuItem("Assets/Utils/SetItemSprite")]
    public static void SetTextFont()
    {
        if (Selection.gameObjects != null)
        {
            GameObject[] goArr = Selection.gameObjects;
            int k = 0;
            bool haveChange = false;
            for (; k < goArr.Length; k++)
            {
                GameObject go = PrefabUtility.InstantiatePrefab(goArr[k] as GameObject) as GameObject;

                if (go != null)
                {
                    //D:\GameClient\ClientBase\Assets\_arts\ui_atlas
                    SpriteAsset iconAtlas = AssetDatabase.LoadAssetAtPath<SpriteAsset>(@"Assets\_arts\ui_atlas\" + "spr_icon.asset");
                    UIGeneralItem[] itemArr = go.transform.GetComponentsInChildren<UIGeneralItem>(true);
                    for (int i = 0; i < itemArr.Length; i++)
                    {
                        //背景
                        var backGround = itemArr[i].backGround;
                        if (backGround != null)
                        {
                            backGround.spriteAtlas = iconAtlas;
                            backGround.spriteName = "img_pinzikuang_jichu";
                            if (haveChange == false)
                            {
                                haveChange = true;
                            }
                        }

                        //绿点
                        var greenImg = itemArr[i].greenImg;
                        if (greenImg != null)
                        {
                            greenImg.spriteAtlas = iconAtlas;
                            greenImg.spriteName = "img_lvdian";
                            if (haveChange == false)
                            {
                                haveChange = true;
                            }
                        }
                    }
                }
                if (haveChange == false)
                {
                    DestroyImmediate(go);
                    Debug.Log("未做修改");
                    break;

                }
                //替换保存  
                PrefabUtility.ReplacePrefab(go, goArr[k], ReplacePrefabOptions.ConnectToPrefab);
                DestroyImmediate(go);
                AssetDatabase.Refresh();
                Debug.Log("图集修改完成");
            }
            Debug.Log("图集修改完成,共：" + k);
        }
    }

}
