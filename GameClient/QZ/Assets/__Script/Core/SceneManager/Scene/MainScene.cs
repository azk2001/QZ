using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainScene : SceneBase
{
    public override void OnBegin()
    {
        SceneManager.LoadScene("mainGame");

        UIManager.Instance.OpenUI(eUIName.UIGameMain);

        base.OnBegin();
    }

    //在这儿可以理解进入房间;
    public override void PlayerInScene(NetPlayer netPlayer)
    {
        GameUnit gameUnit = GameUnitManager.Instance.CreateServerGameUnit(netPlayer.uuid, netPlayer.basicsData, netPlayer.battleUnitData);

        player_c playerInfo = player_c.Get(netPlayer.basicsData.modleId);

        gameUnit.AddSkill(playerInfo.skillId);

        if (BattleProtocol.UUID == netPlayer.uuid)
        {
            LocalPlayer.Instance.gameUnit = gameUnit;

            PlayerController.Instance.AddPlayerController(gameUnit);
        }

        base.PlayerInScene(netPlayer);
    }

    //在这儿可以理解为离开房间;
    public override void PlayerOutScene(NetPlayer netPlayer)
    {
        GameUnitManager.Instance.RemoveGameUnit(netPlayer.uuid);

        base.PlayerOutScene(netPlayer);
    }

    public override void OnEnd()
    {
        GameUnitManager.Instance.RemoveAllGameUnit();

        base.OnEnd();
    }

}