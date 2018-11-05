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


        private loadingtext_c( TabFileData.LineData file)
        {
            id= file.GetContentInt("id");
            minLevel= file.GetContentInt("minLevel");
            text= file.GetContentStr("text");
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

        


        public static void LoadTxt(TabFileData file)
        {
            List<TabFileData.LineData> list = file.GetLineData();
            for (int i = 0, max = list.Count; i < max; i++)
            {
                TabFileData.LineData data = list[i];
                int id = data.GetContentInt("id");
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
