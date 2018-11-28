using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIRoom : UIBase
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

    public override void OnClick(GameObject clickObject)
    {
        switch(clickObject.name)
        {
            case "Btn_StartGame":
                SendStartGame();
                break;
        }

        base.OnClick(clickObject);
    }


    public void SendStartGame()
    {
        UIRoomData.SendStartGame(UIRoomList.addRoomIndex);
    }
}
