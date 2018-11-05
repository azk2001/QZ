using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILogin : UIBase
{

    public override eUIDepth uiDepth
    {
        get { return eUIDepth.ui_system; }
    }

    public override bool showing
    {
        get { return false; }
    }
    public override bool TweenAni
    {
        get { return false; }
    }
    private enum eObjectIndex
    {
        Input_userName,
        Input_passWord,
    }

    private InputField Input_userName = null;
    private InputField Input_passWord = null;


    public override void OnAwake(GameObject obj)
    {
        base.OnAwake(obj);

        Input_userName = gameObjectList.gameObjectList[(int)eObjectIndex.Input_userName].GetComponent<InputField>();
        Input_passWord = gameObjectList.gameObjectList[(int)eObjectIndex.Input_passWord].GetComponent<InputField>();


        Input_userName.text = PlayerPrefs.GetString("UserName", "UserName");
        Input_passWord.text = PlayerPrefs.GetString("PassWord", "PassWord");

    }
    public override void OnInit()
    {
        base.OnInit();
    }
    public override void OnStart()
    {
        base.OnStart();

    }

    public override void OnClick(GameObject clickObject)
    {
        switch (clickObject.name)
        {
            case "btn_login":
                Debug.Log(Input_userName.text);
                Debug.Log(Input_passWord.text);

                PlayerPrefs.SetString("UserName", Input_userName.text);
                PlayerPrefs.SetString("PassWord", Input_passWord.text);
                break;
        }


    }

    private void OnLoadFinish(int val)
    {
        //        Debug.Log(val);
        UIManager.Instance.CloseUI(eUIName.UILogin);
        UIManager.Instance.OpenUI(eUIName.UISelectServer);
    }


}
