using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UIRoomData
{

    public static void SendStartGame(int roomIndex)
    {
        C2SStartGameMessage c2SStartGame = new C2SStartGameMessage();
        c2SStartGame.roomIndex = roomIndex;

        BattleProtocolEvent.SendStartGame(c2SStartGame);

    }

    public static void ReceiveStartGame(S2CStartGameMessage message)
    {
        UIManager.Instance.CloseUI(eUIName.UIRoom, false);
       
        ProcessManager.Instance.Begin(ProcessType.processbattle);

        BattleScene battleScene = GameSceneManager.Instance.GetScene<BattleScene>(SceneType.battle);
        battleScene.AddNetPlayer(message);
    }
}
