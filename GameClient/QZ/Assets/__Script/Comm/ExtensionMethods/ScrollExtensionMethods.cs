using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ScrollExtensionMethods
{
    public static void SetSelectToFirst(this UIScrollView that)
    {
        GridLayoutGroup contentGrid = that.content.GetComponent<GridLayoutGroup>();
        int index = 0;
        bool find = false;
        foreach (Transform tf in contentGrid.transform)
        {
            if (tf.GetInstanceID() == that.selectID)
            {
                find = true;
                break;
            }
            index++;
        }
        if (find)
        {
            float value = (float)index / (contentGrid.transform.childCount);
            contentGrid.SetValue(value);
        }
    }
}

