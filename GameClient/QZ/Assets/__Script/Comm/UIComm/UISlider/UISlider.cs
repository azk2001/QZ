using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : Slider
{
    [SerializeField]
    public Image imageBackEffect = null;

    [SerializeField]
    public Image nextImage = null;

    [SerializeField]
    public List<RectTransform> fillImageList = new List<RectTransform>();

    [SerializeField]
    private int _sliderNum = 1;
    public int sliderNum    //血条总条数
    {
        get
        {
            return _sliderNum;
        }
        set
        {
            _sliderNum = value;

            imageCount = fillImageList.Count;

            if (imageCount > 0)
            {
                int index = (int)(_sliderNum % imageCount);

                int nextIndex = index - 1 < 0 ? imageCount - 1 : index - 1;

                if (lastImageIndex != index)
                {
                    lastImageIndex = index;

                    Image showImage = fillImageList[index].GetComponent<Image>();
                    Image backEffect = fillRect.GetComponent<Image>();

                    if (fillRect != null)
                    {
                        backEffect.sprite = showImage.sprite;
                        backEffect.color = showImage.color;
                    }

                    Image nextShowImage = fillImageList[nextIndex].GetComponent<Image>();
                    if (nextImage != null)
                    {
                        nextImage.sprite = nextShowImage.sprite;
                        nextImage.color = nextShowImage.color;
                    }

                    if (imageBackEffect != null)
                    {
                        imageBackEffect.sprite = nextImage.sprite;
                        Color color = nextImage.color;
                        color.a = 0.6f;
                        imageBackEffect.color = color;

                        imageBackEffect.fillAmount = 1;
                    }

                    curCount = _sliderNum;
                }
            }
        }
    }

    public float oneCount//一段的长度;
    {
        get
        {
            return 1.0f / sliderNum;
        }
    }

    [SerializeField]
    public override float value
    {
        get
        {
            return m_Value;
        }

        set
        {
            m_Value = value;
        }
    }

    [SerializeField]
    private float _dataValue = 1;
    public float dataValue
    {
        get
        {
            return Mathf.Clamp(_dataValue, minValue, maxValue); ;
        }

        set
        {

            float newValue = Mathf.Clamp(value, minValue, maxValue);

            // If the stepped value doesn't match the last one, it's time to update
            if (_dataValue == newValue)
                return;

            _dataValue = newValue;


            Set(_dataValue, true);


        }
    }

    public int curCount = 0;  //当前还有几段;

    private int lastImageIndex = 0;
    private int imageCount = 0;
    private Tweener m_Tweener;

    protected override void Awake()
    {
        imageCount = fillImageList.Count;
        curCount = sliderNum;

        base.Awake();
    }

    protected override void OnDisable()
    {
        sliderNum = 1;
    }

    protected override void Set(float input, bool sendCallback)
    {
        input = Mathf.Clamp(input, minValue, maxValue);

        curCount = Mathf.FloorToInt(dataValue / oneCount);

        value = dataValue % oneCount * sliderNum;

        if (value <0.001f && dataValue > 0.001f) value = 1;
        if (value < 0.001f && dataValue < 0.001f) value = 0;

        if (imageCount > 0)
        {
            int index = (int)(curCount % imageCount);

            int nextIndex = index - 1 < 0 ? imageCount - 1 : index - 1;

            if (lastImageIndex != index)
            {
                lastImageIndex = index;

                Image showImage = fillImageList[index].GetComponent<Image>();
                Image backEffect = fillRect.GetComponent<Image>();

                if (fillRect != null)
                {
                    backEffect.sprite = showImage.sprite;
                    backEffect.color = showImage.color;
                }

                Image nextShowImage = fillImageList[nextIndex].GetComponent<Image>();

                if (nextImage != null)
                {
                    if (curCount >= 1)
                    {
                        nextImage.enabled = true;
                        nextImage.sprite = nextShowImage.sprite;
                        nextImage.color = nextShowImage.color;
                    }
                    else
                    {
                        nextImage.enabled = false;
                    }
                }



                if (imageBackEffect != null)
                {
                    imageBackEffect.sprite = showImage.sprite;
                    Color color = nextImage.color;
                    color.a = 0.6f;
                    imageBackEffect.color = color;

                    imageBackEffect.fillAmount = 1;
                }
            }
        }

        OnRectTransformDimensionsChange();

        if (sendCallback)
            onValueChanged.Invoke(dataValue);
    }

    /// <summary>
    /// 播放背景动画
    /// </summary>
    public void PlayBackTweener()
    {
        if (imageBackEffect != null)
        {
            m_Tweener = DOTween.To(() => imageBackEffect.fillAmount, x => imageBackEffect.fillAmount = x, value, 0.3f);
        }
    }
}
