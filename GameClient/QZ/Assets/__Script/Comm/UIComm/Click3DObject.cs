using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Click3DObject : SingleClass<Click3DObject>
{

    private Vector3 inputPosition = Vector3.zero;
    private bool isClick = false;
    private bool isSlide = false;

    private float slideDataPosition = 0;

    private Vector3 toFollowOffset = new Vector3(0, 20, -20);
    private Vector3 toTrackedObjectOffset = new Vector3(0, -10, 0);

    private float touchDir = 0;

    public bool isControlSlide = true;
    private readonly List<int> rayHitList = new List<int>();

    void Start()
    {

    }

    public void Update()
    {
        if (isControlSlide == false)
            return;

#if !UNITY_EDITOR

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 1)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    inputPosition = touch.position;

                    isClick = true;
                    isSlide = false;
                }
            }
            else
            {
                isClick = false;
            }

            if (Input.touchCount == 2)
            {
                isClick = false;
                isSlide = true;

                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                if (touchDir == 0)
                {
                    touchDir = (touch0.position - touch1.position).SqrMagnitude();
                }

                slideDataPosition = touchDir - (touch0.position - touch1.position).SqrMagnitude();

                touchDir = (touch0.position - touch1.position).SqrMagnitude();
            }
            else
            {
                touchDir = 0;
                isSlide = false;
            }
        }
        else
        {
            touchDir = 0;
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            inputPosition = Input.mousePosition;

            isClick = true;
        }

        slideDataPosition = Input.GetAxis("Mouse ScrollWheel") / 0.00002f;
        if (slideDataPosition != 0)
        {
            isSlide = true;
        }
        else
        {
            slideDataPosition = 0;
            isSlide = false;
        }
#endif

        if (isClick == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);

            if (EventSystem.current != null)
            {
                if (EventSystem.current.IsPointerOverGameObject() == false && IsPointerOverUIObject(inputPosition) == false)
                {

                   
                }
            }


            //if (Physics.Raycast(ray, out rayhit) && !EventSystem.current.IsPointerOverGameObject())
            //{
            //    obj_ref objRef = rayhit.transform.GetComponent<obj_ref>();
            //    if (objRef != null)
            //    {
            //        EventListenerManager.Invoke(EventEnum.onClick3DObject, objRef._uid);
            //    }
            //}

            isClick = false;
        }


        if (!EventSystem.current.IsPointerOverGameObject() && IsPointerOverUIObject(inputPosition) == false)
        {
            if (isSlide == true)
            {
                //float val = main.Instance.inputField.text.ToFloat(1);

              
            }
        }
    }

    //方法二 通过UI事件发射射线
    //是 2D UI 的位置，非 3D 位置
    public bool IsPointerOverUIObject(Vector2 screenPosition)
    {
        //实例化点击事件
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        //将点击位置的屏幕坐标赋值给点击事件
        eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        //向点击处发射射线
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }


}
