using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GameMain
{

    public class TableManager : SingleTon_Class<TableManager>
    {

        //其实界面加载配置表
        private static Dictionary<string, int> loadStartList = new Dictionary<string, int>()
        {
            {"itemchangeway_c", 1},
            {"horoscopeatt_c",1 },
            {"createname_c",1 },
           
        };

        private VoidVoidDelegate loadFinish = null;

        private bool isLoadGameList = false;

        public void Init(VoidVoidDelegate loadFinish)
        {
            this.loadFinish = loadFinish;
            isLoadGameList = false;
        }

        private void OnLoadFinished(AssetBundleRequest bundleRequest, object[] pars)
        {
            textAssetList = bundleRequest.allAssets;

            AnalysisStartTable();
            
        }

        private UnityEngine.Object[] textAssetList = null;
        private int index = 0;

        public void AnalysisStartTable()
        {
            index = 0;

            for (int i = 0, max = textAssetList.Length; i < max; i++)
            {
                string name = textAssetList[i].name;

                if (loadStartList.ContainsKey(name) == true)
                {
                    TextAsset textAsset = textAssetList[i] as TextAsset;

                    TabFileData data = TabFile.parse(textAsset.text, loadStartList[name], name);

                    LoadTxt(name, data);

                    index++;
                }

                if (index >= loadStartList.Count)
                {
                    AnalysisGameTable();
                    break;
                }
            }
        }

        public void AnalysisGameTable()
        {
            if (isLoadGameList == true)
                return;

            isLoadGameList = true;

            index = 0;
        }

        public void LoadTxt(string name, TabFileData data)
        {
            switch (name)
            {
                case "createname_c":
                   
                    break;
            }

        }
    }
}