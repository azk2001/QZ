﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum eJoyStickDrogType
{
    onBegin = 1,
    onDrag = 2,
    onEnd = 3,
}

public class UIJoyStick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
  
    public float radius = 0f;

    public float offsetRadius = 0;

    public RectTransform joyStickParent = null;
    public RectTransform touchImage = null;


    private Camera uiCamera = null;
    private DOTweenAnimation[] tweener = null;
    private Transform transformCache = null;

    protected Vector2 curDelta = Vector2.zero;
    protected Vector2 lastDelta = Vector2.zero;
    protected Vector3 offsetPosition = Vector3.zero;
    protected Vector3 offset = Vector3.zero;
    protected bool isRun = false;
    protected bool isDrag = false;

    public bool isActiveJoyStick = true;

    public bool isIgnoreOffset = true;

    private void Start()
    {
        
        transformCache = this.transform;

        uiCamera = GlobData.uiCamera;

        tweener = transformCache.GetComponentsInChildren<DOTweenAnimation>();

        isDrag = true;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (isActiveJoyStick == false)
            return;

        offsetPosition = uiCamera.ScreenToWorldPoint(eventData.position);
        offsetPosition.z = 0;

        touchImage.position = offsetPosition;

        curDelta = touchImage.anchoredPosition;

        if (curDelta.magnitude > radius)
        {
            curDelta = curDelta.normalized * radius;
            touchImage.anchoredPosition = curDelta;
        }
      
        RefreshAngle();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (isActiveJoyStick == false)
            return;

        isDrag = true;

        offsetPosition = uiCamera.ScreenToWorldPoint(eventData.position);
        offsetPosition.z = 0;
        touchImage.position = offsetPosition;

        if (touchImage.anchoredPosition.magnitude > radius)
        {
            touchImage.anchoredPosition = touchImage.anchoredPosition.normalized * radius;
        }

        PlayTweener(true);

        RefreshAngle();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;

        touchImage.anchoredPosition = Vector2.zero;

        PlayTweener(false);

    }

    private void PlayTweener(bool isForward)
    {
        for (int i = 0, max = tweener.Length; i < max; i++)
        {
            tweener[i].DORestart();
            if (isForward == true)
            {
                tweener[i].DOPlayForward();
            }
            else
            {
                tweener[i].DOPlayBackwards();
            }
        }

    }
    
    protected void RefreshAngle()
    {
        if (touchImage.anchoredPosition != Vector2.zero)
        {
            touchImage.right = touchImage.anchoredPosition;
        }
    }
    
}