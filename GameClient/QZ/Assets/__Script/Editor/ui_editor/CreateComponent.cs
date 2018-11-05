using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CreateComponent : Editor
{
    [MenuItem("GameObject/UI/Image _#&_s")]
    public static void CreateNewImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Img_");
                go.AddComponent<UIImage>().raycastTarget = false;
                SetParent(go);
                Undo.RegisterCreatedObjectUndo(go, "createImg" + go.name);
            }
        }
    }
    [MenuItem("GameObject/UI/Text _#&_l")]
    public static void CreateNewText()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                SetParent(InitText().gameObject);
            }

        }
    }
    private static Text InitText()
    {
        GameObject go = new GameObject("Txt_");
        Text txt = go.AddComponent<UIText>();
        ContentSizeFitter fit = go.AddComponent<ContentSizeFitter>();
        fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fit.enabled = true;
        txt.color = Color.black;
        txt.raycastTarget = false;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.font = AssetDatabase.LoadAssetAtPath<Font>("Assets\\_arts\\font\\" + glob_data.COMMFONTNAME + ".TTF");
        return txt;
    }
    [MenuItem("Assets/Utils/SetFont")]
    public static void SetTextFont()
    {
        if (Selection.gameObjects != null)
        {
            GameObject[] goArr = Selection.gameObjects;
            int k = 0;
            for (; k < goArr.Length; k++)
            {
                GameObject go = PrefabUtility.InstantiatePrefab(goArr[k] as GameObject) as GameObject;

                if (go != null)
                {
                    Font comFont = AssetDatabase.LoadAssetAtPath<Font>("Assets\\_arts\\font\\" + glob_data.COMMFONTNAME + ".TTF");
                    UIText[] txtArr = go.transform.GetComponentsInChildren<UIText>(true);
                    for (int i = 0; i < txtArr.Length; i++)
                    {
                        if (txtArr[i].font == null)
                        {
                            txtArr[i].font = comFont;
                        }
                        else if (glob_data.COMMFONTNAME.Equals(txtArr[i].font.name))
                        {
                            continue;
                        }
                        else if (txtArr[i].font.name.Equals("font_msyh") || txtArr[i].font.name.Equals("MSYH") || txtArr[i].font.name.Equals("Arial"))
                        {
                            txtArr[i].font = comFont;
                        }
                    }
                }
                //替换保存  
                PrefabUtility.ReplacePrefab(go, goArr[k], ReplacePrefabOptions.ConnectToPrefab);
                DestroyImmediate(go);
                AssetDatabase.Refresh();
                Debug.Log("字体替换完成");
            }
            Debug.Log("字体替换完成,共：" + k);
        }
    }

    [MenuItem("GameObject/UI/Button &b")]
    public static void CreatMyButton()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Btn_");
                UIText txt = InitText() as UIText;
                if (txt != null)
                {
                    txt.transform.SetParent(go.transform);
                    txt.gameObject.name = "txt_showName";
                    txt.styleId = 11;
                }
                SetParent(go);
                //go.AddComponent<Button>();
                UIButton uiButton = go.AddComponent<UIButton>();
                uiButton.btnName = txt;

                UIImage btnImg = go.AddComponent<UIImage>();
                SpriteAsset spr_common = AssetDatabase.LoadAssetAtPath<SpriteAsset>("Assets\\_arts\\ui_atlas\\" + "spr_common.asset");
                btnImg.spriteAtlas = spr_common;
                btnImg.spriteName = "btn_big_lan";
                btnImg.SetNativeSize();
                UIWrapper uIWrapper = go.GetComponentInParent<UIWrapper>();
                uiButton.uiWrapper = uIWrapper;
                Undo.RegisterCreatedObjectUndo(go, "createBtn" + go.name);
            }
        }
    }


    private static void SetParent(GameObject go)
    {
        if (go != null)
        {
            go.transform.SetParent(Selection.activeTransform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localEulerAngles = Vector3.zero;
            Selection.activeGameObject = go;
        }
    }

}
