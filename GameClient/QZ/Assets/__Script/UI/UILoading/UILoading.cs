using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    class UILoading : UIBase
    {
        public override eUIDepth uiDepth
        {
            get
            {
                return eUIDepth.ui_max;
            }
        }

        public override bool showing
        {
            get
            {
                return true;
            }
        }

        public override bool TweenAni
        {
            get
            {
                return false;
            }
        }

        private enum eObjectIndex
        {
            Img_Back,
            Slider,
            Txt_Desc,
        }

        private RawImage Img_Back = null;
        private Slider slider = null;
        private UIText Txt_Desc = null;

        public float sliderValue
        {
            get
            {
                return slider.value;
            }
            set
            {
                slider.value = value;
            }
        }

        public static UILoading Instance = null;

        private List<loadingtext_c> loadingtext = new List<loadingtext_c>();

        private List<string> backImgList = new List<string>()
        {
            "img_loading_back_0","img_loading_back_1","img_loading_back_2"
        };

        public override void OnAwake(GameObject obj)
        {
            base.OnAwake(obj);

            Img_Back = gameObjectList.GetUIComponent<RawImage>((int)eObjectIndex.Img_Back);
            slider = gameObjectList.GetUIComponent<Slider>((int)eObjectIndex.Slider);
            Txt_Desc = gameObjectList.GetUIComponent<UIText>((int)eObjectIndex.Txt_Desc);

            sliderValue = 0;

            Instance = this;

            isHide = true;

            gameObject.transform.localPosition = new Vector3(0, 0, -1000);
        }

        public override void OnInit()
        {
            base.OnInit();

            //RefreshUI();
        }
        
        public void RefreshUI()
        {
            //加载后续资源
            TableManager.Instance.AnalysisGameTable();

            sliderValue = 0;

            isHide = false;

            int backImgIndex = Random.Range(0, backImgList.Count);

            Img_Back.texture = Resources.Load<Texture>("UIBackImage/" + backImgList[backImgIndex]);
            
            TimeManager.Instance.Begin(0, ShowDescText);

            isHide = false;
            gameObject.SetActive(true);

            //加载后续资源
            TableManager.Instance.AnalysisGameTable();
        }

        private float ShowDescText()
        {
            if (loadingtext.Count < 1)
                return -1;

            int randomIndex = Random.Range(0, loadingtext.Count);

            Txt_Desc.text = loadingtext[randomIndex].text;

            return 3;
        }

        public override void OnDisable()
        {
            TimeManager.Instance.End(ShowDescText);

            Resources.UnloadAsset(Img_Back.texture);

            Img_Back.texture = null;

            base.OnDisable();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Instance = null;
        }

        public static void UIOpen()
        {
            if (Instance == null)
            {
               // UIManager.Instance.OpenUI(eUIName.UILoading);
            }
            else
            {
                Instance.RefreshUI();
            }
        }
    }
}
