using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GroupExtensionMethods
{

    public static void SetChild<T>(this GridLayoutGroup groups, int childLen, bool isActive, out T[] childArr)
    {
        childArr = default(T[]);
        Transform trans = groups.transform;

        if (trans.childCount < 1)
        {
            Debug.LogError("group not find child");
            return;
        }

        Transform childTrans = trans.GetChild(0);
        childTrans.gameObject.SetActive(false);

        int chidleCount = trans.childCount;
        //忽略第一个模版
        for (int i = 1; i < chidleCount; i++)
        {

            Transform t = trans.GetChild(i);

            if (i <= childLen)
            {
                if (t.gameObject.activeSelf != isActive)
                {
                    t.gameObject.SetActive(isActive);
                }
            }
            else
                t.gameObject.SetActive(false);
        }
        for (int i = trans.childCount - 1; i < childLen; i++)
        {
            GameObject obj = GameObject.Instantiate(childTrans.gameObject);
            obj.transform.parent = groups.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.name = childTrans.name;

            obj.SetActive(isActive);
        }

        childArr = groups.GetComponentsInChildren<T>(false);
    }

    public static bool SetChild(this GridLayoutGroup groups, int childLen, bool isActive)
    {
        Transform trans = groups.transform;

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
                if (t.gameObject.activeSelf != isActive)
                {
                    t.gameObject.SetActive(isActive);
                }
            }
            else
                t.gameObject.SetActive(false);
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
    public static Transform AddChild(this GridLayoutGroup groups, bool isActive)
    {
        Transform trans = groups.transform;

        if (trans.childCount < 1)
        {
            Debug.LogError("group not find child");
            return null;
        }

        Transform childTrans = trans.GetChild(0);

        GameObject obj = GameObject.Instantiate(childTrans.gameObject);
        obj.transform.parent = groups.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.name = childTrans.name;

        obj.SetActive(isActive);

        return obj.transform;
    }

    public static Transform AddChild(this GridLayoutGroup groups, Transform go, bool isActive)
    {
        Transform trans = groups.transform;

        Transform childTrans = go;

        GameObject obj = GameObject.Instantiate(childTrans.gameObject);
        obj.transform.parent = groups.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.name = childTrans.name;

        obj.SetActive(isActive);

        return obj.transform;
    }

    public static void ResetPosition(this GridLayoutGroup groups)
    {

        //ContentSizeFitter sizeFitter = groups.GetComponent<ContentSizeFitter>();

        //if(sizeFitter == null)
        //{
        //    sizeFitter = groups.gameObject.AddComponent<ContentSizeFitter>();
        //}

        //sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        //sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        //sizeFitter.enabled = true;

        //return;
        RectTransform scrollRectTrans = null;
        ScrollRect scrollRect = groups.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRectTrans = scrollRect.viewport;
        }

        groups.enabled = false;
        RectTransform rect = groups.GetComponent<RectTransform>();

        int count = 0;

        for (int i = 0, max = rect.childCount; i < max; i++)
        {
            Transform childTrans = rect.GetChild(i);
            if (childTrans.gameObject.activeSelf)
                count++;
        }

        int constraintCount = groups.constraintCount;

        int num = count * 1.0f % constraintCount == 0 ? count / constraintCount : (count / constraintCount + 1);

        Vector2 data = rect.sizeDelta;

        if (scrollRectTrans != null && scrollRect.content == groups.transform)
        {
            data = scrollRectTrans.sizeDelta;
        }

        if (groups.startAxis == GridLayoutGroup.Axis.Horizontal)
        {
            float y = num * (groups.cellSize.y + groups.spacing.y) + groups.padding.top + groups.padding.bottom;
            data.y = y > data.y ? y : data.y;
        }
        else
        {
            float x = num * (groups.cellSize.x + groups.spacing.x) + groups.padding.left + groups.padding.right;
            data.x = x > data.x ? x : data.x;
        }
        rect.sizeDelta = data;

        groups.enabled = true;
    }

    public static void SetValue(this GridLayoutGroup groups, float value)
    {
        //我也不知道怎么的，获取不到父节点的资源信息，只能延迟;
        TimeManager.Instance.Begin(0.1f, () => {

            if (value < 0) value = 0;
            if (value > 1) value = 1;

            RectTransform rect = groups.GetComponent<RectTransform>();

            ScrollRect scrollRect = groups.GetComponentInParent<ScrollRect>();
            if (scrollRect == null)
            {
                Debug.LogError("nof find ScrollRect");
                return -1;
            }
            RectTransform scrollRectTrans = scrollRect.GetComponent<RectTransform>();

            ResetPosition(groups);

            int constraintCount = groups.constraintCount;

            Vector2 data = rect.sizeDelta;
            Vector2 vector = rect.anchoredPosition;

            if (groups.startAxis == GridLayoutGroup.Axis.Horizontal)
            {
                float y = data.y - scrollRectTrans.sizeDelta.y < 0 ? 0 : data.y /*- scrollRectTrans.sizeDelta.y*/;

                vector.y = value * y;
            }
            else
            {

                float x = data.x - scrollRectTrans.sizeDelta.x < 0 ? 0 : data.x /*- scrollRectTrans.sizeDelta.x*/;

                vector.x = value * x;
            }

            if (scrollRect.horizontal == true)
            {
                rect.anchoredPosition = -vector;
            }
            else
            {
                rect.anchoredPosition = vector;
            }

            return -1;
        });
    }
}