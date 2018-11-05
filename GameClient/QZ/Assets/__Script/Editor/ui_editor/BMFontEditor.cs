using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml;

public class BMFontEditor : EditorWindow
{
    [MenuItem("UGUI/Font")]
    private static void ShowFont()
    {
        BMFontEditor bmFont = EditorWindow.CreateInstance<BMFontEditor>();
        bmFont.titleContent = new GUIContent("美术字体生成");
        bmFont.Show();
    }


    private Texture2D texture2D = null;
    private TextAsset fontAsset = null;

    private string fontName = null;

    private void OnGUI()
    {
        fontName = EditorGUILayout.TextField("字体名字:", fontName);

        texture2D = (Texture2D)EditorGUILayout.ObjectField("贴图", texture2D, typeof(Texture2D), true);
        fontAsset = (TextAsset)EditorGUILayout.ObjectField("字体文件", fontAsset, typeof(TextAsset), true);

        if (GUILayout.Button("生成"))
        {
            if (texture2D == null)
            {
                Debug.LogError("请添加贴图");
            }
            else if (fontAsset == null)
            {
                Debug.LogError("请添加字体");
            }
            else
            {
                CreateFont();
            }
        }

    }

    private void CreateFont()
    {
        string path = AssetDatabase.GetAssetPath(texture2D);
        path = Path.GetDirectoryName(path);
        Material mtr = new Material(Shader.Find("UI/Default Font"));//创建的材质球加载进来
        mtr.mainTexture = texture2D;
        //.SetTexture(0, texture2D);//把图片赋给材质球

        AssetDatabase.CreateAsset(mtr, path + "/" + fontName + ".mat");

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(fontAsset.text);//这是在BMFont里得到的那个.fnt文件,因为是xml文件，所以我们就用xml来解析
        List<CharacterInfo> chtInfoList = new List<CharacterInfo>();
        XmlNode node = xml.SelectSingleNode("font/chars");
        foreach (XmlNode nd in node.ChildNodes)
        {
            XmlElement xe = (XmlElement)nd;
            int x = int.Parse(xe.GetAttribute("x"));
            int y = int.Parse(xe.GetAttribute("y"));
            int width = int.Parse(xe.GetAttribute("width"));
            int height = int.Parse(xe.GetAttribute("height"));
            int advance = int.Parse(xe.GetAttribute("xadvance"));
            CharacterInfo chtInfo = new CharacterInfo();
            float texWidth = texture2D.width;
            float texHeight = texture2D.width;

            chtInfo.glyphHeight = texture2D.height;
            chtInfo.glyphWidth = texture2D.width;
            chtInfo.index = int.Parse(xe.GetAttribute("id"));
            //这里注意下UV坐标系和从BMFont里得到的信息的坐标系是不一样的哦，前者左下角为（0,0），
            //右上角为（1,1）。而后者则是左上角上角为（0,0），右下角为（图宽，图高）
            chtInfo.uvTopLeft = new Vector2((float)x / texture2D.width, 1 - (float)y / texture2D.height);
            chtInfo.uvTopRight = new Vector2((float)(x + width) / texture2D.width, 1 - (float)y / texture2D.height);
            chtInfo.uvBottomLeft = new Vector2((float)x / texture2D.width, 1 - (float)(y + height) / texture2D.height);
            chtInfo.uvBottomRight = new Vector2((float)(x + width) / texture2D.width, 1 - (float)(y + height) / texture2D.height);

            chtInfo.minX = 0;
            chtInfo.minY = -height;
            chtInfo.maxX = width;
            chtInfo.maxY = 0;

            chtInfo.advance = advance;

            chtInfoList.Add(chtInfo);
        }

        Font font = new Font();
        font.material = mtr;
        font.characterInfo = chtInfoList.ToArray();

        AssetDatabase.CreateAsset(font, path + "/" + fontName + ".fontsettings");

        AssetDatabase.Refresh();
    }

}