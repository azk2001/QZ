using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIScrollView : ScrollRect
{
    [Header("滚动方向提示")]
    public bool needTips = false;
    public Image topOrLeft;
    public Image downOrRight;
    public Transform selectFrame;
    private Bounds viewBounds;
    public int selectID;

    public void SetSelected(Transform root)
    {
        if (selectFrame && root)
        {
            selectFrame.SetParent(root);
            ((RectTransform)selectFrame).Reset();
            //((RectTransform)selectFrame).anchoredPosition = Vector3.zero;
            //selectFrame.localScale = Vector3.one;
            selectFrame.gameObject.SetActive(true);
            selectID = root.parent.GetInstanceID();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        onValueChanged.AddListener(OnValueChanged);
    }

    protected override void Start()
    {
        base.Start();
        OnValueChanged(Vector2.zero);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onValueChanged.RemoveListener(OnValueChanged);
    }
    private void OnValueChanged(Vector2 vector)
    {
        viewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);

        if (horizontal == true)
        {
            if (topOrLeft != null)
            {
                if (normalizedPosition.x <= float.Epsilon)
                {
                    ShowGameObject(topOrLeft.gameObject, false);
                }
                else
                {
                    if (hScrollingNeeded == true)
                        ShowGameObject(topOrLeft.gameObject, true);
                }
            }

            if (downOrRight != null)
            {
                if (normalizedPosition.x >= 0.98f)
                {
                    ShowGameObject(downOrRight.gameObject, false);
                }
                else
                {
                    if (hScrollingNeeded == true)
                        ShowGameObject(downOrRight.gameObject, true);
                }
            }
        }

        if (vertical == true)
        {
            if (topOrLeft != null)
            {
                if (normalizedPosition.y >= 0.98f)
                {
                    ShowGameObject(topOrLeft.gameObject, false);
                }
                else
                {
                    if (vScrollingNeeded == true)
                        ShowGameObject(topOrLeft.gameObject, true);
                }
            }

            if (downOrRight != null)
            {
                if (normalizedPosition.y <= float.Epsilon)
                {
                    ShowGameObject(downOrRight.gameObject, false);
                }
                else
                {
                    if (vScrollingNeeded == true)
                        ShowGameObject(downOrRight.gameObject, true);
                }
            }
        }
    }

    public void SetValue(float value, bool doTween = true)
    {
        this.enabled = false;
        TimeManager.Instance.Begin(0.1f, () =>
        {
            if (gameObject == null)
            {
                return -1;
            }
            if (value < 0) value = 0;
            if (value > 1) value = 1;

            RectTransform groups = content;

            RectTransform scrollRectTrans = this.GetComponent<RectTransform>();

            Vector2 data = groups.sizeDelta;
            Vector2 rect = scrollRectTrans.sizeDelta;

            Vector2 vector = groups.anchoredPosition;

            if (vertical)
            {
                float y = data.y - rect.y < 0 ? 0 : data.y - rect.y;

                vector.y = value * y;
            }
            else
            {
                float x = data.x - rect.x < 0 ? 0 : data.x - rect.x;
                vector.x = value * x;
            }

            if (vertical)
            {
                if (doTween)
                {
                    DOTween.To(() => groups.anchoredPosition, x => groups.anchoredPosition = x, vector, 0.2f);
                }

                else
                {
                    groups.anchoredPosition = vector;
                }
            }
            else
            {
                if (doTween)
                {
                    DOTween.To(() => groups.anchoredPosition, x => groups.anchoredPosition = x, -vector, 0.2f);
                }
                else
                {
                    groups.anchoredPosition = -vector;
                }
            }
            this.enabled = true;
            return -1;
        });

    }

    private bool hScrollingNeeded
    {
        get
        {
            if (Application.isPlaying)
                return m_ContentBounds.size.x > viewBounds.size.x + 0.1f;
            return true;
        }
    }
    private bool vScrollingNeeded
    {
        get
        {
            if (Application.isPlaying)
                return m_ContentBounds.size.y > viewBounds.size.y + 0.1f;
            return true;
        }
    }

    private void ShowGameObject(GameObject obj, bool show)
    {
        if (obj.activeSelf != show)
        {
            obj.SetActive(show);
        }
    }

    /// <summary>
    /// 指定一个 item让其定位到ScrollRect中间
    /// </summary>
    /// <param name="target">需要定位到的目标</param>
    public void CenterOnItem(Transform target, float duration = 0f)
    {
        RectTransform contentTransform = this.content;
        RectTransform viewPointTransform = this.viewport;
        // Item is here
        var itemCenterPositionInScroll = GetWorldPointInWidget(this.GetComponent<RectTransform>(), GetWidgetWorldPoint(target as RectTransform));
        Debug.Log("Item Anchor Pos In Scroll: " + itemCenterPositionInScroll);
        // But must be here
        var targetPositionInScroll = GetWorldPointInWidget(this.GetComponent<RectTransform>(), GetWidgetWorldPoint(viewPointTransform));
        Debug.Log("Target Anchor Pos In Scroll: " + targetPositionInScroll);
        // So it has to move this distance
        var difference = targetPositionInScroll - itemCenterPositionInScroll;
        difference.z = 0f;

        var newNormalizedPosition = new Vector2(difference.x / (contentTransform.rect.width - viewPointTransform.rect.width),
            difference.y / (contentTransform.rect.height - viewPointTransform.rect.height));

        newNormalizedPosition = this.normalizedPosition - newNormalizedPosition;

        newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
        newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);

        DOTween.To(() => this.normalizedPosition, x => this.normalizedPosition = x, newNormalizedPosition, duration);
    }

    Vector3 GetWidgetWorldPoint(RectTransform target)
    {
        if (target != null)
        {
            //pivot position + item size has to be included
            var pivotOffset = new Vector3(
                (0.5f - target.pivot.x) * target.rect.size.x,
                (0.5f - target.pivot.y) * target.rect.size.y,
                0f);
            var localPosition = target.localPosition + pivotOffset;
            return target.parent.TransformPoint(localPosition);
        }
        else
        {
            return Vector3.zero;
        }

    }

    Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
    {
        return target.InverseTransformPoint(worldPoint);
    }
}
