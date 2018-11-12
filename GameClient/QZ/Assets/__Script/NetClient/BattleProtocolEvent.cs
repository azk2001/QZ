using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗网络数据处理中心
/// </summary>
public static class BattleProtocolEvent
{
    private static BytesWriter writer = new BytesWriter();
    public static BytesReader reader = new BytesReader();

    public static void SendLogin(C2SLoginMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_Login);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }

    public static void ReceiveLogin(BytesReader reader)
    {
        S2CLoginMessage message = new S2CLoginMessage();
        message.Message(reader);

        UILogin login = UIManager.Instance.GetUIBase<UILogin>(eUIName.UILogin);
        login.OnLoadFinish();

    }

    public static void SendCreatePlayer(C2SCreatePlayerMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_CreatePlayer);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }

    public static void ReceiveCreatePlayer(BytesReader reader)
    {
        S2CCreatePlayerMessage message = new S2CCreatePlayerMessage();
        message.Message(reader);

        for(int i=0;i<message.playerCount;i++)
        {
            NetPlayerManager.AddNetPlayer(message.netPlayer[i]);
        }

        UICreatePlayer createPlayer = UIManager.Instance.GetUIBase<UICreatePlayer>(eUIName.UICreatePlayer);
        createPlayer.OnCreateFinish();

    }

    public static void ReceivePlayerInScene(BytesReader reader)
    {
        S2CPlayerInSceneMessage message = new S2CPlayerInSceneMessage();
        message.Message(reader);

        NetPlayerManager.AddNetPlayer(message.netPlayer);

        GameSceneManager.Instance.curScene.PlayerInScene(message.netPlayer);

    }
    

    public static void SendGetRoom(C2SGetRoomMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_GetRoom);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }

    public static void ReceiveGetRoom(BytesReader reader)
    {
        S2CGetRoomMessage message = new S2CGetRoomMessage();
        message.Message(reader);
    }

    public static void SendCreateRoom(C2SCreateRoomMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_CreateRoom);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }

    public static void ReceiveCreateRoom(BytesReader reader)
    {
        S2CCreateRoomMessage message = new S2CCreateRoomMessage();
        message.Message(reader);
    }

    public static void SendStartGame(C2SStartGameMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_StartGame);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }

    public static void ReceiveStartGame(BytesReader reader)
    {
        S2CStartGameMessage message = new S2CStartGameMessage();
        message.Message(reader);
    }

    public static void SendStartBattle(C2SStartBattleMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_StartBattle);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }

    public static void ReceiveStartBattle(BytesReader reader)
    {
        S2CStartBattleMessage message = new S2CStartBattleMessage();
        message.Message(reader);
    }

    public static void SendPlayerMove(C2SPlayerMoveMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_PlayerMove);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }

    public static void ReceivePlayerMove(BytesReader reader)
    {
        S2CPlayerMoveMessage message = new S2CPlayerMoveMessage();
        message.Message(reader);


        GameUnit mGameUnit = GameUnitManager.Instance.GetGameUnit(message.uuid);

        Vector3 forward = new Vector3(message.fx / 100.0f, message.fy / 100.0f, message.fz / 100.0f);
        Vector3 moveDir = new Vector3(message.mx / 100.0f, message.my / 100.0f, message.mz / 100.0f);
        Vector3 position = new Vector3(message.px / 100.0f, message.py / 100.0f, message.pz / 100.0f);

        mGameUnit.SetForward(forward);

        //客服端直接移动;
        mGameUnit.PlayRunAnimation(moveDir);
        
        if(moveDir == Vector3.zero)
        {
            mGameUnit.mUnitController.MoveToPoint(position);
        }
    }

    public static void SendPlayerSkill(C2SPlayerSkillMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_PlayerSkill);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }

    public static void ReceivePlayerSkill(BytesReader reader)
    {
        S2CPlayerSkillMessage message = new S2CPlayerSkillMessage();
        message.Message(reader);
    }

    public static void SendPlayerHit(C2SPlayerHitMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_PlayerHit);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }

    public static void ReceivePlayerHit(BytesReader reader)
    {
        S2CPlayerHitMessage message = new S2CPlayerHitMessage();
        message.Message(reader);
    }

    public static void SendPlayerAddBuff(C2SPlayerAddBuffMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_PlayerAddBuff);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }


    public static void ReceivePlayerAddBuff(BytesReader reader)
    {
        S2CPlayerAddBuffMessage message = new S2CPlayerAddBuffMessage();
        message.Message(reader);
    }

    public static void SendPlayerRemoveBuff(C2SPlayerRemoveBuffMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_PlayerRemoveBuff);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }

    public static void ReceivePlayerRemoveBuff(BytesReader reader)
    {
        S2CPlayerRemoveBuffMessage message = new S2CPlayerRemoveBuffMessage();
        message.Message(reader);
    }

    public static void ReceivePlayerRefreshBuff(BytesReader reader)
    {
        S2CPlayerRefreshBuffMessage message = new S2CPlayerRefreshBuffMessage();
        message.Message(reader);
    }

    public static void SendPlayerDie(C2SPlayerDieMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_PlayerDie);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }

    public static void ReceivePlayerDie(BytesReader reader)
    {
        S2CPlayerDieMessage message = new S2CPlayerDieMessage();
        message.Message(reader);
    }

    public static void SendAddRoom(C2SAddRoomMessage message)
    {
        writer.Clear();
        writer.WriteByte((byte)C2SBattleProtocol.C2S_AddRoom);

        writer = message.Message(writer);

        BattleProtocol.SendBytes(writer);
    }

    public static void ReceiveAddRoom(BytesReader reader)
    {
        S2CAddRoomMessage message = new S2CAddRoomMessage();
        message.Message(reader);
    }
    
}