using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


class CameraLookPlayer : MonoBehaviour
{
    public static CameraLookPlayer Instance = null;

    public Transform targetTrans = null;
    public Vector3 lookOffset = Vector3.zero; //目标偏移;

    public float maxHight = 4;      //照相机最大高度;
    public float minHight = 0.5f;   //照相机最底高度;

    public float distance = 4;      //照相机和角色之间的距离;
    public float eulerAngles = 0;   //和世界前向的旋转角度;
    public float curHight = 0;      //当前照相机的高度;

    public bool isLook = true; //是否执行;

    private Transform transformCache = null;
    private Vector3 cameraPos = Vector3.zero;  //照相机现在的位置;

    private void Awake()
    {
        transformCache = this.transform;
        Instance = this;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (isLook == false)
            return;

        if (targetTrans == null)
            return;

        Vector3 lookPos = targetTrans.position + lookOffset;

        transformCache.LookAt(lookPos);

        Vector3 cameraForward = Quaternion.AngleAxis(eulerAngles, Vector3.up) * Vector3.forward;

        Vector3 tempPos = targetTrans.position - cameraForward * distance;
        tempPos.y  = Mathf.Lerp(tempPos.y, curHight, 0.1f);

        if ((tempPos.y - targetTrans.position.y) > maxHight)
        {
            tempPos.y = targetTrans.position.y + maxHight;
        }

        if ((tempPos.y - targetTrans.position.y) < minHight)
        {
            tempPos.y = targetTrans.position.y - minHight;
        }

        cameraPos = tempPos;

        transformCache.position = cameraPos;
    }

    private void OnEnable()
    {

    }
}
