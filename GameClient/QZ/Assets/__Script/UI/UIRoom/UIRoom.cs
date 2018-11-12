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
            case "":

                break;
        }

        base.OnClick(clickObject);
    }

    public void SendCreateRoom()
    {
        C2SCreateRoomMessage c2SCreateRoom = new C2SCreateRoomMessage();
        c2SCreateRoom.uuid = BattleProtocol.UUID;

        BattleProtocolEvent.SendCreateRoom(c2SCreateRoom);
    }

    public void SendAddRoom(int roomIndex)
    {
        C2SAddRoomMessage c2SAddRoom = new C2SAddRoomMessage();
        c2SAddRoom.roomIndex = roomIndex;

        BattleProtocolEvent.SendAddRoom(c2SAddRoom);
    }

    public void SendGetRoom()
    {
        C2SGetRoomMessage c2SGetRoom = new C2SGetRoomMessage();
        c2SGetRoom.uuid = BattleProtocol.UUID;

        BattleProtocolEvent.SendGetRoom(c2SGetRoom);

    }

    public void ReceiveCreateRoom(S2CCreateRoomMessage message)
    {

    }

    public void ReceiveGetRoom(S2CGetRoomMessage message)
    {

    }

    public void ReceiveAddRoom(S2CAddRoomMessage message)
    {
       
    }
}
