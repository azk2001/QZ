using UnityEngine;
using UnityEngine.EventSystems;
using Werewolf.StatusIndicators.Components;

class UISkill1Joystick : UIJoyStick
{
    public static UISkill1Joystick Instance = null;

    public VoidIntVector3Delegate OnFireEvent = null;

    private AngleMissile angleMissile = null;
    private LineMissile lineMissile = null;

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
            OnFireEvent(GlobData.skill1Index, curDelta.normalized);
        }

        base.OnEndDrag(eventData);
    }
}
