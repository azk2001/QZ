using UnityEngine;
using UnityEngine.EventSystems;

class UIAtkJoystick : UIJoyStick
{
    public static UIAtkJoystick Instance = null;

    public VoidIntVector3Delegate OnFireEvent = null;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        if (OnFireEvent != null)
        {
            OnFireEvent(1, Vector3.zero);
        }

    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (OnFireEvent != null)
        {
            OnFireEvent(1, Vector3.zero);
        }

        base.OnEndDrag(eventData);
    }
}
