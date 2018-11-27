using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer
{
    /// <summary>
    /// 副本通关条件判断;
    /// </summary>
    public class VictoryCondition
    {
        private List<StarCondition> conditionsList = new List<StarCondition>();

        public bool isRun = false;
        private bool isVictory = false;

        private dungeon_s dungeonInfo = null;
        private BattleCore battleCore = null;

        public void AddConditions(List<StarCondition> victoryConditionsList)
        {
            List<StarCondition> tempList = new List<StarCondition>();

            for (int i = 0; i < victoryConditionsList.Count; i++)
            {
                tempList.Add(victoryConditionsList[i].Clone());
            }
            conditionsList.AddRange(tempList);
        }

        public List<StarCondition> GetConditions()
        {
            List<StarCondition> showConditionList = new List<StarCondition>();

            foreach (StarCondition item in conditionsList)
            {
                if (item.isShow == true)
                {
                    showConditionList.Add(item);
                }
            }

            return showConditionList;
        }

        public void Start(BattleCore battleCore)
        {
            this.battleCore = battleCore;
            this.dungeonInfo = battleCore.dungeonInfo;

            isRun = true;
            isVictory = false;

            conditionsList.Clear();

            AddConditions(dungeonInfo.victoryConditionsList);
            AddConditions(dungeonInfo.starConditionsList);

        }

        public void End()
        {

        }

        /// <summary>
        /// 获取几颗星;
        /// </summary>
        /// <returns></returns>
        public int GetStar()
        {
            int star = 0; //通关就算1星;

            for (int i = 0, max = conditionsList.Count; i < max; i++)
            {
                if (conditionsList[i].isVictory == true)
                {
                    star++;
                    continue;
                }

                if (conditionsList[i].isShow == false)
                {
                    star++;
                    continue;
                }

                if (conditionsList[i].isFinish == true)
                {
                    star++;
                    continue;
                }
            }
            return star;
        }

        public void OnDungeonRunTime(int dungeonRunTime)
        {
            if (isRun == false)
                return;
            
            if (dungeonRunTime > dungeonInfo.timeLimit)
            {
                GameFinishData.Instance.OnGameFinish(battleCore, GlobalData.pvpPlayerCamp);
                isRun = false;
                return;
            }

            CheckConditions(eStarConditionType.limitTime, dungeonRunTime);
            CheckConditions(eStarConditionType.existTime, dungeonRunTime);
        }

        public void OnUnitReduceLife(int uid)
        {
            if (isRun == false)
                return;

            GameUnit gameUnit = GameUnitManager.GetGameUnit(uid);

            CheckConditions(eStarConditionType.endElementHp, gameUnit);
            CheckConditions(eStarConditionType.protectMonster, gameUnit);

        }

        public void OnDungeonFinish()
        {
            if (isRun == false)
                return;

            CheckConditions(eStarConditionType.killAllMonster, null);
        }

        public void OnUnitDeath(int roomIndex, int atkUUID , int hitUUID)
        {
            if (isRun == false)
                return;

            GameUnit atkUnit = GameUnitManager.GetGameUnit(atkUUID);
            GameUnit hitUnit = GameUnitManager.GetGameUnit(hitUUID);

            bool isPlayerDeath = false;
            bool isFinish = true;

            if(battleCore.dungeonInfo.mapType != eMapType.pvpFightChaos)
            {
                if(atkUnit.unitType == GameUnitType.player)
                {
                    isPlayerDeath = true;
                }

                RoomBase roomBase = RoomManager.GetRoomBase(roomIndex);
                
                for (int i = 0, max = roomBase.netPlayerList.Count; i < max; i++)
                {
                    NetPlayer netPlayer = roomBase.netPlayerList[i];
                    
                    if (netPlayer.gameUnit.baseUnitData.camp == atkUnit.baseUnitData.camp)
                    {
                        if (isFinish == true && netPlayer.roomState == ePlayerRoomState.fighting)
                        {
                            isFinish = false;
                        }
                    }
                }

                if (isPlayerDeath == true && isFinish == true)
                {
                    GameFinishData.Instance.OnGameFinish(battleCore, (byte)atkUnit.baseUnitData.camp);
                    isRun = false;

                    return;
                }
            }

            CheckConditions(eStarConditionType.killMonster, hitUnit);
            CheckConditions(eStarConditionType.killPlayer, hitUnit);
            
        }

        public void OnGameUnitBirth(int uid)
        {
            GameUnit gameUnit = GameUnitManager.GetGameUnit(uid);
            CheckConditions(eStarConditionType.endElementHp, gameUnit);
        }


        private bool CheckConditions(eStarConditionType conditionType, object par)
        {
            bool isFnish = true;
            bool isVictory = true;
            byte loseCamp = 0;
            for (int i = 0, max = conditionsList.Count; i < max; i++)
            {
                StarCondition starCondition = conditionsList[i];

                if (conditionType == starCondition.starType)
                {
                    starCondition.isFinish = false;

                    switch (starCondition.starType)
                    {
                        case eStarConditionType.limitTime:          //1规定时间;

                            int maxLimitTime = starCondition.param.ToInt(0);
                            int curLimitTime = (int)par;

                            if (curLimitTime < maxLimitTime)
                            {
                                starCondition.isFinish = true;
                                loseCamp = GlobalData.pvpMonsterCamp;
                            }

                            starCondition.runParam = curLimitTime;
                            break;
                        case eStarConditionType.killMonster:        //2击杀指定怪物;

                            GameUnit killGameUnit = par as GameUnit;


                            string[] killParamList = starCondition.param.Split('|');   // param: 5|10001|10002

                            if (killParamList.Length < 2)
                            {
                                MyDebug.WriteLine("关卡配置表，关卡条件2击杀指定怪物通关条件参数配置不对");
                                break;
                            }

                            if (killParamList.Contains(killGameUnit.modleId.ToString()) == true)
                            {
                                starCondition.runParam++;

                                if (killParamList[0].ToInt(1000) <= starCondition.runParam)
                                {
                                    starCondition.isFinish = true;
                                    loseCamp = GlobalData.pvpMonsterCamp;
                                }
                            }

                            break;
                        case eStarConditionType.killAllMonster:     //3击杀所有怪物;

                            starCondition.isFinish = true;

                            starCondition.dungeonShowText = starCondition.showText;

                            break;
                        case eStarConditionType.existTime:          //4生存时间; (只能用于通关条件)

                            int maxExistTime = starCondition.param.ToInt(0);
                            int curExistTime = (int)par;

                            if (curExistTime > maxExistTime)
                            {
                                starCondition.isFinish = true;
                                loseCamp = GlobalData.pvpMonsterCamp;
                            }

                            starCondition.runParam = curExistTime;

                            break;
                        case eStarConditionType.endElementHp:        //5血量;

                            GameUnit elementGameUnit = par as GameUnit;

                            string[] elemenParamList = starCondition.param.Split('|');   // param：0|80  10001|80

                            if (elemenParamList.Length != 2)
                            {
                                MyDebug.WriteLine("关卡配置表，关卡条件5血量通关条件参数配置不对");
                                break;
                            }

                            if (elementGameUnit == null)
                            {
                                break;
                            }

                            float eh = (elementGameUnit.runUnitData.life * 1.0f / elementGameUnit.baseUnitData.life * 1.0f) * 100;
                            int maxEh = elemenParamList[1].ToInt(100);
                            if (eh > maxEh)
                            {
                                starCondition.isFinish = true;
                                loseCamp = GlobalData.pvpMonsterCamp;
                            }
                            break;
                        case eStarConditionType.protectMonster:     //6保护怪物血量;

                            GameUnit protectGameUnit = par as GameUnit;

                            string[] protectParamList = starCondition.param.Split('|');   // param:10001|80 

                            if (protectParamList.Length != 2)
                            {
                                MyDebug.WriteLine("关卡配置表，关卡条件6保护怪物血量通关条件参数配置不对");
                                break;
                            }

                            float pm = (protectGameUnit.runUnitData.life * 1.0f / protectGameUnit.baseUnitData.life * 1.0f) * 100;
                            int maxPm = protectParamList[1].ToInt(100);
                            if (pm > maxPm)
                            {
                                starCondition.isFinish = true;
                                loseCamp = GlobalData.pvpMonsterCamp;
                            }

                            break;
                        case eStarConditionType.ignoreMonster:      //7忽略怪物;

                            break;
                        case eStarConditionType.killPlayer:         //击杀指定敌人次数
                            {
                                GameUnit gameUnit = par as GameUnit;
                                if (starCondition.deathNum.ContainsKey(gameUnit.uuid) == false)
                                    starCondition.deathNum[gameUnit.uuid] = 0;

                                starCondition.deathNum[gameUnit.uuid]++;
                                int dethNum = starCondition.param.ToInt(0);

                                if (starCondition.deathNum[gameUnit.uuid] >= dethNum)
                                {
                                    starCondition.isFinish = true;
                                    loseCamp = (byte)gameUnit.baseUnitData.camp;
                                }
                            }
                            break;
                        case eStarConditionType.killCamp:       //击杀指定正营;
                            {
                                GameUnit playerGameUnit = par as GameUnit;
                                int camp = playerGameUnit.baseUnitData.camp;
                                if (starCondition.deathCamp.ContainsKey(camp) == false)
                                    starCondition.deathCamp[camp] = 0;

                                starCondition.deathCamp[camp]++;

                                int dethNum = starCondition.param.ToInt(0);
                                if (starCondition.deathCamp[camp] >= dethNum)
                                {
                                    starCondition.isFinish = true;
                                    loseCamp = (byte)camp;
                                }
                            }
                            break;
                    }
                }

                if (isFnish == true && starCondition.isFinish == false && starCondition.isVictory == true)
                {
                    isFnish = false;
                }
            }

            //游戏结束;
            if (isFnish == true)
            {
                isRun = false;

                this.isVictory = isVictory;

                GameFinishData.Instance.OnGameFinish(battleCore,loseCamp);
            }

            return isVictory;
        }
    }
}
