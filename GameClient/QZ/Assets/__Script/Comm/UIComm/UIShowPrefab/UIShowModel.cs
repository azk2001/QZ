using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

/// <summary>
/// UI显示模型
/// 调用方式:UIShowPrefab.ShowPrefab(gb, position, layerName);
/// </summary>
[RequireComponent(typeof(RawImage))]
public class UIShowModel : MonoBehaviour
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

    void Awake()
    {
        //RawImage可以手动添加，设置no alpha材质，以显示带透明的粒子
        rawImage = gameObject.GetComponent<RawImage>();

        rawImage.color = Color.white;

        RawImage.enabled = false;
    }

    void OnEnable()
    {
        RawImage.enabled = true;

    }

    void OnDisable()
    {
        RawImage.enabled = false;
        HidePrefab();
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

    //public GameObject ShowPrefab(UnitControler unitCtrl, int uid = 0, Vector3 localPosition = default(Vector3), Vector3 localAngle = default(Vector3), float size = 1, string layerName = "UI")
    //{
    //    unitCtrl.play("custom_idle");
    //    return ShowPrefab(unitCtrl.trans_root.gameObject, uid, localPosition, localAngle, size, layerName);
    //}


    /// <summary>
    /// 显示模型到UI上;
    /// </summary>
    /// <param name="effectPrefab">模型资源</param>
    /// <param name="uid">uid</param>
    /// <param name="localPosition">偏移位置</param>
    /// <param name="layerName">layer名字</param>
    /// <returns></returns>
    public GameObject ShowPrefab(GameObject effectPrefab, int uid = 0, Vector3 localPosition = default(Vector3), Vector3 localAngle = default(Vector3), float size = 1, string layerName = "UI")
    {
        offsetVector.x += 100;
        localPosition.z = 100;
        if (rtCamera == null)
        {
            GameObject cameraObj = new GameObject("UIEffectCamera");
            rtCamera = cameraObj.AddComponent<Camera>();
            renderTexture = new RenderTexture((int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y, 24);
            renderTexture.antiAliasing = 4;
            rtCamera.clearFlags = CameraClearFlags.SolidColor;
            rtCamera.backgroundColor = new Color();
            rtCamera.cullingMask = 0;
            rtCamera.orthographic = true;
            rtCamera.orthographicSize = size;

            rtCamera.targetTexture = renderTexture;
            RawImage.texture = renderTexture;

            cameraObj.transform.position = offsetVector;

            GameObject.DontDestroyOnLoad(cameraObj);
        }

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