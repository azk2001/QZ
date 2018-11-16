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
                new List<string>(){ "horoscopeatt_b", "1"},
                new List<string>(){ "dungeon_b", "1"},
                new List<string>(){ "monster_b", "1"},
                new List<string>(){ "monsterrevise_b", "1"},
                new List<string>(){ "battleroominit_b", "1"},
                new List<string>(){ "chaosrcore_b", "1"},
                new List<string>(){ "chaoticdata_b", "1"},
                
        };

        public void Init()
        {

        }


        private void AnalysisTable()
        {
            string tablePath = System.IO.Directory.GetCurrentDirectory() + "/Config/Txt/";


            for (int i = 0, max = loadList.Count; i < max; i++)
            {
                List<string> strList = loadList[i];

                string filePath = tablePath + strList[0] + ".txt";

                string allText = File.ReadAllText(filePath, Encoding.UTF8);

                tab_file_data data = tab_file.parse(allText, strList[1].ToInt(2), strList[0]);

                LoadTxt(strList[0], data);
            }

        }


        public void LoadTxt(string name, tab_file_data data)
        {
            switch (name)
            {
                case "horoscopeatt_b":
                    horoscopeatt_b.LoadTxt(data);
                    break;
                case "dungeon_b":
                    dungeon_b.LoadTxt(data);
                    break;
                case "monster_b":
                    monster_b.LoadTxt(data);
                    break;
                case "monsterrevise_b":
                    monsterrevise_b.LoadTxt(data);
                    break;
                case "battleroominit_b":
                    battleroominit_b.LoadTxt(data);
                    break;
                case "chaosrcore_b":
                    chaosrcore_b.LoadTxt(data);
                    break;
                case "chaoticdata_b":
                    chaoticdata_b.LoadTxt(data);
                    break;
            }

        }
    }
}


