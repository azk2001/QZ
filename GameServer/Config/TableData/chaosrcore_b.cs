using System;
using System.Collections.Generic;

namespace GameServer
{
    public class chaosrcore_b
    {
        private static readonly Dictionary<int, chaosrcore_b> gInfoDic = new Dictionary<int, chaosrcore_b>();
        private static readonly List<chaosrcore_b> gInfoList = new List<chaosrcore_b>();

        public readonly int id;
        public readonly int minLevel;
        public readonly int maxLevel;
        public readonly int rcore;
        public readonly float combeRcore;


        private chaosrcore_b(tab_file_data.line_data file)
        {
            id = file.get_content_int("id");
            minLevel = file.get_content_int("minLevel");
            maxLevel = file.get_content_int("maxLevel");
            rcore = file.get_content_int("rcore");
            combeRcore = file.get_content_float("combeRcore");
        }


        public static chaosrcore_b Get(int id)
        {
            if (gInfoDic.ContainsKey(id))
                return gInfoDic[id];
            MyDebug.WriteLine("chaosrcore_b 未能找到id: " + id.ToString());
            return null;
        }

        public static chaosrcore_b GetChaosrcore(int level)
        {
            chaosrcore_b chaosrcore = null;

            for (int i = 0, max = gInfoList.Count; i < max; i++)
            {
                chaosrcore = gInfoList[i];

                if (level > chaosrcore.minLevel && level < chaosrcore.maxLevel)
                    return chaosrcore;
            }

            return null;
        }

        public static List<chaosrcore_b> GetList()
        {
            return gInfoList;
        }


        public static void LoadTxt(tab_file_data file)
        {
            List<tab_file_data.line_data> list = file.get_line_data();
            for (int i = 0, max = list.Count; i < max; i++)
            {
                tab_file_data.line_data data = list[i];
                int id = data.get_content_int("id");
                if (gInfoDic.ContainsKey(id))
                {
                    MyDebug.WriteLine("chaosrcore_b配置表id重复:" + id);
                }
                else
                {
                    chaosrcore_b info = new chaosrcore_b(data);
                    gInfoDic.Add(id, info);
                    gInfoList.Add(info);
                }
            }
        }
    }
}
