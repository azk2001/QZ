using System;
using System.Collections.Generic;

namespace GameServer
{
    public enum eModleType
    {
        common = 1,
        elite = 2,
        boss = 3,
        player = 100,
    }

    public class monster_b
    {
        private static readonly Dictionary<int, monster_b> gInfoDic = new Dictionary<int, monster_b>();
        private static readonly List<monster_b> gInfoList = new List<monster_b>();

        public readonly int id;
        public readonly string name;
        public readonly int modleId;
        public readonly int camp;
        public readonly eModleType type;
        public readonly bool birthAnimation;
        public readonly int level;
        public readonly int horoscopeId;
        public readonly float battlePoint;
        public readonly int criticalAll;                 //暴击  
        public readonly int dodgeAll;                    //闪避
        public readonly int hitAll;                      //命中
        public readonly int antiCriticalAll;             //韧性
        public readonly int strikeAll;                   //打击力度（对护甲伤害）
        public readonly int armorAll;                    //抗打击值（总护甲值)
        public readonly int antiStrikeAll;               //总破甲抵抗值
        public readonly int damageRAll;                  //总伤害减免
        public readonly int shieldRecoverAll;            //护甲再生速度
        public readonly int shieldCdAll;                 //破甲回复时间
        public readonly int moveSpeed;                   //移动速度
        public readonly int aiId;
        public readonly List<int> skillIdList;
        public readonly int dropId;
        public readonly int showDropId;
        public readonly bool isActive;
        public readonly bool isInvincible;
        public readonly int size;
        public readonly bool isShowHP;
        public readonly int deathSkillId;
        public readonly string monsterIcon;
        public readonly int hpThickness;
        public readonly string cruiseDialogue;
        public readonly string skillDialogue;
        public readonly string birthDialogue;
        public readonly string deathDialogue;
        public readonly string runDialogue;
        public readonly float dialogueShowTime;
        public readonly float dialogueCD;


        public BattleUnitData dataBase = null;

        private monster_b(tab_file_data.line_data file)
        {
            id = file.get_content_int("id");
            name = file.get_content_str("name");
            modleId = file.get_content_int("modleId");
            camp = file.get_content_int("camp");
            type = (eModleType)file.get_content_int("type");
            birthAnimation = file.get_content_int("birthAnimation") == 1;
            level = file.get_content_int("level");
            horoscopeId = file.get_content_int("horoscopeId");
            battlePoint = file.get_content_float("battlePoint");
            criticalAll = file.get_content_int("criticalAll");
            dodgeAll = file.get_content_int("dodgeAll");
            hitAll = file.get_content_int("hitAll");
            antiCriticalAll = file.get_content_int("antiCriticalAll");
            strikeAll = file.get_content_int("strikeAll");
            armorAll = file.get_content_int("armorAll");
            antiStrikeAll = file.get_content_int("antiStrikeAll");
            damageRAll = file.get_content_int("damageRAll");
            shieldRecoverAll = file.get_content_int("shieldRecoverAll");
            shieldCdAll = file.get_content_int("shieldCdAll");
            moveSpeed = file.get_content_int("moveSpeed");
            aiId = file.get_content_int("aiId");

            skillIdList = new List<int>();
            string[] skillId = file.get_content_str("skillId").Split(';');
            for (int i = 0, max = skillId.Length; i < max; i++)
            {
                skillIdList.Add(skillId[i].ToInt(0));
            }

            dropId = file.get_content_int("dropId");
            showDropId = file.get_content_int("showDropId");
            isActive = file.get_content_int("isActive") == 1;
            isInvincible = file.get_content_int("isInvincible") == 1;
            size = file.get_content_int("size");
            isShowHP = file.get_content_int("isShowHP") == 1 ? true : false;
            deathSkillId = file.get_content_int("deathSkillId");
            monsterIcon = file.get_content_str("monsterIcon");
            hpThickness = file.get_content_int("hpThickness");
            cruiseDialogue = file.get_content_str("cruiseDialogue");
            skillDialogue = file.get_content_str("skillDialogue");
            birthDialogue = file.get_content_str("birthDialogue");
            deathDialogue = file.get_content_str("deathDialogue");
            runDialogue = file.get_content_str("runDialogue");
            dialogueShowTime = file.get_content_float("dialogueShowTime");
            dialogueCD = file.get_content_float("dialogueCD");

        }


        public static monster_b Get(int id)
        {
            if (gInfoDic.ContainsKey(id))
                return gInfoDic[id];
            MyDebug.WriteLine("monster_c 未能找到id: " + id.ToString());
            return null;
        }


        public static List<monster_b> GetList()
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
                    MyDebug.WriteLine("monster_c配置表id重复:" + id);
                }
                else
                {
                    monster_b info = new monster_b(data);
                    BattleUnitData dataBase = new BattleUnitData();
                    info.dataBase = dataBase;

                    gInfoDic.Add(id, info);
                    gInfoList.Add(info);
                }
            }
        }
    }
}
