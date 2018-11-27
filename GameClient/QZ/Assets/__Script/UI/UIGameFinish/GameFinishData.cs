using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GameFinishData : SingleClass<GameFinishData>
{

    public void ReceiveGameFinish(S2CGameFinishMessage s2CGameFinish)
    {
        dungeon_c dungeon = dungeon_c.Get(s2CGameFinish.dungeonId);

        switch (dungeon.mapType)
        {
            case eMapType.pvpfight1V1:

                break;
            case eMapType.pvpFightChaos:

                break;
        }
    }
}