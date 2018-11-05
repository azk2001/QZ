using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameMain
{
    public class UIStart : UIBase {

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

        public override void OnClick(GameObject clickObject)
        {
            if (clickObject.name.Equals("Btn_Start"))
            {
//                Debug.Log("click btn");
//                process_base.active("process_switch", "demo_process_game", "THD");
            }

            base.OnClick(clickObject);
        }       
    }
}