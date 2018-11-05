using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteAsset : ScriptableObject
{
    public int spriteNum = 0;

    /// <summary>
    /// 所有sprite信息 SpriteAssetInfor类为具体的信息类
    /// </summary>
    public List<SpriteInfor> spriteList = new List<SpriteInfor>();

    public void AddSprite(SpriteInfor spriteInfor)
    {
        for (int i=0;i< spriteList.Count;i++)
        {
            if (spriteList[i].name.Equals(spriteInfor.name))
            {
                spriteList[i] = spriteInfor;

                return;
            }
        }

        spriteList.Add(spriteInfor);
    }

    public Sprite GetSprite(string name)
    {
        foreach (var item in spriteList)
        {
            if (item.name.Equals(name))
                return item.sprite;
        }

        return null;
    }

    public void RemoveSprite(string name)
    {
        foreach (var item in spriteList)
        {
            if (item.name.Equals(name))
            {
                spriteList.Remove(item);
                return;
            }
        }
    }

}

[System.Serializable]
public class SpriteInfor
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name;
    /// <summary>
    /// 精灵
    /// </summary>
    public Sprite sprite;

    public Texture2D texture;
}
