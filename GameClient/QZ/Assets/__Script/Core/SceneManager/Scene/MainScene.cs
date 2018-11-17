using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainScene : SceneBase
{
    public override void OnBegin()
    {
        SceneManager.LoadScene("mainGame");

        List<NetPlayer> netPlayers = NetPlayerManager.GetAllNetPlayer();
        foreach(NetPlayer netPlayer in netPlayers)
        {
            PlayerInScene(netPlayer);
        }

        UIManager.Instance.OpenUI(eUIName.UIGameMain);

        base.OnBegin();
    }

    public override void PlayerInScene(NetPlayer netPlayer)
    {
        GameUnit gameUnit = GameUnitManager.Instance.CreateServerGameUnit(netPlayer.uuid, netPlayer.basicsData, netPlayer.battleUnitData);

        SkillManager.Instance.AddSkill(gameUnit, 1);

        if (BattleProtocol.UUID == netPlayer.uuid)
        {

            LocalPlayer.Instance.netPlayer = netPlayer;
            LocalPlayer.Instance.gameUnit = gameUnit;
        }

        base.PlayerInScene(netPlayer);
    }

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