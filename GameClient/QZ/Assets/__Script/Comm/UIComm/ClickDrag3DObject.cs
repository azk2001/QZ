using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickDrag3DObject : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public VoidVector3Delegate OnPointerEvent = null;

    public Transform target;
    public float speed = 1f;

    private Transform mTrans;

    private void Awake()
    {
        mTrans = transform;
    }


    void OnDrag(Vector2 delta)
    {
        if (target != null)
        {
            target.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * target.localRotation;
        }
        else
        {
            mTrans.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * mTrans.localRotation;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDrag(eventData.delta);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnPointerEvent != null)
        {
            if (Vector3.Distance(eventData.pressPosition, eventData.position) < 10)
                OnPointerEvent(eventData.position);
        }
    }
}
