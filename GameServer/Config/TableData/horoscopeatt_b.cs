using System;
using System.Collections.Generic;

namespace BattleServer
{
    public class horoscopeatt_b
    {
        private static readonly Dictionary<int, horoscopeatt_b> gInfoDic = new Dictionary<int, horoscopeatt_b>();
        private static readonly List<horoscopeatt_b> gInfoList = new List<horoscopeatt_b>();

        public readonly int id;
        public readonly string desc;
        public readonly int pAttackAll;      //外攻击参数
        public readonly int sAttackAll;     //内攻击参数
        public readonly int pDefenceAll;    //外防御参数
        public readonly int sDefenceAll;    //内防御参数
        public readonly int lifeAll;        //生命参数


        private horoscopeatt_b( tab_file_data.line_data file)
        {
            id= file.get_content_int("id");
            desc= file.get_content_str("desc");
            pAttackAll= file.get_content_int("pAttackAll");
            sAttackAll= file.get_content_int("sAttackAll");
            pDefenceAll= file.get_content_int("pDefenceAll");
            sDefenceAll= file.get_content_int("sDefenceAll");
            lifeAll= file.get_content_int("lifeAll");
        }


        public static horoscopeatt_b Get(int id)
        {
            if (gInfoDic.ContainsKey(id))
                return gInfoDic[id];
            MyDebug.WriteLine("horoscopeatt_c 未能找到id: " +id.ToString());
            return null;
        }


        public static List<horoscopeatt_b> GetList()
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
                     MyDebug.WriteLine("horoscopeatt_c配置表id重复:"+id);
                }
                else
                {
                    horoscopeatt_b info = new horoscopeatt_b(data);
                    gInfoDic.Add(id,info);
                    gInfoList.Add(info);
                }
            }
        }
    }
}
