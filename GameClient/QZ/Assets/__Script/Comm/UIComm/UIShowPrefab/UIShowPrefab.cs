using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;
using System.Collections.Generic;

/// <summary>
/// UI显示模型
/// 调用方式:UIShowPrefab.ShowPrefab(gb, position, layerName);
/// </summary>
[RequireComponent(typeof(RawImage))]
public class UIShowPrefab : MonoBehaviour
{
    public Vector3 localPosition = Vector3.zero;
    public Vector3 localEulerAngles = Vector3.zero;
    public float size = 1;
    public bool rotate = true;

    public int showUid = 0;

    private RenderTexture renderTexture;
    private Camera rtCamera;
    private RawImage rawImage;

    private GameObject effectGO;

    private SpinWithMouse swm = null;

    private static Vector3 offsetVector = Vector3.zero;

    private Dictionary<int, int> particLayer = new Dictionary<int, int>();

    void Awake()
    {
        //RawImage可以手动添加，设置no alpha材质，以显示带透明的粒子
        rawImage = gameObject.GetComponent<RawImage>();

        rawImage.color = Color.white;

        rawImage.material = Resources.Load<Material>("UIShowPrefab/UI_Material");

        RawImage.enabled = false;


    }

    void OnEnable()
    {
        RawImage.enabled = true;

        TimeManager.Instance.End(OnHidePrefab);
    }

    void OnDisable()
    {
        RawImage.enabled = false;

        TimeManager.Instance.End(OnHidePrefab);
        TimeManager.Instance.Begin(0.1f, OnHidePrefab);
    }

    private float OnHidePrefab()
    {
        HidePrefab();
        return -1;
    }

    public RectTransform rectTransform
    {
        get
        {
            return transform as RectTransform;
        }
    }

    public RawImage RawImage
    {
        get
        {
            if (rawImage == null)
            {
                rawImage = GetComponent<RawImage>();
            }
            return rawImage;
        }
    }

    public SpinWithMouse Swm
    {
        get
        {
            if (swm == null && rotate)
            {
                swm = gameObject.AddComponent<SpinWithMouse>();

            }
            return swm;
        }
    }

    /// <summary>
    /// 显示模型到UI上;
    /// </summary>
    /// <param name="effectPrefab">模型资源</param>
    /// <param name="uid">uid</param>
    /// <param name="localPosition">偏移位置</param>
    /// <param name="layerName">layer名字</param>
    /// <returns></returns>
    public GameObject ShowPrefab(GameObject effectPrefab, int uid = 0, Vector3 localPosition = default(Vector3), Vector3 localAngle = default(Vector3), float size = 170f, string layerName = "UI")
    {
        localPosition.z = -300;
        if (localPosition.Equals(default(Vector3)) == false)
        {
            this.localPosition = localPosition;
        }

        if (localAngle.Equals(default(Vector3)) == false)
        {
            this.localEulerAngles = localAngle;
        }
       

        return effectGO;
    }

    public void HidePrefab()
    {

        if (effectGO != null)
        {
          

            effectGO = null;
        }

        if (rtCamera != null)
        {
            Destroy(rtCamera.gameObject);
            rtCamera = null;
        }

        if (renderTexture != null)
        {
            Destroy(renderTexture);
            renderTexture = null;
        }
    }

    void OnDestroy()
    {
        HidePrefab();
    }
}