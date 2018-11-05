using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

using System;
using System.Reflection;
using System.Collections.Generic;

namespace MXEngine
{
    public static class GameUtilsEditor
    {
        //检查一组资源的后缀
        public static bool CheckPathNameExtension(string extension, params UnityEngine.Object[] objs)
        {
            if (objs == null || objs.Length == 0)
                return false;

            for (int i = 0; i < objs.Length; ++i)
            {
                string path = AssetDatabase.GetAssetPath(objs[i]);
                if (!path.EndsWith(extension))
                    return false;
            }
            return true;
        }
    }
}