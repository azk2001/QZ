using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace GameServer
{
    public class ElementManager
    {
        public bool isFinishGame = false;       //游戏是否结束;

        public bool isPause = false;    //是否暂停;

        private Dictionary<int, ElementWave> allWave = new Dictionary<int, ElementWave>();  //当前关卡里面所有的原件波;
        private Dictionary<int, ElementGroup> allGroup = new Dictionary<int, ElementGroup>();  //当前关卡里面所有的原件组;

        private List<int> curRunWaveIdList = new List<int>();//当前运行中的原件波ID;
        private List<int> curFinishWaveIdList = new List<int>();//当前完成中的原件波ID;

        private List<int> curRunGroupIdList = new List<int>();//当前运行中的原件组ID;
        private List<int> curFinishGroupIdList = new List<int>();//当前完成中的原件组ID;

        private Dictionary<int, List<GameUnit>> curRunObjectList = new Dictionary<int, List<GameUnit>>();                       //当前运行的怪物的游戏对象; key = 波 ; vale : key=组
        private Dictionary<GameUnit, int> curRunObject = new Dictionary<GameUnit, int>();                                       //游戏对象存在的怪物波组;

        public BattleCore battleCore = null;   //当前这次战役类型
        public DateTime startTime;  //关卡开始的时间
        public int roomIndex; //当前房间的索引

        public List<WaveBirthTrigger> waveTriggerList = new List<WaveBirthTrigger>(); //波触发事件处理中心;
        public List<GroupBirthTrigger> groupTriggerList = new List<GroupBirthTrigger>(); //组触发事件处理中心;

        public void Init(BattleCore battleCore, int roomIndex)
        {
            allWave.Clear();
            allGroup.Clear();
            curRunWaveIdList.Clear();
            curFinishWaveIdList.Clear();
            curRunGroupIdList.Clear();
            curFinishGroupIdList.Clear();
            curRunObjectList.Clear();
            curRunObject.Clear();

            waveTriggerList.Clear();
            groupTriggerList.Clear();

            isPause = false;
            isFinishGame = false;

            this.startTime = DateTime.Now;
            this.battleCore = battleCore;
            this.roomIndex = roomIndex;
        }

        /// <summary>
        /// 心跳函数;
        /// </summary>
        /// <param name="dateTime"></param>
        public void Update(float deltaTime)
        {
            for (int i = 0, max = waveTriggerList.Count; i < max; i++)
            {
                waveTriggerList[i].Update(deltaTime);
            }

            for (int i = 0, max = groupTriggerList.Count; i < max; i++)
            {
                groupTriggerList[i].Update(deltaTime);
            }
        }

        /// <summary>
        /// 外部调用，出生怪物;
        /// </summary>
        /// <param name="wave">波</param>
        /// <param name="group">组</param>
        /// <param name="go">游戏物体</param>
        public void BirthElement(int wave, int group, GameUnit go)
        {
            int key = WaveGroupToInt(wave, group);

            if (curRunObjectList.ContainsKey(key) == false)
            {
                curRunObjectList[key] = new List<GameUnit>();
            }

            List<GameUnit> waveDic = curRunObjectList[key];
            waveDic.Add(go);
            curRunObject[go] = WaveGroupToInt(wave, group);
        }

        /// <summary>
        /// 外部调用，击杀怪物;
        /// </summary>
        /// <param name="go">游戏物体</param>
        public void DeadElement(GameUnit go)
        {
            if (isFinishGame == true)
                return;

            if (curRunObject.ContainsKey(go) == true)
            {
                int val = curRunObject[go];

                int wave = 0;
                int group = 0;

                IntToWaveGroup(val, ref wave, ref group);

                List<GameUnit> waveDic = curRunObjectList[val];

                waveDic.Remove(go);

                if (waveDic.Count < 1)
                {
                    OnEndGroup(wave, group);
                }

                bool isFinishWave = true;
                List<ElementGroup> elementGroupList = allWave[wave].elementGroupList;
                for (int i = 0, max = elementGroupList.Count; i < max; i++)
                {
                    ElementGroup elementGroup = elementGroupList[i];

                    //如果当前组标示忽略;
                    if (elementGroup.isNeglect == true)
                        continue;

                    int finishGroup = WaveGroupToInt(elementGroup.curWave, elementGroup.curGroup);

                    if (curFinishGroupIdList.Contains(finishGroup) == false)
                    {
                        isFinishWave = false;
                        break;
                    }
                }

                if (isFinishWave == true)
                {
                    OnEndWave(wave);

                    curRunObjectList.Remove(wave);
                }

                curRunObject.Remove(go);
            }
        }

        //设置原件的hp;
        public void SetElementHP(GameUnit go, int startHP, int curHP)
        {
            if (curRunObject.ContainsKey(go) == true)
            {
                int val = curRunObject[go];
                ElementGroup elementGroup = allGroup[val];
                elementGroup.curBossHP = (int)(curHP * 1.0f / startHP * 1.0f);
            }
        }

        //设置原件的波;
        public void SetWave(int wave, ElementWave elementWave)
        {
            allWave[wave] = elementWave;
        }

        //设置原件的组;
        public void SetGroup(int wave, int group, ElementGroup elementGroup)
        {
            allGroup[WaveGroupToInt(wave, group)] = elementGroup;
        }

        //获取当前组是否在运行;
        public bool GetRunGroupId(int wave, int group)
        {
            return curRunGroupIdList.Contains(WaveGroupToInt(wave, group));
        }

        //获取当前组是否完成;
        public bool GetFinishGroupId(int wave, int group)
        {
            return curFinishGroupIdList.Contains(WaveGroupToInt(wave, group));
        }

        //获取当前波是否在运行;
        public bool GetRunWaveId(int wave)
        {
            return curRunWaveIdList.Contains(wave);
        }

        //获取当前波是否完成;
        public bool GetFinishWaveId(int wave)
        {
            return curFinishWaveIdList.Contains(wave);
        }

        //获取当前波组的游戏对象;
        public List<GameUnit> GetObjectList(int wave, int group)
        {
            int id = WaveGroupToInt(wave, group);

            if (curRunObjectList.ContainsKey(id) == true)
            {
                return curRunObjectList[id];
            }
            return null;
        }

        //获取波信息;
        public ElementWave GetElementWave(int wave)
        {
            ElementWave elementWave = null;

            if (allWave.ContainsKey(wave))
                elementWave = allWave[wave];

            return elementWave;
        }

        //获取组信息;
        public ElementGroup GetElementGroup(int wave, int group)
        {
            ElementGroup elementGroup = null;

            if (allGroup.ContainsKey(WaveGroupToInt(wave, group)))
                elementGroup = allGroup[WaveGroupToInt(wave, group)];

            return elementGroup;
        }

        //开始执行当前波;
        public void OnStartWave(int wave)
        {
            if (curRunWaveIdList.Contains(wave) == false)
            {
                curRunWaveIdList.Add(wave);
            }

            allWave[wave].curBirthTime = DateTime.Now;
            allWave[wave].curLoopBirthTime = DateTime.Now;

            //启动这一波所有的怪物组 开始检测;
            List<ElementGroup> groupList = allWave[wave].elementGroupList;
            for (int i = 0, max = groupList.Count; i < max; i++)
            {
                ElementGroup tmpGroup = groupList[i];

                GroupBirthTrigger gbt = tmpGroup.triggerGameObject;
                gbt.SetElementTrigger(tmpGroup, this);
                gbt.isRun = true;
            }
        }

        //当前波结束;
        public void OnEndWave(int wave)
        {
            if (curRunWaveIdList.Contains(wave))
            {
                curRunWaveIdList.Remove(wave);
            }

            if (curFinishWaveIdList.Contains(wave) == false)
            {
                curFinishWaveIdList.Add(wave);
            }

            ElementWave elementWave = allWave[wave];
            elementWave.curEndTime = DateTime.Now;
            elementWave.triggerGameObject.isDetection = false;
        }

        //开始执行当前组; 
        public void OnStartGroup(int wave, int group)
        {
            int id = WaveGroupToInt(wave, group);

            if (curRunGroupIdList.Contains(id) == false)
            {
                curRunGroupIdList.Add(id);
            }

            ElementGroup elementGroup = allGroup[id];
            elementGroup.curBirthTime = DateTime.Now;
            elementGroup.curLoopBirthTime = DateTime.Now;

            switch (elementGroup.elementType)
            {
                case eElementType.monster:
                    {
                        battleCore.BirthMonsterEvent(elementGroup);
                    }
                    break;
                case eElementType.player:
                    {
                        battleCore.BirthPlayerEvent(elementGroup);
                    }
                    break;
                case eElementType.eventTrigger:
                    {
                        battleCore.BirthEventEvent(elementGroup);
                    }
                    break;
                case eElementType.obstruct:
                    {
                        battleCore.BirthObstructEvent(elementGroup);
                    }
                    break;
            }
        }

        //结束当前组;
        public void OnEndGroup(int wave, int group)
        {
            int id = WaveGroupToInt(wave, group);

            if (curRunGroupIdList.Contains(id))
            {
                curRunGroupIdList.Remove(id);
            }

            if (curFinishGroupIdList.Contains(id) == false)
            {
                curFinishGroupIdList.Add(id);
            }

            ElementGroup elementGroup = allGroup[id];

            elementGroup.curEndTime = DateTime.Now;
            elementGroup.curLoopEndTime = DateTime.Now;
            elementGroup.triggerGameObject.isDetection = false;

            bool isFinishGame = true;

            foreach (ElementGroup item in allGroup.Values)
            {
                int finishGroup = WaveGroupToInt(item.curWave, item.curGroup);

                if (item.elementType == eElementType.obstruct)
                    continue;

                if (item.elementType == eElementType.player)
                    continue;

                if (item.isNeglect == true)
                    continue;

                if (curFinishGroupIdList.Contains(finishGroup) == true)
                    continue;

                isFinishGame = false;

                break;
            }

            //如果已经完成的波和总波数一样就证明已经全部完成刷怪器;
            if (isFinishGame)
            {
                battleCore.victoryCondition.OnDungeonFinish();
            }
        }

        private int WaveGroupToInt(int wave, int group)
        {
            return wave * 100 + group;
        }

        private bool IntToWaveGroup(int val, ref int wave, ref int group)
        {
            wave = val / 100;
            group = val % 100;
            return true;
        }

    }
}

