using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameObjectList : MonoBehaviour
{
    public List<Transform> gameObjectList = new List<Transform>();
    public List<Text> uiLableList = new List<Text>();
    
    /// <summary>
    /// 根据gameObjectList的索引获取游戏对象上的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index">ameObjectList的索引</param>
    /// <returns>组件</returns>
    public T GetUIComponent<T>(int index) where T : MonoBehaviour
    {
        if (index < 0 || index >= gameObjectList.Count)
        {
            return default(T);
        }
        Transform trans = gameObjectList[index];
        if (trans)
        {
            return trans.GetComponent<T>();
        }
        return default(T);
    }

}
