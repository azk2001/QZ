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
        RectTransform rect = transform as RectTransform;
        if (rect != null)
        {
            rect.anchoredPosition3D = Vector3.zero;
            rect.localEulerAngles = Vector3.zero;
            rect.localScale = Vector3.one;
        }
        else
        {
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;
        }

    }

    public static void ClearChild(this Transform transform, bool isActive = true)
    {
        Transform[] childList = transform.GetComponentsInChildren<Transform>();

        for (int i = childList.Length - 1; i >= 0; i--)
        {
            if (childList[i] == transform)
                continue;

            if (isActive == true)
            {
                if (childList[i].gameObject.activeSelf == true)
                {
                    if(Application.isEditor ==true)
                    {
                        GameObject.DestroyImmediate(childList[i].gameObject);
                    }
                    else
                    {
                        GameObject.Destroy(childList[i].gameObject);
                    }
                }
            }
            else
            {
                if (Application.isEditor == true)
                {
                    GameObject.DestroyImmediate(childList[i].gameObject);
                }
                else
                {
                    GameObject.Destroy(childList[i].gameObject);
                }
            }

        }

    }

    public static void AddChilden(this Transform trans, int childenNum, bool isActive)
    {
        int haveChildren = trans.childCount;
        for (int i = 0; i < haveChildren; i++)
        {
            trans.GetChild(i).gameObject.SetActive(i < childenNum);
        }
        for (int i = haveChildren; i < childenNum; i++)
        {
            trans.AddChild(true);
        }
    }
    public static Transform AddChild(this Transform trans, bool isActive)
    {
        if (trans.childCount < 1)
        {
            Debug.LogError("group not find child");
            return null;
        }

        Transform childTrans = trans.GetChild(0);

        GameObject obj = GameObject.Instantiate(childTrans.gameObject);
        obj.transform.parent = trans.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.name = childTrans.name;

        obj.SetActive(isActive);

        return obj.transform;
    }

    public static Transform AddChild(this Transform trans, Transform go, bool isActive)
    {

        Transform childTrans = go;

        GameObject obj = GameObject.Instantiate(childTrans.gameObject);
        obj.transform.SetParent(trans);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.name = childTrans.name;

        obj.SetActive(isActive);

        return obj.transform;
    }

    public static bool SetChild(this Transform trans, int childLen, bool isActive)
    {
        if (trans.childCount < 1)
        {
            Debug.LogError("group not find child");
            return false;
        }

        Transform childTrans = trans.GetChild(0);

        int chidleCount = trans.childCount;

        for (int i = 0; i < chidleCount; i++)
        {
            Transform t = trans.GetChild(i);

            if (i < childLen)
            {
                if(t.gameObject.activeSelf != isActive )
                {
                    t.gameObject.SetActive(isActive);
                }
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }

        for (int i = trans.childCount; i < childLen; i++)
        {
            GameObject obj = GameObject.Instantiate(childTrans.gameObject);
            obj.transform.SetParent(trans);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.name = childTrans.name;

            obj.SetActive(isActive);
        }
        return true;
    }


    public static Transform FindHideChildGameObject(this Transform parent, string childName)
    {
        if (parent.name == childName)
        {
            return parent;
        }

        if (parent.transform.childCount < 1)
        {
            return null;
        }

        Transform tf = null;

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            tf = parent.transform.GetChild(i);
            tf = FindHideChildGameObject(tf, childName);
            if (tf != null)
            {
                break;
            }
        }
        return tf;

    }

    public static void HideAllChild(this Transform trans)
    {
        int chidleCount = trans.childCount;

        for (int i = 0; i < chidleCount; i++)
        {
            Transform t = trans.GetChild(i);

            t.gameObject.SetActive(false);

        }
    }
}