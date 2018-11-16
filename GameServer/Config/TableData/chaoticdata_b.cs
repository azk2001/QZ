using System;
using System.Collections.Generic;

namespace GameServer
{
    public class chaoticdata_b
    {
        private static readonly Dictionary<string, chaoticdata_b> gInfoDic = new Dictionary<string, chaoticdata_b>();
        private static readonly List<chaoticdata_b> gInfoList = new List<chaoticdata_b>();

        public readonly string key;
        public readonly string value;


        private chaoticdata_b( tab_file_data.line_data file)
        {
            key= file.get_content_str("key");
            value= file.get_content_str("value");
        }


        public static chaoticdata_b Get(string key)
        {
            if (gInfoDic.ContainsKey(key))
                return gInfoDic[key];
            MyDebug.WriteLine("chaoticdata_b 未能找到id: " +key.ToString());
            return null;
        }


        public static List<chaoticdata_b> GetList()
        {
            return gInfoList;
        }


        public static void LoadTxt(tab_file_data file)
        {
            List<tab_file_data.line_data> list = file.get_line_data();
            for (int i = 0, max = list.Count; i < max; i++)
            {
                tab_file_data.line_data data = list[i];
                string id = data.get_content_str("key");
                if (gInfoDic.ContainsKey(id))
                {
                     MyDebug.WriteLine("chaoticdata_b配置表id重复:"+id);
                }
                else
                {
                    chaoticdata_b info = new chaoticdata_b(data);
                    gInfoDic.Add(id,info);
                    gInfoList.Add(info);
                }
            }
        }
    }
}
