using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{

    public class UIGameFinish : UIBase
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
                return false;
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

        public override void OnDisable()
        {
            base.OnDisable();
          
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
          
        }

        public override void OnClick(GameObject clickObject)
        {
            base.OnClick(clickObject);

            switch (clickObject.name)
            {
              
            }
        }
    }

}
