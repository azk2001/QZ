using UnityEngine;
using UnityEngine.EventSystems;

class UISkill1Joystick : UIJoyStick
{
    public static UISkill1Joystick Instance = null;

    public VoidIntVector3Delegate OnFireEvent = null;

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
        if (OnFireEvent!=null)
        {
            OnFireEvent(2,curDelta.normalized);
        }

        base.OnEndDrag(eventData);
    }
}
