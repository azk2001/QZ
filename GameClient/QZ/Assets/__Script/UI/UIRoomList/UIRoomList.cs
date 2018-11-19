using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

class UIRoomList : UIBase
{
    public override eUIDepth uiDepth
    {
        get
        {
            return eUIDepth.ui_system;
        }
    }

    public override bool showing
    {
        get
        {
            return true;
        }
    }
    
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

    public override void OnAwake(GameObject obj)
    {

        base.OnAwake(obj);
        
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
        
    }

    protected override void OnHintChange(params object[] args)
    {
        base.OnHintChange(args);
    }
}