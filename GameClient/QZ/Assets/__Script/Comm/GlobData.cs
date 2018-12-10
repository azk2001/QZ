using System;
using UnityEngine;
using System.Collections;

public delegate void VoidVoidDelegate();
public delegate void VoidFloatDelegate(float val);
public delegate void VoidStringDelegate(string val);
public delegate void VoidIntDelegate(int val);
public delegate void VoidIntIntDelegate(int val, int val1);
public delegate void VoidVector2Delegate(Vector2 val);
public delegate void VoidVector3Delegate(Vector3 val);
public delegate void VoidIntVector3Delegate(int val, Vector3 val1);
public delegate void VoidIntVector2Delegate(int val, Vector2 val1);
public delegate void VoidObjectDelegate(object val);
public delegate float FloatDelegate();
public delegate void VoidTransDelegate(Transform val);
public delegate void VoidIntStringDelegate(int val, string arg);
public delegate Vector3 Vector3Delegate();
public delegate void VoidByteDelegate(byte[] arg);
public delegate void VoidBoolDelegate(bool flag);
public delegate void VoidStrBoolDelegate(string str, bool isTrue);
public delegate void VoidColliderDelegate(Collider other);


public enum eModleType
{
    common = 1,
    elite = 2,
    boss = 3,
    player = 100,
}

public static class GlobData
{
    public static int atkEndIndex = 0;
    public static int atkFireIndex = 1;
    public static int skill1Index = 2;
    public static int skill2Index = 3;


    private static Camera _uiCamera = null;
    public static Camera uiCamera
    {
        get
        {
            if (_uiCamera == null)
            {
                if (GameObject.FindGameObjectWithTag("UICamera") == null)
                    return null;

                _uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
            }
            return _uiCamera;
        }
    }

    /// <summary>
    /// 添加UI照相机照射layer
    /// </summary>
    public static void UICameraSetLayer(bool isAddLayer, string layerName)
    {
        if (uiCamera == null)
            return;

        int layer = LayerMask.NameToLayer(layerName);

        if (isAddLayer == true)
        {
            uiCamera.cullingMask |= (1 << layer);  // 打开x层
        }
        else
        {
            uiCamera.cullingMask &= ~(1 << layer); // 关闭x层
        }
    }

    /// <summary>
    /// 添加UI照相机照射layer
    /// </summary>
    public static void CameraSetLayer(bool isAddLayer, string layerName)
    {
        Camera camera = Camera.main;

        if (camera == null)
            return;

        int layer = LayerMask.NameToLayer(layerName);

        if (isAddLayer == true)
        {
            camera.cullingMask |= (1 << layer);  // 打开x层
        }
        else
        {
            camera.cullingMask &= ~(1 << layer); // 关闭x层
        }
    }


    public static Vector3 WorldToUI(Vector3 worldPoint)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPoint);
        screenPoint = uiCamera.ScreenToWorldPoint(screenPoint);
        screenPoint.z = 0;

        return screenPoint;
    }

    public static Vector3 UIToWorld(Vector3 uiPoint)
    {
        Vector3 worldPoint = uiCamera.WorldToScreenPoint(uiPoint);
        worldPoint = Camera.main.ScreenToWorldPoint(worldPoint);
        worldPoint.z = 0;

        return worldPoint;
    }

    public static void SetGameQuality()
    {
        PlayerPrefs.SetInt("isBackMusic", 1);
        PlayerPrefs.SetInt("isBackSound", 1);
        PlayerPrefs.SetInt("isShowShadow", 1);
        PlayerPrefs.SetInt("isShowStreamer", 1);
        PlayerPrefs.SetInt("gameQuality", 3);
        PlayerPrefs.SetInt("playerNum", 10);
    }
}
