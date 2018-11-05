using UnityEngine;
using System.Collections;

public static class TransformExtensionMethods
{
    /// <summary>
    /// 确定子节点数量, 如果不足够添加至ensureLen长度. 并且显示出来, 多余的隐藏...
    /// </summary>
    /// <returns><c>true</c>, 有变化时, <c>false</c> 没有变化.</returns>
    /// <param name="transform">父节点</param>
    /// <param name="ensureLen">确保长度</param>
    /// <param name="copyIndex">如果不足够, 参考下标</param>
    public static bool EnsureChildNumberSpareFlase(this Transform transform, int ensureLen, int copyIndex)
    {
        bool isChange = false;
        int transformChildCount = transform.childCount;
        int c = ensureLen - transformChildCount;
        if (c > 0)
        {
            if (copyIndex >= transformChildCount)
            {
                Debug.LogError("EnsureChildNumberSpareFlase() copyIndex error: " + copyIndex);
                return false;
            }

            isChange = true;
            Transform tranChild = transform.GetChild(copyIndex);
            GameObject instanceObj = tranChild.gameObject;
            //Vector3 localScale = tranChild.localScale;
            do
            {
                GameObject.Instantiate(instanceObj);
                //GameObject obj = (GameObject)GameObject.Instantiate(instanceObj, transform, false);
                //Transform t = obj.transform;
                //obj.name = instanceObj.name;
                //t.localScale = localScale;
            } while (--c > 0);
        }

        c = 0;
        while (c < ensureLen)
        {
            GameObject obj = transform.GetChild(c).gameObject;
            if (!obj.activeSelf)
            {
                isChange = true;
                obj.SetActive(true);
            }
            ++c;
        }

        while (ensureLen < transformChildCount)
        {
            GameObject obj = transform.GetChild(ensureLen).gameObject;
            if (obj.activeSelf)
            {
                isChange = true;
                obj.SetActive(false);
            }
            else
                break;
            ++ensureLen;
        }

        return isChange;
    }

    public static void SetLayers(this Transform transform, int layer)
    {
        transform.gameObject.layer = layer;
        for (int i = 0, imax = transform.childCount; i < imax; ++i)
        {
            Transform child = transform.GetChild(i);
            SetLayers(child, layer);
        }
    }

    public static void SetLayers(this Transform transform, string layerName)
    {
        SetLayers(transform, LayerMask.NameToLayer(layerName));
    }

    public static void Reset(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;
    }
}