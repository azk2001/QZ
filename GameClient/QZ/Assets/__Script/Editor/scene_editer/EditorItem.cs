using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class EditorItem : EditorWindow
{


    private static bool isSave = false;
    private static bool isLoad = false;
    private static bool isClear = false;
    private static string xmlPath
    {
        get
        {
            return Application.dataPath + "/___Prefab/Config/Resources/MapConfig/";
        }
    }
    public static string xmlName = "";
    private static string sceneName = "";

    [MenuItem("MapEditor/关卡编辑器")]
    public static void CreateEditor()
    {
        EditorItem window = (EditorItem)EditorWindow.GetWindow(typeof(EditorItem), false, "关卡编辑器");
        window.Show();
    }

    //[MenuItem("Tool/一建替换图片")]
    //public static void CreateEditor1()
    //{
    //    Object[] objectList =Selection.objects;

    //    for (int i = 0; i < objectList.Length; i++)
    //    {
    //        GameObject game = objectList[i] as GameObject;

    //        Image[] imageList = game.GetComponentsInChildren<Image>(true);

    //        for (int j = 0; j < imageList.Length; j++)
    //        {
    //            GameObject go = imageList[j].gameObject;
    //            UIImage uiimage = go.GetComponent<UIImage>();
    //            uiimage.spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>("Assets/_arts/ui_atlas/spr_common.spriteatlas");
    //            uiimage.SetSpriteName("ui_common_frame_neikuang");
    //        }
    //    }

    //}


    void OnGUI()
    {
        GUILayout.Label("打开场景：只能打开路径\"Assets/_arts/Scene/？/Export/？.unity\"");

        GUILayout.BeginHorizontal();

        GUILayout.Label("打开场景");
        sceneName = GUILayout.TextField(sceneName, 1000);

        if (GUILayout.Button("打开场景"))
        {
            EditorSceneManager.OpenScene("Assets/_arts/Scene/" + sceneName + "/Export/" + sceneName + ".unity");
        }

        GUILayout.EndHorizontal();

        GUILayout.Label("刷挂器名字:(保存/加载)(XML)");
        xmlName = GUILayout.TextField(xmlName, 1000);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("保存关卡"))
        {
            isSave = true;
        }

        if (GUILayout.Button("加载关卡"))
        {
            isLoad = true;
        }

        if (GUILayout.Button("清空场景"))
        {
            isClear = true;
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("创建元件组"))
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
            g.transform.parent = EditorManager.GetInstance().elementPoints.transform;
            g.name = "wave_0";
            g.AddComponent<ElementWave>();

            Selection.activeGameObject = g;
        }
        GUILayout.Label("想要运行最新的配置文件必须： 找到 _config/map_config 文件夹，选中点击鼠标右键 bilde_selected_floder");
        if (isSave == true)
        {
            SaveMap();
        }

        if (isLoad == true)
        {
            LoadMap();
        }

        if (isClear == true)
        {
            ClearSence();
        }
    }
    private static bool SaveMap()
    {
        bool isSaveed = false;
        if (xmlName.Equals(""))
        {
            GUILayout.Label("关卡名字不能为空");
            isSave = false;
        }
        else
        {
            string tmpxmlPath = xmlPath + xmlName + ".xml";
            if (System.IO.File.Exists(tmpxmlPath))
            {
                GUILayout.Label("已经有你当前要保存的关卡了，是否替换以前关卡");
                if (GUILayout.Button("替换"))
                {
                    isSave = false;
                    isSaveed = LoadSaveMap.SaveMap(tmpxmlPath, xmlName);
                }
                if (GUILayout.Button("返回"))
                {
                    isSave = false;
                    isSaveed = false;
                }
            }
            else
            {
                isSaveed = LoadSaveMap.SaveMap(tmpxmlPath, xmlName);
                isSave = false;
            }
        }
        if (isSaveed)
        {
            Debug.LogError(System.DateTime.Now + "--------" + xmlName + " 保存成功--------");
            AssetDatabase.Refresh();
        }

        return isSaveed;
    }
    private static void LoadMap()
    {
        if (xmlName.Equals(""))
        {
            GUILayout.Label("XML名字不能为空");
            isLoad = false;
        }
        else
        {
            string tmpxmlPath = xmlPath + xmlName + ".xml";
            if (System.IO.File.Exists(tmpxmlPath) == false)
            {
                GUILayout.Label("没有找到你的关卡请查看：" + tmpxmlPath);
            }
            else
            {
                LoadSaveMap.LoadMap(tmpxmlPath, false);

                EditorElementParam._elementIndex = LoadSaveMap.elementCount + 1;

                isLoad = false;
            }
        }
    }

    private static void ClearSence()
    {
        GameObject element = GameObject.FindGameObjectWithTag("Element");
        if (element == null)
        {
            Debug.LogError("没有找到Tag = Obstructions,请参考___res_pool/_ScenesPool/ScenesTemp_Arts/level_template 场景");
        }

        if ((element != null && element.transform.childCount > 0))
        {
            GUILayout.Label("当前场景还有你配置的信息，是否要清空？");

            if (GUILayout.Button("清空"))
            {
                for (int i = element.transform.childCount - 1; i >= 0; i--)
                {
                    GameObject.DestroyImmediate(element.transform.GetChild(i).gameObject);
                }
                isClear = false;
            }
            if (GUILayout.Button("返回"))
            {
                isClear = false;
            }
        }
        else
        {
            isClear = false;
        }
    }


}
