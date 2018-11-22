using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UIRoomListData
{


    //返回获取队伍列表;
    public static void ReceiveGetRoom(S2CGetRoomMessage message)
    {
        UIRoomList.message = message;
        if (UIRoomList.Instance == null)
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

        for (int i=0,max = message.playerCount;i<max;i++)
        {
            NetPlayer netPlayer = message.playerList[i];
            
            GameSceneManager.Instance.curScene.PlayerInScene(netPlayer);
        }

        UIManager.Instance.OpenUI(eUIName.UIRoom);
    }

    public static void ReceiveAddRoom(S2CAddRoomMessage message)
    {
        UIRoomList.Instance.RefreshAddRoom(message);

        for (int i = 0, max = message.playerCount; i < max; i++)
        {
            NetPlayer netPlayer = message.playerList[i];

            GameSceneManager.Instance.curScene.PlayerInScene(netPlayer);
        }

        UIManager.Instance.OpenUI(eUIName.UIRoom);
    }
}