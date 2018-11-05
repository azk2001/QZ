using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UITableStyle
{
    public string tabName = "";		    //显示文本;
    public bool isShow = true;          //是否显示;
    public bool isStartShow = false;    //是否默认选中;
    public Sprite backSprite = null;    //背景显示图片; (不填就不显示)
    public Sprite showSprite = null;    //选中显示图片; (不填就不显示)
    public string msgTxt = "";  		//传出字符串;
    public int param;
    public UITableStyle(string tabName, string msgTxt) : this(msgTxt)
    {
        this.tabName = tabName;
    }
    public UITableStyle(string msgTxt)
    {
        this.msgTxt = msgTxt;
    }
    public UITableStyle()
    {

    }
}

public class UITable : MonoBehaviour
{

    public UIWrapper wrapper = null;

    private GridLayoutGroup group = null;
    private UITableItem defaultOnItem;

    private Dictionary<string, UITableItem> tabItemDic = new Dictionary<string, UITableItem>();

    private Transform transformCache = null;
    private bool ignore = false;
    public Transform seleFrame;
    public GridLayoutGroup Group
    {
        get
        {
            if (group == null)
            {
                group = this.GetComponent<GridLayoutGroup>();
            }
            return group;
        }

    }

    public Transform TransformCache
    {
        get
        {
            if (transformCache == null)
            {
                transformCache = transform;
            }
            return transformCache;
        }
    }

    public UITableItem GetSelected()
    {
        var itor = tabItemDic.GetEnumerator();
        while (itor.MoveNext())
        {
            if (itor.Current.Value.tog_button.isOn)
            {
                return itor.Current.Value;
            }
        }
        itor.Dispose();
        return null;
    }
    public void Reset()
    {
        if (defaultOnItem != null)
        {
            ignore = true;
            //重置时，阻断点击事件
            SetItemIsOn(defaultOnItem.txt_text.text);
            ignore = false;
        }

    }

    public void SetItemIsOn(int para, bool ignore = false)
    {
        var itor = tabItemDic.GetEnumerator();
        this.ignore = ignore;
        while (itor.MoveNext())
        {
            if (itor.Current.Value.param == para)
            {
                itor.Current.Value.tog_button.isOn = true;
                SetSelFrame(itor.Current.Value);
                continue;
            }
            itor.Current.Value.tog_button.isOn = false;
        }
        itor.Dispose();
        this.ignore = false;
        ChangeSelectTxtStytle();
    }
    public void SetItemIsOn(string itemName)
    {
        var itor = tabItemDic.GetEnumerator();
        while (itor.MoveNext())
        {
            if (itor.Current.Key == itemName)
            {
                itor.Current.Value.tog_button.isOn = true;
                SetSelFrame(itor.Current.Value);
                continue;
            }
            itor.Current.Value.tog_button.isOn = false;
        }
        itor.Dispose();
        ChangeSelectTxtStytle();
    }
    void Awake()
    {
        //TransformCache = this.transform;
        //group = this.GetComponent<GridLayoutGroup>();
    }

    public List<UITableItem> SetTableNum(List<UITableStyle> tableStyle, bool includeInactive, bool itemEqualStyle = false)
    {
        transform.AddChilden(tableStyle.Count, true);
        List<UITableItem> tableItems = new List<UITableItem>();

        TransformCache.GetComponentsInChildren<UITableItem>(includeInactive, tableItems);
        if (itemEqualStyle == false)
        {
            for (int i = tableItems.Count - 1; i >= 0; i--)
            {
                if (tableItems[i].gameObject.activeSelf == false)
                {
                    tableItems.RemoveAt(i);
                }
            }
        }
        for (int i = 0, max = Math.Min(tableItems.Count, tableStyle.Count); i < max; i++)
        {

            UITableItem item = tableItems[i];
            UITableStyle style = tableStyle[i];

            item.Init(style);
            if (item.tog_button.isOn)
            {
                defaultOnItem = item;
                SetSelFrame(item);
            }
            tabItemDic[style.tabName] = item;
            item.tog_button.onValueChanged.AddListener(OnSelected);
        }
        ChangeSelectTxtStytle();
        return tableItems;
    }
    public List<UITableItem> SetTableNum(List<UITableStyle> tableStyle)
    {
        Group.SetChild(tableStyle.Count, true);


        List<UITableItem> tableItems = new List<UITableItem>();

        TransformCache.GetComponentsInChildren<UITableItem>(true, tableItems);

        for (int i = tableItems.Count - 1; i >= 0; i--)
        {
            if (tableItems[i].gameObject.activeSelf == false)
            {
                tableItems.RemoveAt(i);
            }
        }
        ignore = true;
        for (int i = 0, max = tableItems.Count; i < max; i++)
        {

            UITableItem item = tableItems[i];
            UITableStyle style = tableStyle[i];

            item.Init(style);
            if (item.tog_button.isOn)
            {
                defaultOnItem = item;
                SetSelFrame(item);
            }
            tabItemDic[style.tabName] = item;

            ////注册一次
            //if (seleFrame != null)
            //{
            item.tog_button.onValueChanged.AddListener(OnSelected);
            //}
        }
        ignore = false;
        ChangeSelectTxtStytle();
        return tableItems;
    }

    private void ChangeSelectTxtStytle()
    {
        var itor = tabItemDic.GetEnumerator();
        while (itor.MoveNext())
        {
            if (itor.Current.Value.tog_button.isOn == true)
            {
                if (itor.Current.Value.txt_text != null)
                {
                    itor.Current.Value.txt_text.SetStyle(itor.Current.Value.Select_TxtstyleId);
                }
            }
            else
            {
                if (itor.Current.Value.txt_text != null)
                {
                    itor.Current.Value.txt_text.SetStyle(itor.Current.Value.NoSelect_TxtstyleId);
                }
            }
        }
        itor.Dispose();
    }

    private void SetSelFrame(UITableItem item)
    {
        if (seleFrame != null)
        {
            var rect = seleFrame as RectTransform;
            rect.pivot = 0.5f * Vector2.one;
            rect.anchorMax = 0.5f * Vector2.one;
            rect.anchorMin = 0.5f * Vector2.one;
            seleFrame.SetParent(item.transform);
            seleFrame.Reset();
            seleFrame.SetAsFirstSibling();
        }
    }

    private void OnSelected(bool arg0)
    {
        if (arg0 == true)
        {
            var itor = tabItemDic.GetEnumerator();
            while (itor.MoveNext())
            {
                if (itor.Current.Value.tog_button.isOn == true)
                {
                    SetSelFrame(itor.Current.Value);
                }
            }
            itor.Dispose();

            ChangeSelectTxtStytle();

        }

    }

    public UITableItem GetTableTrans(string tabName)
    {
        if (tabItemDic.ContainsKey(tabName))
            return tabItemDic[tabName];

        return null;
    }

    public UITableItem GetTableTrans(int para)
    {
        var itor = tabItemDic.GetEnumerator();
        while (itor.MoveNext())
        {
            if (itor.Current.Value.param == para)
            {
                return itor.Current.Value;
            }
        }
        itor.Dispose();
        return null;
    }
    //点击了table按钮;
    public void OnClick(Toggle obj)
    {
        if (obj.isOn == false)
            return;
        if (ignore)
        {
            return;
        }
        if (wrapper != null)
        {
            wrapper.OnClickObject(obj.gameObject);
        }
    }

}
