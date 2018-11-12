using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class BattleScene : SceneBase
{
    public static BattleScene instance = null;

    public GameUnit localGmeUnit = null;        //本地角色控制单位;

    public Dictionary<int, List<GameUnit>> battleUnitList = new Dictionary<int, List<GameUnit>>();

    public cs_dungeon curDungeonParam = null;

    public override void OnBegin()
    {
        SceneManager.LoadScene("battleGame");

        base.OnBegin();
    }

    public void AddNetPlayer(S2CStartGameMessage message)
    {
        foreach (PlayerBirthParam param in message.birthParamList)
        {
            NetPlayer netPlayer = NetPlayerManager.GetNetPlayer(param.uuid);
            netPlayer.battleUnitData.camp = param.camp;
            PlayerInScene(netPlayer);
        }

        SendStartBattle();
    }

    public override void PlayerInScene(NetPlayer netPlayer)
    {
        GameUnitData unitData = new GameUnitData();
        unitData.speed = 2;
        GameUnit gameUnit = GameUnitManager.Instance.CreateServerGameUnit(netPlayer.uuid, netPlayer.basicsData, unitData);

        SkillManager.Instance.AddSkill(gameUnit, 1);

        if (BattleProtocol.UUID == netPlayer.uuid)
        {
            PlayerController.Instance.AddPlayerController(gameUnit);

            LocalPlayer.Instance.netPlayer = netPlayer;
            LocalPlayer.Instance.gameUnit = gameUnit;
        }

        base.PlayerInScene(netPlayer);
    }

    public override void PlayerOutScene(NetPlayer netPlayer)
    {

        base.PlayerOutScene(netPlayer);
    }

    public void AddGameUnit(int campId, GameUnit gameUnit)
    {
        if (battleUnitList.ContainsKey(campId) == false)
        {
            battleUnitList[campId] = new List<GameUnit>();
        }
        battleUnitList[campId].Add(gameUnit);
    }

    //战斗结束;
    public void BattleEnd()
    {

    }

    private void OnEndGame()
    {

    }

    //发送场景准备完成;
    private void SendStartBattle()
    {
        C2SStartBattleMessage c2SStartBattle = new C2SStartBattleMessage();
        c2SStartBattle.uuid = BattleProtocol.UUID;

        BattleProtocolEvent.SendStartBattle(c2SStartBattle);
    }

    public void ReceiveStartBattle(S2CStartBattleMessage message)
    {
        //可以开始战斗;
        if(message.isStartBattle == 1)
        {

        }
    }


    public override void OnEnd()
    {

        BuffManager.Instance.RemoveAll();
        GameUnitManager.Instance.RemoveAllGameUnit();
        PlayerController.Instance.RemovePlayerController();

        BattleBuffRoot.Instance.DeSpwanAll();
        BattleEffectRoot.Instance.DeSpwanAll();
        BattleUnitRoot.Instance.DeSpwanAll();
        UIBattleRoot.instance.DeSpwanAll();

        base.OnEnd();
    }



    public void Init(int dungeonId)
    {
        battleUnitList.Clear();



    }

    //本地玩家创建完成;
    public void OnCreatePlayerFinish()
    {

    }

}
