using System;
using System.Collections.Generic;
namespace BattleServer
{
    public class NetPlayerManager
    {
        public static Dictionary<int, NetPlayer> netPlayerDic = new Dictionary<int, NetPlayer>(); //网络玩家总管理器;

        public static void AddNetPlayer(NetPlayer netPlayer)
        {
            netPlayerDic[netPlayer.uuid] = netPlayer;
        }

        public static void RemoveNetPlayer(NetPlayer netPlayer)
        {
            RemoveNetPlayer(netPlayer.uuid);
        }

        public static void RemoveNetPlayer(int uuid)
        {
            if (netPlayerDic.ContainsKey(uuid))
            {
                netPlayerDic.Remove(uuid);
            }
        }

        public static NetPlayer GetNetPlayer(int uuid)
        {
            if (netPlayerDic.ContainsKey(uuid))
            {
                return netPlayerDic[uuid];
            }
            return null;
        }
        public static List<NetPlayer> GetNetPlayers()
        {
            return new List<NetPlayer>(netPlayerDic.Values);
        }
    }
}
