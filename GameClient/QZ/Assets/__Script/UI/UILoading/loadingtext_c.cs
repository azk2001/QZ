using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class loadingtext_c
    {
        private static readonly Dictionary<int, loadingtext_c> gInfoDic = new Dictionary<int, loadingtext_c>();
        private static readonly List<loadingtext_c> gInfoList = new List<loadingtext_c>();

        public readonly int id;
        public readonly int minLevel;
        public readonly string text;


        private loadingtext_c( tab_file_data.line_data file)
        {
            id= file.get_content_int("id");
            minLevel= file.get_content_int("minLevel");
            text= file.get_content_str("text");
        }


        public static loadingtext_c Get(int id)
        {
            if (gInfoDic.ContainsKey(id))
                return gInfoDic[id];

            Debug.LogError("loadingtext_c 未能找到id: " +id.ToString());

            return null;
        }


        public static List<loadingtext_c> GetList()
        {
            return gInfoList;
        }

        public static List<loadingtext_c> GetList(int level)
        {
            List<loadingtext_c> list = new List<loadingtext_c>();

            for (int i = 0,max = gInfoList.Count; i < max; i++)
            {
                loadingtext_c loadingtext = gInfoList[i];

                if(loadingtext.minLevel<= level)
                {
                    list.Add(loadingtext);
                }
            }

            return list;
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
                     Debug.LogError("loadingtext_c配置表id重复:"+id);
                }
                else
                {
                    loadingtext_c info = new loadingtext_c(data);
                    gInfoDic.Add(id,info);
                    gInfoList.Add(info);
                }
            }
        }
    }
}
