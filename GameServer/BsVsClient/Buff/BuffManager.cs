using System.Collections;
using System.Collections.Generic;

namespace GameServer
{
    public class BuffManager
    {
        private static BuffManager _instance = null;
        public static BuffManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BuffManager();
                }

                return _instance;
            }
        }

        public List<int> buffList = new List<int>() {
          1, //速度变化;

        };


        public int RandomBuff()
        {
            return Random.Range(0, buffList.Count);
        }

    }
}

