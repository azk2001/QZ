using System;
using System.Collections.Generic;

namespace GameServer
{
    public class battleroominit_b
    {
        private static readonly Dictionary<int, battleroominit_b> gInfoDic = new Dictionary<int, battleroominit_b>();
        private static readonly List<battleroominit_b> gInfoList = new List<battleroominit_b>();

        public readonly int id;
        public readonly int mapType;
        public readonly int roomNum;
        public readonly int minLevel;
        public readonly int maxLevel;
        public readonly int roomLevel;


        private battleroominit_b( tab_file_data.line_data file)
        {
            id= file.get_content_int("id");
            mapType= file.get_content_int("mapType");
            roomNum= file.get_content_int("roomNum");
            minLevel= file.get_content_int("minLevel");
            maxLevel= file.get_content_int("maxLevel");
            roomLevel= file.get_content_int("roomLevel");
        }


        public static battleroominit_b Get(int id)
        {
            if (gInfoDic.ContainsKey(id))
                return gInfoDic[id];
            MyDebug.WriteLine("battleroominit 未能找到id: " +id.ToString());
            return null;
        }


        public static List<battleroominit_b> GetList()
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
                     MyDebug.WriteLine("battleroominit配置表id重复:"+id);
                }
                else
                {
                    battleroominit_b info = new battleroominit_b(data);
                    gInfoDic.Add(id,info);
                    gInfoList.Add(info);
                }
            }
        }
    }
}
