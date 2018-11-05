using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGuideClick : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UIWrapper uiWrapper = null;

    public bool isClick = true;

    //监听点击
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClick == false)
            return;

        if (uiWrapper != null)
        {
            uiWrapper.OnClickObject(eventData.pointerPress);
        }

        PassEvent(eventData, ExecuteEvents.pointerDownHandler);
        PassEvent(eventData, ExecuteEvents.pointerClickHandler);
        PassEvent(eventData, ExecuteEvents.pointerUpHandler);

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isClick == false)
            return;

        PassEvent(eventData, ExecuteEvents.dragHandler);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isClick == false)
            return;

        PassEvent(eventData, ExecuteEvents.beginDragHandler);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isClick == false)
            return;

        PassEvent(eventData, ExecuteEvents.endDragHandler);
    }


    //把事件透下去
    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = data.position;
        eventData.pressPosition = data.position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        GameObject current = data.pointerCurrentRaycast.gameObject;

        //如果当前点击的按钮是背景框就退出引导
        if (current.name.Equals("Btn_TipsGuideMaskBack"))
            return;

        for (int i = 0; i < results.Count; i++)
        {
            GameObject curObject = results[i].gameObject;

            if (curObject == this.gameObject)
                continue;

            if (current != curObject)
            {
                data.pointerPress = curObject;
                data.pointerDrag = curObject;

                ExecuteEvents.Execute(curObject, data, function);
                // break;
                //RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
            }
        }
    }


}