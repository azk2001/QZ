using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GameServer
{
    public class TableManager : SingleTon_Class<TableManager>
    {
        private List<List<string>> loadList = new List<List<string>>
        {
                new List<string>(){ "dungeon_s", "1"},                
        };

        public void Init()
        {
            AnalysisTable();
        }


        private void AnalysisTable()
        {
            string tablePath = System.IO.Directory.GetCurrentDirectory() + "/Config/Config/";


            for (int i = 0, max = loadList.Count; i < max; i++)
            {
                List<string> strList = loadList[i];

                string filePath = tablePath + strList[0] + ".txt";

                string allText = File.ReadAllText(filePath, Encoding.UTF8);

                TabFileData data = TabFile.Parse(allText, strList[1].ToInt(2), strList[0]);

                LoadTxt(strList[0], data);
            }

        }


        public void LoadTxt(string name, TabFileData data)
        {
            switch (name)
            {
                case "dungeon_s":
                    dungeon_s.LoadTxt(data);
                    break;
            }

        }
    }
}


