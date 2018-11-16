using System;
using System.Collections;
using System.Collections.Generic;

namespace GameServer
{
    //脚本开启 来计算是否触发;
    public class GroupBirthTrigger
    {

        private float dataTime = 0;
        private float frequency = 0.2f;
        private ElementGroup group = null;
        public bool isDetection = true;

        public bool isRun = false;

        private ElementManager elementManager = null;

        public void Start()
        {
            isDetection = false;
        }

        public void Update(float deltaTime)
        {
            if (isRun == false)
                return;

            //if (ElementManager.Instance.isPause == true)
            //    return;

            dataTime += deltaTime;
            if (dataTime < frequency)
                return;

            dataTime = 0;

            if (elementManager.isFinishGame == true || isDetection == false)
                return;

            if (IsFinish() == false)
                return;

            CommunicateFinish();
        }

        public void SetElementTrigger(ElementGroup group, ElementManager elementManager)
        {
            isDetection = true;
            isRun = true;

            dataTime = 0;

            this.group = group;
            this.elementManager = elementManager;
        }


        private bool IsFinish()
        {
            bool flag = false;

            bool isLastRun = elementManager.GetRunGroupId(group.curWave, group.lastGroup);
            bool isLastFinish = elementManager.GetFinishGroupId(group.curWave, group.lastGroup);

            bool isLastGroup = elementManager.GetElementGroup(group.curWave, group.lastGroup) != null;

            //判断怪物的类型;
            switch (group.elementType)
            {
                case eElementType.monster:      //出生怪物;
                case eElementType.player:
                case eElementType.eventTrigger:
                case eElementType.obstruct:
                    {

                        switch (group.elementTriggerType)
                        {
                            case eElementTriggerType.curOutTime:    //当前波下面只能有这么一组怪物
                                                                    //当前波死完的时间+配置延迟的时间<现在的游戏时间;
                                
                                if ((DateTime.Now - group.curLoopEndTime).TotalSeconds > group.curOutTime)
                                {
                                    flag = true;
                                    //重置这次结束的时间;
                                    group.curLoopEndTime = DateTime.Now;
                                }
                                break;
                            case eElementTriggerType.curStartTime:   //当前波下面只能有这么一组怪物
                                                                     //当前波死完的时间+配置延迟的时间<现在的游戏时间;
                                double time = (DateTime.Now - group.curLoopEndTime).TotalSeconds;
                                if (time > group.curStartTime)
                                {
                                    flag = true;
                                    //重置这次开始的时间;
                                    group.curLoopBirthTime = DateTime.Now;
                                }
                                break;
                            case eElementTriggerType.lastElementNum:

                                if (isLastGroup)
                                {
                                    //获取怪物组是否运行;
                                    if (isLastRun)
                                    {
                                        List<GameUnit> elementObject = elementManager.GetObjectList(group.curWave, group.lastGroup);
                                        if (elementObject != null)
                                        {
                                            if (group.lastElementNum >= elementObject.Count)
                                            {
                                                flag = true;
                                            }
                                        }
                                    }

                                    if(isLastFinish == true)
                                    {
                                        flag = true;
                                    }
                                }
                                else
                                {
                                    flag = true;
                                }
                                break;
                            case eElementTriggerType.lastOutTime:
                                if (isLastGroup)
                                {
                                    //判断上一波是否完成;
                                    if (isLastFinish)
                                    {
                                        ElementGroup lastGroup = elementManager.GetElementGroup(group.curWave, group.lastGroup);
                                        //上一波死完的时间+配置延迟的时间<现在的游戏时间;
                                        if ((DateTime.Now - lastGroup.curEndTime).TotalSeconds > group.lastOutTime )
                                        {
                                            flag = true;
                                        }
                                    }
                                }
                                else
                                {
                                    flag = true;
                                }
                                break;
                            case eElementTriggerType.lastBossHp:

                                if (isLastGroup)
                                {
                                    ElementGroup lastGroup = elementManager.GetElementGroup(group.curWave, group.lastGroup);
                                    if (lastGroup.elementType == eElementType.monster)
                                    {
                                        //配置的bossHP > boss现在的HP
                                        if (group.lastBossHp > lastGroup.curBossHP)
                                        {
                                            flag = true;
                                        }
                                    }
                                }
                                else
                                {
                                    flag = true;
                                }

                                break;
                            case eElementTriggerType.lastStartTime:
                                if (isLastGroup)
                                {
                                    ElementGroup lastGroup = elementManager.GetElementGroup(group.curWave, group.lastGroup);
                                    //上一波死完的时间+配置延迟的时间<现在的游戏时间;
                                    if ((DateTime.Now - lastGroup.curBirthTime).TotalSeconds > group.lastStartTime)
                                    {
                                        flag = true;
                                    }
                                }
                                else
                                {
                                    flag = true;
                                }
                                break;
                        }
                        break;
                    }
            }

            return flag;
        }

        public void CommunicateFinish()
        {
            if (group.loopNum > 0)
            {
                elementManager.OnStartGroup(group.curWave, group.curGroup);
            }

            //判断循环几次怪物;
            group.loopNum--;
            if (group.loopNum <= 0)
            {
                isRun = false;
                isDetection = false;
            }
        }
    }
}

