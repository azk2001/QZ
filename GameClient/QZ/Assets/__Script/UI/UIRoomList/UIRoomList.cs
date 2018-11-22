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
            return false;
        }
    }

    public static UIRoomList Instance = null;

    public static S2CGetRoomMessage message;

    public override void OnAwake(GameObject obj)
    {
        Instance = this;
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
        Instance = null;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

    }

    //刷新房间列表;
    public void RefreshRoomList(S2CGetRoomMessage message)
    {

    }

    //创建房间列表;
    public void RefreshCreateRoom(S2CCreateRoomMessage message)
    {

    }

    //加入房间列表;
    public void RefreshAddRoom(S2CAddRoomMessage message)
    {

    }

    public override void OnClick(GameObject clickObject)
    {
        base.OnClick(clickObject); 

        switch(clickObject.name)
        {
            case "Btn_CreateRoom":
                {
                    UIRoomListData.SendCreateRoom();
                }
                break;
            case "Btn_AddRoom":
                {
                    int roomIndex = 0;
                   UIRoomListData. SendAddRoom(roomIndex);
                }
                break;
        }
    }

   

}