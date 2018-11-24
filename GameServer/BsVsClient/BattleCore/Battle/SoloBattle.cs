using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class SoloBattle : BattleCore
    {

        public override void Init(int dungeonId, int roomIndex)
        {
            base.Init(dungeonId, roomIndex);
        }

        public override void InScene(NetPlayer netPlayer)
        {
            base.InScene(netPlayer);

            ElementParam elementList = playerElement.elementParamList[netPlayerList.Count - 1];
            netPlayer.gameUnit.position = elementList.transform.position;

            netPlayer.basicsData.px = (int)(netPlayer.gameUnit.position.x * 100);
            netPlayer.basicsData.py = (int)(netPlayer.gameUnit.position.y * 100);

            netPlayer.gameUnit.Init(netPlayer.battleUnitData, netPlayer.basicsData);
        }

        public override void LeaveScene(NetPlayer netPlayer)
        {

            base.LeaveScene(netPlayer);
        }

        public override void End()
        {
            base.End();
        }
    }
}
