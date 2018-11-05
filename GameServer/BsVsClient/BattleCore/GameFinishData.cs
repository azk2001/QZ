using System.Collections;
using System.Collections.Generic;

namespace BattleServer
{
    public class GameFinishParam
    {
        public byte loseCamp = 0;
        public int star = 0;
        // public List<ItemRes> rewardList = null;
    }


    public class LadderFinishData
    {
        public int serverId;
        public byte isWin;
        public string uuid;
        public string name;
        public string rivalName;
    }


    public class GameFinishData : SingleTon_Class<GameFinishData>
    {
        public GameFinishParam finishParam = new GameFinishParam();

        private List<string> winUUIDList = new List<string>();
        private List<string> loseUUIDList = new List<string>();
        private List<LadderFinishData> ladderFinishList = new List<LadderFinishData>();

        public void OnGameFinish(BattleCore battleCore, byte loseCamp)
        {
          
        }
    }
}


