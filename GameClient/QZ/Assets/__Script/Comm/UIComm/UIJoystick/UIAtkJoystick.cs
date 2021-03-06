﻿using UnityEngine;
using UnityEngine.EventSystems;

class UIAtkJoystick : UIJoyStick
{
    public static UIAtkJoystick Instance = null;

    public VoidIntVector3Delegate OnFireEvent = null;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (isActiveJoyStick == false)
            return;

        isDrag = true;

        base.OnPointerEnter(eventData);

        if (OnFireEvent != null)
        {
            OnFireEvent(GlobData.atkFireIndex, Vector3.zero);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (OnFireEvent != null)
        {
            OnFireEvent(GlobData.atkEndIndex, Vector3.zero);
        }

        base.OnPointerExit(eventData);
    }
}
