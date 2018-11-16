using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer
{
    public delegate void VoidBoolDelegate(bool flag);
    public delegate void Callback<T>(T arg1);

    public class GlobalData
    {
        public static byte pvpPlayerCamp = 1;  //组队副本玩家的正营ID;
        public static byte pvpMonsterCamp = 2; //组队副本怪物的正营ID;

    }
}
