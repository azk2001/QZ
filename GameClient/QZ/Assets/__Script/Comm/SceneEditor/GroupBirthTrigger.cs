using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//脚本开启 来计算是否触发;
public class GroupBirthTrigger : MonoBehaviour
{

    private float dataTime = 1;
    private float frequency = 0.2f;
    private ElementGroup group = null;
    private bool isDetection = true;

    public bool isRun = false;

    void Start()
    {

    }
    void Update()
    {
        if (isRun == false)
            return;

        //if (ElementManager.Instance.isPause == true)
        //    return;

        dataTime += Time.deltaTime;
        if (dataTime < frequency)
            return;

        dataTime = 0;

        if (ElementManager.Instance.isFinishGame == true || isDetection == false)
            return;

        if (IsFinish() == false)
            return;

        CommunicateFinish();
    }

    public void SetElementTrigger(ElementGroup group)
    {
        isDetection = true;
        isRun = true;

        dataTime = 0;

        this.group = group;

        enabled = true;
    }


    private bool IsFinish()
    {
        bool flag = false;

        bool isLastRun = ElementManager.Instance.GetRunGroupId(group.curWave, group.lastGroup);
        bool isLastFinish = ElementManager.Instance.GetFinishGroupId(group.curWave, group.lastGroup);

        bool isLastGroup = ElementManager.Instance.GetElementGroup(group.curWave, group.lastGroup) != null;

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
                            if (group.curLoopEndTime + group.curOutTime < Time.time)
                            {
                                flag = true;
                                //重置这次结束的时间;
                                group.curLoopEndTime = 1000000;
                            }
                            break;
                        case eElementTriggerType.curStartTime:   //当前波下面只能有这么一组怪物
                            //当前波死完的时间+配置延迟的时间<现在的游戏时间;
                            if (group.curLoopBirthTime + group.curStartTime < Time.time)
                            {
                                flag = true;
                                //重置这次开始的时间;
                                group.curLoopBirthTime = (int)(group.curStartTime + Time.time);
                            }
                            break;
                        case eElementTriggerType.lastElementNum:

                            if (isLastGroup)
                            {
                                //获取怪物组是否运行;
                                if (isLastRun)
                                {
                                    List<GameObject> elementObject = ElementManager.Instance.GetObjectList(group.curWave, group.lastGroup);
                                    if (elementObject != null)
                                    {
                                        if (group.lastElementNum >= elementObject.Count)
                                        {
                                            flag = true;
                                        }
                                    }
                                }

                                if(isLastFinish ==true)
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
                                    ElementGroup lastGroup = ElementManager.Instance.GetElementGroup(group.curWave, group.lastGroup);
                                    //上一波死完的时间+配置延迟的时间<现在的游戏时间;
                                    if (lastGroup.curEndTime + group.lastOutTime < Time.time)
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
                                ElementGroup lastGroup = ElementManager.Instance.GetElementGroup(group.curWave, group.lastGroup);
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
                                ElementGroup lastGroup = ElementManager.Instance.GetElementGroup(group.curWave, group.lastGroup);
                                //上一波死完的时间+配置延迟的时间<现在的游戏时间;
                                if (lastGroup.curBirthTime + group.lastStartTime < Time.time)
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
        if (group == null)
            return;

        if (group.loopNum > 0)
        {
            ElementManager.Instance.OnStartGroup(group.curWave, group.curGroup);
        }

        //判断循环几次怪物;
        group.loopNum--;
        if (group.loopNum <= 0)
        {
            isRun = false;
            isDetection = false;
            enabled = false;
        }
    }
}
