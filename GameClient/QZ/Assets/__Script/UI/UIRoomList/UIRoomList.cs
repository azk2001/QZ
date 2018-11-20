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
                    SendCreateRoom();
                }
                break;
            case "Btn_AddRoom":
                {

                }
                break;
        }
    }


    //返回获取队伍列表;
    public static void ReceiveGetRoom(S2CGetRoomMessage message)
    {
        UIRoomList.message = message;
        if (Instance == null)
        {
            UIManager.Instance.OpenUI(eUIName.UIRoomList);
        }
        else
        {
            UIRoomList.Instance.RefreshRoomList(message);
        }
    }

    public static void SendCreateRoom()
    {
        C2SCreateRoomMessage c2SCreateRoom = new C2SCreateRoomMessage();
        c2SCreateRoom.uuid = BattleProtocol.UUID;

        BattleProtocolEvent.SendCreateRoom(c2SCreateRoom);
    }

    public static void SendAddRoom(int roomIndex)
    {
        C2SAddRoomMessage c2SAddRoom = new C2SAddRoomMessage();
        c2SAddRoom.roomIndex = roomIndex;

        BattleProtocolEvent.SendAddRoom(c2SAddRoom);
    }

    public static void ReceiveCreateRoom(S2CCreateRoomMessage message)
    {
        UIRoomList.Instance.RefreshCreateRoom(message); 
    }

    public static void ReceiveAddRoom(S2CAddRoomMessage message)
    {
        UIRoomList.Instance.RefreshAddRoom(message);
    }

}