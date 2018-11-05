//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2017 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.U2D;
using System;

/// <summary>
/// Editor component used to display a list of sprites.
/// </summary>

public class SpriteSelector : ScriptableWizard
{
    static public SpriteSelector instance;

    void OnEnable() { instance = this; }
    void OnDisable() { instance = null; }

    public Action<string> mCallback;

    public SpriteAsset spriteAsset = null;
    private Vector2 scrollPos;

    private string selectText = "";

    private List<SpriteInfor> sprites = new List<SpriteInfor>();

    /// <summary>
    /// Draw the custom wizard.
    /// </summary>

    void OnGUI()
    {

        GUILayout.Space(20);

        selectText = EditorGUILayout.TextField("查找:", selectText);

        GUILayout.Space(30);

        EditorGUIUtility.labelWidth = 80;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (spriteAsset == null)
        {
            GUILayout.Label("No Atlas selected.", "LODLevelNotifyText");
        }
        else
        {
            sprites.Clear();
            if (selectText.Equals("") == true)
            {
                sprites = new List<SpriteInfor>(spriteAsset.spriteList);
            }
            else
            {
                for (int i = 0; i < spriteAsset.spriteList.Count; i++)
                {
                    if (spriteAsset.spriteList[i].name.Contains(selectText))
                    {
                        sprites.Add(spriteAsset.spriteList[i]);
                    }
                }

            }

            float size = 80f;
            float padded = size + 10f;
#if UNITY_4_7
			int screenWidth = Screen.width;
#else
            int screenWidth = (int)EditorGUIUtility.currentViewWidth;
#endif
            int columns = Mathf.FloorToInt(screenWidth / padded);
            if (columns < 1) columns = 1;

            int offset = 0;
            Rect rect = new Rect(10f, 0, size, size);

            GUILayout.Space(10f);
            int rows = 1;

            while (offset < sprites.Count)
            {
                GUILayout.BeginHorizontal();
                {
                    int col = 0;
                    rect.x = 10f;

                    for (; offset < sprites.Count; ++offset)
                    {
                        Sprite sprite = sprites[offset].sprite;

                        if (sprite == null)
                            continue;

                        // Button comes first
                        if (GUI.Button(rect, ""))
                        {
                            if (Event.current.button == 0)
                            {
                                if (mCallback != null)
                                {
                                    string str = sprite.name;

                                    mCallback(str);
                                    
                                    Close();
                                }
                            }
                        }

                        if (Event.current.type == EventType.Repaint)
                        {
                            // Calculate the texture's scale that's needed to display the sprite in the clipped area
                            float scaleX = 1;
                            float scaleY = 1;

                            // Stretch the sprite so that it will appear proper
                            float aspect = (scaleY / scaleX) / ((float)sprite.texture.height / sprite.texture.width);
                            Rect clipRect = rect;

                            if (aspect != 1f)
                            {
                                if (aspect < 1f)
                                {
                                    // The sprite is taller than it is wider
                                    float padding = size * (1f - aspect) * 0.5f;
                                    clipRect.xMin += padding;
                                    clipRect.xMax -= padding;
                                }
                                else
                                {
                                    // The sprite is wider than it is taller
                                    float padding = size * (1f - 1f / aspect) * 0.5f;
                                    clipRect.yMin += padding;
                                    clipRect.yMax -= padding;
                                }
                            }

                            GUI.DrawTexture(clipRect, sprite.texture);

                        }

                        GUI.backgroundColor = new Color(1f, 1f, 1f, 0.5f);
                        GUI.contentColor = new Color(1f, 1f, 1f, 0.7f);
                        GUI.Label(new Rect(rect.x, rect.y + rect.height, rect.width, 50f), sprite.name, "ProgressBarBack");
                        GUI.contentColor = Color.white;
                        GUI.backgroundColor = Color.white;

                        if (++col >= columns)
                        {
                            ++offset;
                            break;
                        }
                        rect.x += padded;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(padded);
                rect.y += padded + 36;
                ++rows;
            }
            GUILayout.Space(rows * 35);

        }

        EditorGUILayout.EndScrollView();
    }

}
