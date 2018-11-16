using System;
using System.Collections.Generic;

namespace GameServer
{
    public class monsterrevise_b
    {
        private static readonly Dictionary<int, monsterrevise_b> gInfoDic = new Dictionary<int, monsterrevise_b>();
        private static readonly List<monsterrevise_b> gInfoList = new List<monsterrevise_b>();
        private static readonly Dictionary<int, List<monsterrevise_b>> gInfoDicList = new Dictionary<int, List<monsterrevise_b>>();

        public readonly int index;
        public readonly int mapid;
        public readonly int level;
        public readonly float resvise;

        public static int maxLevel = 0;

        private monsterrevise_b(tab_file_data.line_data file)
        {
            index = file.get_content_int("index");
            mapid = file.get_content_int("mapid");
            level = file.get_content_int("level");
            resvise = file.get_content_float("resvise");

            if (maxLevel < level) maxLevel = level;
        }


        public static monsterrevise_b Get(int index)
        {
            if (gInfoDic.ContainsKey(index))
                return gInfoDic[index];
            MyDebug.WriteLine("monster_revise_b 未能找到id: " + index.ToString());
            return null;
        }

        public static monsterrevise_b GetMapLevel(int mapId,int level)
        {
            if (gInfoDicList.ContainsKey(mapId))
            {
                List<monsterrevise_b> list = gInfoDicList[mapId];

                for (int i = 0,max = list.Count; i < max; i++)
                {
                    monsterrevise_b monsterrevise = list[i];
                    if(monsterrevise.level == level)
                    {
                        return monsterrevise;
                    }
                }
            }

            MyDebug.WriteLine("monster_revise_b 未能找到id: " + mapId.ToString());

            return null;
        }
        

        public static List<monsterrevise_b> GetList()
        {
            return gInfoList;
        }


        public static void LoadTxt(tab_file_data file)
        {
            List<tab_file_data.line_data> list = file.get_line_data();
            for (int i = 0, max = list.Count; i < max; i++)
            {
                tab_file_data.line_data data = list[i];
                int id = data.get_content_int("index");
                if (gInfoDic.ContainsKey(id))
                {
                    MyDebug.WriteLine("monster_revise_b配置表id重复:" + id);
                }
                else
                {
                    monsterrevise_b info = new monsterrevise_b(data);
                    gInfoDic.Add(id, info);
                    gInfoList.Add(info);

                    if(gInfoDicList.ContainsKey(info.mapid) ==false)
                    {
                        gInfoDicList[info.mapid] = new List<monsterrevise_b>();
                    }

                    gInfoDicList[info.mapid].Add(info);
                }
            }
        }
    }
}
