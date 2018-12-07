using UnityEngine;
using UnityEngine.EventSystems;
using Werewolf.StatusIndicators.Components;

class UIRollJoystick : UIJoyStick
{
    public static UIRollJoystick Instance = null;

    public VoidVector2Delegate OnRollDragEvent = null;

    private LineMissile lineMissile = null;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        SplatManager.Instance.SelectSpellIndicator("Line");

        lineMissile = SplatManager.Instance.CurrentSpellIndicator as LineMissile;

        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Transform target = SplatManager.Instance.targetPoint;
        Vector2 v2 = curDelta;
        if (v2.magnitude > offsetRadius)
        {
            v2 = v2.normalized;
            Vector3 startPoint = target.position;
            Vector3 forward = target.forward;
            Vector3 dir = new Vector3(v2.x, 0, v2.y);

            Vector3 moveForward = Quaternion.AngleAxis(Angle(Vector3.forward, dir), Vector3.up) * forward;

            Vector3 endPoint = startPoint + moveForward.normalized * 1.5f;

            if (lineMissile != null)
            {
                lineMissile.SetAngle(endPoint);
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
