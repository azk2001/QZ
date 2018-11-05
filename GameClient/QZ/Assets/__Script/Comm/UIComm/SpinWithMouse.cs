using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class SpinWithMouse :MonoBehaviour,IDragHandler
{
    public Transform target;
    public float speed = 1f;

    private Transform mTrans;

    public void OnDrag(PointerEventData eventData)
    {
        OnDrag(eventData.delta);
    }

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
}
