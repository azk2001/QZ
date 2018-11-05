using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetBundleRequestMethods
{

    public static Object GetAssets(this AssetBundleRequest that, string resName)
    {
        if (that == null)
        {
            return null;
        }
           

        Object[] allAssets = that.allAssets;
        for (int i = 0, max = allAssets.Length; i < max; i++)
        {
            if (allAssets[i].name == resName)
                return allAssets[i];
        }
        return null;
    }

    public static T GetAssets<T>(this AssetBundleRequest that, string resName) where T : Object
    {
        if (that == null)
        {
            return null;
        }

        Object[] allAssets = that.allAssets;
        for (int i = 0, max = allAssets.Length; i<max; i++)
        {
            if (allAssets[i].name == resName)
                return allAssets[i] as T;
        }
        return null;
    }
}
