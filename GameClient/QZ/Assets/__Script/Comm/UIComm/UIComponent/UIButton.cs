using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIButton : Button
{

    public UIWrapper uiWrapper = null;
    /// <summary>
    /// 按钮名称
    /// </summary>
    public Text btnName;

    /// <summary>
    /// 传递参数
    /// </summary>
    public int msgParam;


    public object param;

    public bool clickTweener = true;

    private bool isClick = false;
    private PointerEventData downClickEventData = null;

    private int styleID = -1;
    protected override void Awake()
    {
        base.Awake();
        UIText btnShowName = btnName as UIText;
        if (btnShowName != null)
        {
            styleID = btnShowName.styleId;
        }
    }

    protected override void OnDisable()
    {
        isClick = false;
    }

    /// <summary>
    /// 设置按钮状态
    /// </summary>
    /// <param name="unUsable">true可用</param>
    /// <param name="btnNameColor">按钮名称颜色</param>
    public void SetBtnState(bool unUsable, Color btnNameColor)
    {
        if (btnName != null)
        {
            var showBtnName = btnName as UIText;
            if (showBtnName != null)
            {
                int id = btnNameColor == Color.white ? styleID : 92;
                showBtnName.SetStyle(id, true);
            }
        }
        if (image != null)
        {
            image.raycastTarget = unUsable;
        }
    }

    public void SetBtnState(bool unUsable)
    {
        if (image != null)
        {
            image.raycastTarget = unUsable;
            image.color = unUsable ? new Color(48, 63, 95) : Color.gray;
        }
        this.enabled = unUsable;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        // Debug.Log(eventData.)

        if (uiWrapper != null)
        {
            uiWrapper.OnClickObject(eventData.pointerPress);
        }

        //if(isClick ==false)
        //{
        //    isClick = true;

        //    if (uiWrapper != null)
        //    {
        //        uiWrapper.OnClickObject(eventData.pointerPress);
        //    }
        //}

        base.OnPointerClick(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isClick = false;

        downClickEventData = eventData;

        if (clickTweener == true)
        {
            DOTween.To(() => transform.localScale, r => transform.localScale = r, Vector3.one * 0.9f, 0.1f);
        }

        base.OnPointerDown(eventData);

        return;

        if (uiWrapper != null)
        {
            if (eventData.lastPress != null)
            {
                uiWrapper.OnTriggerPress(eventData.lastPress);
            }
        }

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (clickTweener == true)
        {
            DOTween.To(() => transform.localScale, r => transform.localScale = r, Vector3.one, 0.1f);
        }

        //if(isClick ==false)
        //{
        //    isClick = true;

        //    if (uiWrapper != null)
        //    {
        //        uiWrapper.OnClickObject(eventData.pointerPress);
        //    }
        //}

        base.OnPointerUp(eventData);

        return;

        if (uiWrapper != null)
        {
            if (eventData.pointerPress != null)
            {
                uiWrapper.OnTriggerRelease(eventData.pointerPress);
            }

        }
    }
}
