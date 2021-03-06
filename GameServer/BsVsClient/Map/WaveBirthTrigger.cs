using System;
using System.Collections;
using System.Collections.Generic;

namespace GameServer
{
    //脚本开启 来计算是否触发;
    public class WaveBirthTrigger
    {
        private float dataTime = 0;
        private float mFrequency = 0.2f;
        private ElementWave wave = null;

        public bool isDetection = true;

        public bool isRun = false;

        public ElementManager elementManager = null;  //当前关卡事件中心

        public void Init()
        {
            isDetection = false;
        }

        public void Update(float deltaTime)
        {
            if (isRun == false)
                return;

            if (elementManager.isPause == true)
                return;

            dataTime += deltaTime;

            if (dataTime < mFrequency)
            {
                return;
            }

            dataTime = 0;

            if (elementManager.isFinishGame == true || isDetection == false)
                return;

            if (IsFinish() == false)
                return;

            CommunicateFinish();
        }

        public void SetElementTrigger(ElementWave wave, ElementManager elementManager)
        {
            isDetection = true;

            this.elementManager = elementManager;
            this.wave = wave;

        }

        private bool IsFinish()
        {
            bool flag = false;

            ElementWave elementWave = elementManager.GetElementWave(wave.curWave);
            ElementWave lastWave = elementManager.GetElementWave(wave.sponsorWave);

            //判断上一个波是否存在; 没有存在证明没有上一波的关联 直接触发;
            bool hasGroup = elementManager.GetElementGroup(wave.sponsorWave, wave.sponsorGroup) != null;

            switch (elementWave.elementTriggerType)
            {

                case eWaveTriggerType.radius:
                    {
                        if (hasGroup == true)
                        {

                            bool isRun = elementManager.GetRunGroupId(wave.sponsorWave, wave.sponsorGroup);
                            if (isRun == true)
                            {
                                List<GameUnit> elementList = elementManager.GetObjectList(wave.sponsorWave, wave.sponsorGroup);
                                if (elementList != null)
                                {
                                    for (int i = 0, max = elementList.Count; i < max; i++)
                                    {
                                        GameUnit gameUnit = elementList[i];
                                        if (gameUnit != null)
                                        {
                                            if (Vector3.SqrMagnitude(elementWave.transform.position - gameUnit.transform.position) < elementWave.radius * elementWave.radius)
                                            {
                                                flag = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    break;
                case eWaveTriggerType.lastStartTime:
                    {
                        if (hasGroup == true)
                        {
                            ElementGroup elementGroup = elementManager.GetElementGroup(wave.sponsorWave, wave.sponsorGroup);
                            if ((DateTime.Now - elementGroup.curBirthTime).TotalSeconds > elementWave.lastStartTime)
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    break;
                case eWaveTriggerType.lastOutTime:
                    {
                        if (hasGroup == true)
                        {
                            ElementGroup elementGroup = elementManager.GetElementGroup(wave.sponsorWave, wave.sponsorGroup);
                            if ((DateTime.Now - elementGroup.curEndTime).TotalSeconds > elementWave.lastOutTime)
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            flag = true;
                        }
                    }

                    break;
            }
            return flag;
        }

        private void CommunicateFinish()
        {
            elementManager.OnStartWave(wave.curWave);

            wave.loopNum--;
            if (wave.loopNum <= 0)
            {
                isDetection = false;
            }
        }
    }
}