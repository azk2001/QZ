using UnityEngine;
using UnityEngine.EventSystems;

class UIMoveJoystick : UIJoyStick
{
    public static UIMoveJoystick Instance = null;

    public VoidVector2Delegate OnMoveDragEvent = null;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        if (touchImage.anchoredPosition.magnitude > offsetRadius)
        {
            OnMoveDragEvent(touchImage.anchoredPosition.normalized);
            EventListenerManager.Invoke(EventEnum.LOCAL_PLAYER_STAER_MOVE_POINT, null);
        }
        else
        {
            OnMoveDragEvent(Vector2.zero);
        }

    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        if (curDelta.magnitude > offsetRadius)
        {
            if (isIgnoreOffset == true)
            {
                if (Vector3.Angle(curDelta, lastDelta) > 2)
                {
                    OnMoveDragEvent(curDelta.normalized);

                    lastDelta = curDelta;
                }
            }
            else
            {
                OnMoveDragEvent(curDelta.normalized);
                lastDelta = curDelta;
            }

        }
        else
        {
            lastDelta = curDelta = Vector2.zero;
            OnMoveDragEvent(Vector2.zero);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        EventListenerManager.Invoke(EventEnum.LOCAL_PLAYER_END_MOVE_POINT, null);

        OnMoveDragEvent(Vector2.zero);

        base.OnEndDrag(eventData);
    }

    public void Update()
    {
        if (isActiveJoyStick == false)
            return;

        if (isDrag == true)
            return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            offset.y = 1;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            offset.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            offset.y = -1;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            offset.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            offset.x = -1;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            offset.x = 0;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            offset.x = 1;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            offset.x = 0;
        }

        touchImage.anchoredPosition = offset * 50;

        RefreshAngle();

        if (offset.magnitude > 0)
        {
            isRun = true;

            if (OnMoveDragEvent != null)
                OnMoveDragEvent(touchImage.anchoredPosition.normalized);
        }
        else
        {
            if (isRun == true)
            {
                isRun = false;
                OnMoveDragEvent(Vector2.zero);
            }
        }

    }
}
