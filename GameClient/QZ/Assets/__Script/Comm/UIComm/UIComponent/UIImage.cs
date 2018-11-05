using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIImage : Image
{
    public SpriteAsset spriteAtlas = null;

    public string spriteName
    {
        set
        {
            if (sprite == null)
            {
                if (spriteAtlas != null) sprite = spriteAtlas.GetSprite(value);
            }
            else
            {
                if (sprite.name != value)
                {
                    sprite = spriteAtlas.GetSprite(value);
                    //   UpdateMaterial();
                }
            }


        }
    }

    protected override void UpdateMaterial()
    {
        base.UpdateMaterial();
    }
}
