using UnityEngine;
using UnityEngine.EventSystems;

class UIRollJoystick : UIJoyStick
{
    public static UIRollJoystick Instance = null;

    public VoidVector2Delegate OnRollDragEvent = null;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (curDelta.magnitude > offsetRadius)
        {
            OnRollDragEvent(curDelta.normalized);
        }

        base.OnEndDrag(eventData);
    }
}
