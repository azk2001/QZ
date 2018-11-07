using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum eUIDepth
{
    ui_base = 1,    //底层UI 					深度区间（100-199）;
    ui_system = 2,  //系统UI					深度区间（200-299）;
    ui_box = 3,     //弹出提示性质UI			深度区间（300-399）;
    ui_tips = 4,    //提示UI					深度区间（400-499）;
    ui_text = 5,    //提示文本性质UI			深度区间（500-599）;
    ui_max = 6,		//最顶层UI; 类似断线重连;	深度区间（600-699）;
}

//UI基础管理类;
public abstract class UIBase
{
    private GameObject _gameObject = null;

    private bool tweenAni = true;

    /// <summary>
    /// UI GameObject;
    /// </summary>
    public GameObject gameObject
    {
        get
        {
            return _gameObject;
        }
    }

    private UIGameObjectList _gameObjectList = null;

    public UIGameObjectList gameObjectList
    {
        get
        {
            return _gameObjectList;
        }
    }

    /// <summary>
    /// 是否隐藏;
    /// </summary>
    public bool isHide
    {
        get
        {
            return gameObject.activeSelf == false;
        }
        set
        {
            if (gameObject != null)
            {
                if (value)
                {
                    if (TweenAni)
                    {
                        if (Img_Mask != null)
                        {
                            Img_Mask.DOScale(0, 0);
                        }
                        DOTween.To(() => gameObject.transform.localScale, x => gameObject.transform.localScale = x, Vector3.zero, 0.2f).OnComplete
                            (() =>
                            {
                                gameObject.SetActive(false);
                            });
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }
                else
                {
                    gameObject.SetActive(true);
                }

            }
        }
    }

    /// <summary>
    /// UI深度;
    /// </summary>
    public abstract eUIDepth uiDepth { get; }

    /// <summary>
    /// 是否常显;
    /// </summary>
    public abstract bool showing { get; }

    public virtual bool TweenAni
    {
        get
        {
            return tweenAni;
        }
    }

    /// <summary>
    /// 主接收点击事件;
    /// </summary>
    protected GraphicRaycaster graphic = null;


    protected UIBase()
    {
        EventListenerManager.AddListener((int)EventEnum.getHint, OnGetRootHintSystem);
    }


    /// <summary>
    /// Init 在UI初始化的时候调用;
    /// </summary>
    /// <param name="obj">UI 的 GameObcject</param>
    public virtual void OnInit()
    {
        if (TweenAni)
        {
            if (Img_Mask != null)
            {
                Img_Mask.DOScale(1, 0).SetDelay(0.1f);
            }
            DOTween.To(() => gameObject.transform.localScale, x => gameObject.transform.localScale = x, Vector3.one,
                0.2f);
        }
        else
        {
            gameObject.transform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// 设置UI的深度
    /// </summary>
    /// <param name="depth"></param>
    public void SetDepth(int depth)
    {
        Canvas canvas = gameObjectList.GetComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = depth;

        graphic = gameObjectList.GetComponent<GraphicRaycaster>();
        graphic.ignoreReversedGraphics = false;
        //  graphic.

    }

    //点击事件返回统一处理;
    private void OnUIEventTrigger(params object[] args)
    {
        UIEventParam eventParam = (UIEventParam)args[0];

        switch (eventParam.eventType)
        {
            case EventIdentity.uiStart:
                OnStart();
                break;
            case EventIdentity.uiDestroy:
                OnDestroy();
                break;
            case EventIdentity.uiEnable:
                OnEnable();
                break;
            case EventIdentity.uiDisable:
                OnDisable();
                break;
            case EventIdentity.uiClick:
                OnClick(eventParam.gameobject);
                break;
            case EventIdentity.uiLongClick:
                OnLongClick(eventParam.gameobject);
                break;
            case EventIdentity.uiPress:
                OnPress(eventParam.gameobject);
                break;
            case EventIdentity.uiRelease:
                OnRelease(eventParam.gameobject);
                break;
        }
    }

    public Transform Img_Mask = null;
    /// <summary>
    /// GameObject Awake;
    /// </summary>
    /// <param name="obj">GameObject</param>
    public virtual void OnAwake(GameObject obj)
    {
        _gameObject = obj;
        _gameObjectList = _gameObject.GetComponent<UIGameObjectList>();

        EventListenerManager.AddListener(gameObject.GetInstanceID(), OnUIEventTrigger);
    }

    /// <summary>
    /// GameObject Start;
    /// </summary>
    public virtual void OnStart()
    {

    }

    /// <summary>
    /// GameObject Destroy;
    /// </summary>
    public virtual void OnDestroy()
    {
        EventListenerManager.RemoveListener(gameObject.GetInstanceID(), OnUIEventTrigger);
    }

    /// <summary>
    /// GameObject Enable;
    /// </summary>
    public virtual void OnEnable()
    {

    }

    /// <summary>
    /// GameObejct Disable;
    /// </summary>
    public virtual void OnDisable()
    {

    }

    /// <summary>
    /// 点击事件;
    /// </summary>
    /// <param name="clickObject">点击的 GameObject</param>
    public virtual void OnClick(GameObject clickObject)
    {

    }

    /// <summary>
    /// 长按事件,只触发1次;
    /// </summary>
    /// <param name="clickObject">点击的 GameObject</param>
    public virtual void OnLongClick(GameObject clickObject)
    {

    }

    /// <summary>
    /// 触碰事件;
    /// </summary>
    /// <param name="clickObject">点击的 GameObject</param>
    public virtual void OnPress(GameObject clickObject)
    {

    }

    /// <summary>
    /// 释放事件;
    /// </summary>
    /// <param name="clickObject">点击的 GameObject</param>
    public virtual void OnRelease(GameObject clickObject)
    {

    }

    /// <summary>
    /// 获取根节点红绿点开放显示;
    /// </summary>
    /// <param name="args">返回红绿灯配置</param>
    public virtual void OnGetRootHintSystem(params object[] args)
    {

    }

    //设置红绿点功能;
    protected virtual void OnHintChange(params object[] args)
    {

    }
}