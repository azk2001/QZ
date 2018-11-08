using UnityEngine;
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
    public static UIJoyStick Instance = null;

    public VoidVector2Delegate OnDragEvent = null;

    public float radius = 0f;

    public float offsetRadius = 0;

    public RectTransform joyStickParent = null;

    public RectTransform touchImage = null;

    private Vector2 curDelta = Vector2.zero;
    private Vector2 lastDelta = Vector2.zero;
    private Vector3 offsetPosition = Vector3.zero;

    private Camera uiCamera = null;

    private DOTweenAnimation[] tweener = null;

    private Transform transformCache = null;

    private Vector3 offset = Vector3.zero;
    private bool isRun = false;
    private bool isDrag = false;

    public bool isActiveJoyStick = true;

    public bool isIgnoreOffset = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        transformCache = this.transform;

        uiCamera = GlobData.uiCamera;

        tweener = transformCache.GetComponentsInChildren<DOTweenAnimation>();

        isDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
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
       
        if (curDelta.magnitude > offsetRadius)
        {
            if (isIgnoreOffset ==true)
            {
                if (Vector3.Angle(curDelta, lastDelta) > 2)
                {
                    OnDragEvent(curDelta.normalized);

                    lastDelta = curDelta;
                }
            }
            else
            {
                OnDragEvent(curDelta.normalized);
                lastDelta = curDelta;
            }
            
        }
        else
        {
            lastDelta = curDelta = Vector2.zero;
            OnDragEvent(Vector2.zero);
        }

        RefreshAngle();


    }
    public void OnBeginDrag(PointerEventData eventData)
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

        if (touchImage.anchoredPosition.magnitude > offsetRadius)
        {
            OnDragEvent(touchImage.anchoredPosition.normalized);

            EventListenerManager.Invoke(EventEnum.localPlayerStartMovePoint, null);
        }
        else
        {
            OnDragEvent(Vector2.zero);
        }

        RefreshAngle();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;

        touchImage.anchoredPosition = Vector2.zero;

        OnDragEvent(Vector2.zero);

        PlayTweener(false);

        EventListenerManager.Invoke(EventEnum.localPlayerEndMovePoint, null);
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

            if (OnDragEvent != null)
                OnDragEvent(touchImage.anchoredPosition.normalized);
        }
        else
        {
            if (isRun == true)
            {
                isRun = false;
                OnDragEvent(Vector2.zero);
            }
        }

    }

    void RefreshAngle()
    {
        if (touchImage.anchoredPosition != Vector2.zero)
        {
            touchImage.right = touchImage.anchoredPosition;
        }
    }


}