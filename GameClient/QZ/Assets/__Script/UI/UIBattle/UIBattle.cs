using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum eScenceType
{
    main,
    battle,
}

class UIBattle : UIBase
{
    public override eUIDepth uiDepth
    {
        get
        {
            return eUIDepth.ui_base;
        }
    }

    public override bool showing
    {
        get
        {
            return true;
        }
    }

    public static UIBattle Instance = null;

    private DOTweenAnimation tweenAnimation = null;

    public override bool TweenAni
    {
        get
        {
            return false;
        }
    }

    public DOTweenAnimation TweenAnimation
    {
        get
        {
            if (tweenAnimation == null)
            {
                tweenAnimation = gameObject.GetComponent<DOTweenAnimation>();
            }
            return tweenAnimation;
        }
    }
    

    public UIButton btnOnFire = null;

    public override void OnAwake(GameObject obj)
    {

        base.OnAwake(obj);

        Instance = this;

        btnOnFire = gameObjectList.GetUIComponent<UIButton>((int)1);
    }

    public override void OnInit()
    {
        base.OnInit();
    }

   
    public override void OnEnable()
    {
        base.OnEnable();

    }

    public override void OnDisable()
    {
        base.OnDisable();

    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        Instance = null;
    }

    protected override void OnHintChange(params object[] args)
    {
        base.OnHintChange(args);
    }
}