using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIGeneralItem : MonoBehaviour
{
    public UIImage backGround;
    public UIImage icon;
    public CircleImage circleIcon;
    public UIText showName;
    public int para;
    public UIText num;
    public UIImage checkedImg;
    public UIImage flagImg;
    public UIImage greenImg;
    public UIImage stateImg;

    public long itemuuid;      //item uuid
    public int itemId;
    public string uuid;    //player uuid

    [SerializeField]
    private bool needClickTips;
    private Button myBtn;

    public bool NeedClickTips
    {
        get
        {
            return needClickTips;
        }
        set
        {
            needClickTips = value;
        }
    }

    //public bool NeedClickTips
    //{
    //    get
    //    {
    //        return needClickTips;
    //    }

    //    set
    //    {
    //        if (myBtn == null)
    //        {
    //            myBtn = GetComponent<Button>();
    //        }
    //        if (value)
    //        {
    //            myBtn.onClick.AddListener(OnClick);
    //        }
    //        else
    //        {
    //            myBtn.onClick.RemoveListener(OnClick);
    //        }
    //        needClickTips = value;
    //    }
    //}

    private void Awake()
    {
        if (NeedClickTips == true)
        {
            myBtn = GetComponent<Button>();

            if (myBtn == null)
            {
                myBtn = this.gameObject.AddComponent<Button>();
            }

            if (myBtn != null)
            {
                myBtn.onClick.AddListener(OnClick);
            }
        }

        SetChecked(false);
    }
    private void Destroy()
    {
        myBtn.onClick.RemoveListener(OnClick);
    }
    private void OnClick()
    {
        if (NeedClickTips)
        {
            //广播
            EventListenerManager.Invoke(EventEnum.onclickItem, this);
        }
    }
    /// <summary>
    /// 选中标识
    /// </summary>
    public void SetChecked(bool isChecked)
    {
        if (checkedImg)
        {
            checkedImg.gameObject.SetActive(isChecked);
            checkedImg.transform.Reset();
        }
    }
}
