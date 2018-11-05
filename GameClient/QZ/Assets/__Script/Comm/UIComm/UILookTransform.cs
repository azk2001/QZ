using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookTransform : MonoBehaviour
{
    public Transform contTrans = null;
    public Transform lookTrans = null;

    public Vector3 offset = Vector3.zero;

    public Vector3 neglectPoint = new Vector3(0.1f, 0, 0.1f);

    public float showRadius = 12;

    public bool checkMyPlayer = false;

    private bool lastShow = true;
    private float dis = 0;

    private float p = 0.2f;
    private float dataTime = 0;

    private Vector3 position = Vector3.zero;
    private Vector3 lastPoint = Vector3.zero;
    private Vector3 rightUpPoint = Vector3.one;
    private Vector3 leftDownPoint = Vector3.one;

    private void Awake()
    {
        if (contTrans == null)
            contTrans = this.transform;

        neglectPoint =Vector3.zero;
    }

    void LateUpdate() 
    {
        if (lookTrans == null)
            return;

        if (contTrans == null)
            return;

        //Vector3 screenPoint = glob_data.WorldToUI(lookTrans.position);
        //screenPoint = screenPoint + offset;

        rightUpPoint = glob_data.WorldToUI(lookTrans.position + neglectPoint);
        rightUpPoint = rightUpPoint + offset;

        leftDownPoint = glob_data.WorldToUI(lookTrans.position - neglectPoint);
        leftDownPoint = leftDownPoint + offset;

        if (lastPoint.x < rightUpPoint.x)
        {
            lastPoint.x = rightUpPoint.x;
        }

        if (lastPoint.y > rightUpPoint.y)
        {
            lastPoint.y = rightUpPoint.y;
        }

        if (lastPoint.x > leftDownPoint.x)
        {
            lastPoint.x = leftDownPoint.x;
        }

        if (lastPoint.y < leftDownPoint.y)
        {
            lastPoint.y = leftDownPoint.y;
        }

        contTrans.position = lastPoint;

        dataTime += Time.deltaTime;

        if (dataTime > p)
        {
            dataTime = 0;
        }
    }

    private void IsShow(bool isShow)
    {
        if (lastShow != isShow)
        {
            contTrans.gameObject.SetActive(isShow);

            lastShow = isShow;
        }
    }
}
