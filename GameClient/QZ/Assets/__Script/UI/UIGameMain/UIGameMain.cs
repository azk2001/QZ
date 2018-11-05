using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameMain
{
    public enum eScenceType
    {
        main,
        battle,
    }

    class UIGameMain : UIBase
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

        public static UIGameMain Instance = null;

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

            Instance = this;
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

        public override void OnPress(GameObject clickObject)
        {
            base.OnPress(clickObject);
        }

        public override void OnRelease(GameObject clickObject)
        {
            base.OnRelease(clickObject);
        }

        public override void OnClick(GameObject clickObject)
        {
            base.OnClick(clickObject);
        }

        protected override void OnHintChange(params object[] args)
        {
            base.OnHintChange(args);
        }
    }
}
