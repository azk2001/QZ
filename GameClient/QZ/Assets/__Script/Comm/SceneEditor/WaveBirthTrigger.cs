using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//脚本开启 来计算是否触发;
public class WaveBirthTrigger : MonoBehaviour
{
    private float dataTime = 1;
    private float mFrequency = 0.2f;
    private ElementWave wave = null;

    private bool isDetection = true;

    public bool isRun = false;

    void Start()
    {

    }
    void Update()
    {
        if (isRun == false)
            return;

        if (ElementManager.Instance.isPause == true)
            return;

        dataTime += Time.deltaTime;
        if (dataTime < mFrequency)
        {
            return;
        }

        dataTime = 0;

        if (ElementManager.Instance.isFinishGame == true || isDetection == false)
            return;

        if (IsFinish() == false)
            return;

        CommunicateFinish();
    }

    public void SetElementTrigger(ElementWave wave)
    {
        isDetection = true;

        this.wave = wave;
        switch (wave.elementTriggerType)
        {
            case eWaveTriggerType.trigger://trigger触发;
                Rigidbody r = this.gameObject.GetComponent<Rigidbody>(); ;
                if (r == null)
                {
                    r = this.gameObject.AddComponent<Rigidbody>();
                }
                r.useGravity = false;

                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (isRun == false)
            return;

        if (wave.elementTriggerType != eWaveTriggerType.trigger)
            return;

        if (ElementManager.Instance.isFinishGame == true || isDetection == false)
            return;

        if (IsFinish())
            return;

        if (ElementManager.Instance.isPause == true)
            return;

        List<GameObject> elementList = ElementManager.Instance.GetObjectList(wave.sponsorWave, wave.sponsorGroup);
        if (elementList != null)
        {
            for (int i = 0, max = elementList.Count; i < max; i++)
            {
                if (collider.gameObject == elementList[i])
                {
                    CommunicateFinish();
                }
            }
        }

    }

    private bool IsFinish()
    {
        bool flag = false;

        ElementWave elementWave = ElementManager.Instance.GetElementWave(wave.curWave);
        ElementWave lastWave = ElementManager.Instance.GetElementWave(wave.sponsorWave);

        //判断上一个波是否存在; 没有存在证明没有上一波的关联 直接触发;
        bool hasGroup = ElementManager.Instance.GetElementGroup(wave.sponsorWave, wave.sponsorGroup) != null;

        switch (elementWave.elementTriggerType)
        {
            //case eWaveTriggerType.trigger:
            //    //判断上一个波是否存在; 没有存在证明没有上一波的关联 直接触发;
            //    if (isLastWave)
            //    {
            //        flag = ElementManager.Instance.GetFinishWaveId(elementWave.lastWave);
            //    }
            //    else
            //    {
            //        flag = true;
            //    }
            //    break;
            case eWaveTriggerType.radius:
                {
                    if (hasGroup == true)
                    {
                        //bool isFinish = ElementManager.Instance.GetFinishGroupId(wave.sponsorWave, wave.sponsorGroup);

                        //if (isFinish == true)
                        //{
                        //    flag = true;
                        //    break;
                        //}

                        bool isRun = ElementManager.Instance.GetRunGroupId(wave.sponsorWave, wave.sponsorGroup);
                        if (isRun == true)
                        {
                            List<GameObject> elementList = ElementManager.Instance.GetObjectList(wave.sponsorWave, wave.sponsorGroup);
                            if (elementList != null)
                            {
                                for (int i = 0, max = elementList.Count; i < max; i++)
                                {
                                    if(elementList[i] != null)
                                    {
                                        Transform trans = elementList[i].transform;
                                        if (trans != null)
                                        {
                                            if (Vector3.SqrMagnitude(elementWave.gameObject.transform.position - trans.position) < elementWave.radius * elementWave.radius)
                                            {
                                                flag = true;
                                                break;
                                            }
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
                        ElementGroup elementGroup = ElementManager.Instance.GetElementGroup(wave.sponsorWave, wave.sponsorGroup);
                        if(elementGroup.curBirthTime + elementWave.lastStartTime < Time.time)
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                }
                ////判断上一个波是否在运行;
                //if (lastWave != null)
                //{
                //    //上一波出生的时间 + 这一波余姚延迟的时间 < 现在的时间;
                //    if ((lastWave.curBirthTime + elementWave.lastStartTime) < Time.time)
                //    {
                //        flag = true;
                //    }
                //}
                //else
                //{
                //    flag = true;
                //}
                break;
            case eWaveTriggerType.lastOutTime:
                {
                    if (hasGroup == true)
                    {
                        ElementGroup elementGroup = ElementManager.Instance.GetElementGroup(wave.sponsorWave, wave.sponsorGroup);
                        if (elementGroup.curEndTime + elementWave.lastOutTime < Time.time)
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                }
                ////判断上一个波是否在运行;
                //if (lastWave != null)
                //{
                //    if ((lastWave.curBirthTime + elementWave.lastOutTime) < Time.time)
                //    {
                //        flag = true;
                //    }
                //}
                //else
                //{
                //    flag = true;
                //}
                break;
        }
        return flag;
    }

    private void CommunicateFinish()
    {
        if (wave == null)
            return;
                
        ElementManager.Instance.OnStartWave(wave.curWave);

        wave.loopNum--;
        if (wave.loopNum <= 0)
        {
            isDetection = false;
            this.gameObject.SetActive(false);
        }
    }
}
