using System.Collections;
using System.Collections.Generic;

namespace GameServer
{

    public class GameFinishData : SingleTon_Class<GameFinishData>
    {

        private List<string> winUUIDList = new List<string>();
        private List<string> loseUUIDList = new List<string>();

        public void OnGameFinish(BattleCore battleCore, byte loseCamp)
        {

            S2CGameFinishMessage s2CGameFinish = new S2CGameFinishMessage();
            s2CGameFinish.dungeonId = battleCore.dungeonInfo.mapId;
            s2CGameFinish.loseCamp = loseCamp;
            s2CGameFinish.roomIndex = battleCore.roomIndex;

            switch (battleCore.dungeonInfo.mapType)
            {
                case eMapType.pvpfight1V1:



                    break;
                case eMapType.pvpFightChaos:

                    break;
            }

            BattleProtocolEvent.SendGameFinish(s2CGameFinish);
        }
    }
}


