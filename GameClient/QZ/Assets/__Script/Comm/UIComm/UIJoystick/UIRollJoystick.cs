using UnityEngine;
using UnityEngine.EventSystems;
using Werewolf.StatusIndicators.Components;

class UIRollJoystick : UIJoyStick
{
    public static UIRollJoystick Instance = null;

    public VoidVector2Delegate OnRollDragEvent = null;

    private AngleMissile angleMissile= null;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        SplatManager.Instance.SelectSpellIndicator("Direction");

        angleMissile = SplatManager.Instance.CurrentSpellIndicator as AngleMissile;

        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Transform target = SplatManager.Instance.targetPoint;
        Vector2 v2 = curDelta;
        if (v2.magnitude > offsetRadius)
        {
            v2 = v2.normalized;

            Vector3 dir = new Vector3(v2.x, 0, v2.y);
            Vector3 moveForward = Quaternion.AngleAxis(Angle(Vector3.forward, dir), Vector3.up) * target.forward;
            Vector3 endPoint = target.position + moveForward.normalized * 1.5f;

            if (angleMissile != null)
            {
                angleMissile.SetAngle(endPoint);
            }
        }

        base.OnDrag(eventData);
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
