using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using System;

namespace BattleServer
{
    public class LoadSaveMap
    {
        public static VoidBoolDelegate OnLoadFinishEvent;
        private static string xmlPath = "/Config/MapXml/{0}.xml";

        public static bool LoadMap(int mapConfig, ElementManager elementManager)
        {
           string  path = System.IO.Directory.GetCurrentDirectory() + string.Format(xmlPath, mapConfig);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNodeList allConfig = xmlDoc.SelectSingleNode("XML").ChildNodes;

            foreach (XmlElement xe in allConfig)
            {
                //读取怪物的位置信息;
                if (xe.Name.Equals("Element"))
                {
                    XmlNodeList elementXmlList = xe.ChildNodes;
                    for (int i = 0; i < elementXmlList.Count; i++)
                    {
                        XmlElement elementXml = (XmlElement)elementXmlList[i];
                        if (elementXml.Name == "Wave")
                        {
                            string communicateName = elementXml.GetAttribute("objectNeme");      //读取组的名字;

                            ElementWave elementWave =new ElementWave();
                            XMLToTransform(elementXml, elementWave.transform);


                            elementWave.curWave = int.Parse(elementXml.GetAttribute("curWave"));//波;
                                                                                                //             elementWave.lastWave = int.Parse(elementXml.GetAttribute("lastWave"));//上一波;
                            elementWave.loopNum = int.Parse(elementXml.GetAttribute("loopNum")); //循环;

                            elementWave.sponsorWave = int.Parse(elementXml.GetAttribute("sponsorWave"));        //触碰触发波;
                            elementWave.sponsorGroup = int.Parse(elementXml.GetAttribute("sponsorGroup"));      //触碰触发组;

                            elementWave.elementTriggerType = (eWaveTriggerType)System.Enum.Parse(typeof(eWaveTriggerType), elementXml.GetAttribute("elementTriggerType"));

                            //获取怪物的类型;
                            XmlNodeList waveXmlList = elementXml.ChildNodes;
                            foreach (XmlElement groupXml in waveXmlList)
                            {
                                //获取出生触发类型;
                                if (groupXml.Name.Equals("ElementTriggerType"))
                                {
                                    {
                                        WaveBirthTrigger waveBirthTrigger = new WaveBirthTrigger();
                                        waveBirthTrigger.SetElementTrigger(elementWave, elementManager);

                                        elementWave.triggerGameObject = waveBirthTrigger;

                                        //游戏一开始就直接执行;
                                        waveBirthTrigger.isRun = true;

                                        elementManager.waveTriggerList.Add(waveBirthTrigger);
                                    }
                                    

                                    switch (elementWave.elementTriggerType)
                                    {
                                        case eWaveTriggerType.trigger:

                                            break;
                                        case eWaveTriggerType.radius:
                                            elementWave.radius = float.Parse(groupXml.GetAttribute("radius")); //半径;
                                            break;
                                        case eWaveTriggerType.lastStartTime://上一波怪出生;
                                            elementWave.lastStartTime = float.Parse(groupXml.GetAttribute("lastStartTime"));

                                            break;
                                        case eWaveTriggerType.lastOutTime://上一波击杀完成;
                                            elementWave.lastOutTime = float.Parse(groupXml.GetAttribute("lastOutTime"));
                                            break;
                                    }
                                }

                                if (groupXml.Name.Equals("Group"))
                                {
                                    string elementGroupName = groupXml.GetAttribute("enemyName");   //组的名字;
                                    ElementGroup elementGroup =new ElementGroup();
                                    XMLToTransform(groupXml, elementGroup.transform);

                                    elementWave.elementGroupList.Add(elementGroup);

                                    elementGroup.curGroup = int.Parse(groupXml.GetAttribute("curGroup"));//组编号;
                                    elementGroup.curWave = int.Parse(groupXml.GetAttribute("curWave"));//波编号;
                                    elementGroup.loopNum = int.Parse(groupXml.GetAttribute("loopNum"));//波编号;
                                    elementGroup.lastGroup = int.Parse(groupXml.GetAttribute("lastGroup"));

                                    if (groupXml.HasAttribute("isNeglect"))
                                    {
                                        elementGroup.isNeglect = bool.Parse(groupXml.GetAttribute("isNeglect"));
                                    }

                                    if (groupXml.HasAttribute("aiStart"))
                                    {
                                        elementGroup.aiStart = bool.Parse(groupXml.GetAttribute("aiStart"));
                                    }

                                    elementGroup.elementType = (eElementType)System.Enum.Parse(typeof(eElementType), groupXml.GetAttribute("elementType"));


                                    if(elementGroup.elementType == eElementType.player)
                                    {
                                        elementManager.battleCore.playerElement = elementGroup;
                                    }

                                    //写入唤醒Trigger类型;
                                    elementGroup.elementTriggerType = (eElementTriggerType)System.Enum.Parse(typeof(eElementTriggerType), groupXml.GetAttribute("elementTriggerType"));

                                    //获取下一级的属性;
                                    XmlNodeList groupXmlList = groupXml.ChildNodes;
                                    foreach (XmlElement elementGroupXml in groupXmlList)
                                    {
                                        //写入唤醒出生触发的类型;
                                        if (elementGroupXml.Name.Equals("ElementTriggerType"))
                                        {
                                            {
                                                //保存触发Trigger检测的物体;
                                                GroupBirthTrigger groupBirthTrigger = new GroupBirthTrigger();
                                                elementGroup.triggerGameObject = groupBirthTrigger;

                                                //游戏一开始就关掉执行;
                                                groupBirthTrigger.isRun = false;

                                                elementManager.groupTriggerList.Add(groupBirthTrigger);
                                            }

                                            switch (elementGroup.elementTriggerType)
                                            {
                                                case eElementTriggerType.lastElementNum:
                                                    elementGroup.lastElementNum = int.Parse(elementGroupXml.GetAttribute("lastElementNum"));
                                                    break;
                                                case eElementTriggerType.lastOutTime:
                                                    elementGroup.lastOutTime = float.Parse(elementGroupXml.GetAttribute("lastOutTime"));
                                                    break;
                                                case eElementTriggerType.lastBossHp:

                                                    if (elementGroup.lastBossHp > 100) elementGroup.lastBossHp = 100;
                                                    if (elementGroup.lastBossHp < 0) elementGroup.lastBossHp = 0;

                                                    elementGroup.lastBossHp = int.Parse(elementGroupXml.GetAttribute("lastBossHp"));

                                                    break;
                                                case eElementTriggerType.lastStartTime:
                                                    elementGroup.lastStartTime = float.Parse(elementGroupXml.GetAttribute("lastStartTime"));
                                                    break;
                                                case eElementTriggerType.curOutTime:
                                                    elementGroup.curOutTime = float.Parse(elementGroupXml.GetAttribute("curOutTime"));
                                                    break;
                                                case eElementTriggerType.curStartTime:
                                                    elementGroup.curStartTime = float.Parse(elementGroupXml.GetAttribute("curStartTime"));
                                                    break;
                                            }
                                        }

                                        //获取怪物的属性;
                                        if (elementGroupXml.Name.Equals("ElementParam"))
                                        {

                                            ElementParam elementParam = new ElementParam();

                                            XMLToTransform(elementGroupXml, elementParam.transform);

                                            elementGroup.elementParamList.Add(elementParam);

                                            elementParam.elementId = int.Parse(elementGroupXml.GetAttribute("elementId"));

                                            string eventKey = elementGroupXml.GetAttribute("eventKey");
                                            if (eventKey.Equals("") == false)
                                            {
                                                elementParam.eventKey = (eEventKye)Enum.Parse(typeof(eEventKye), eventKey, false);

                                                if (elementParam.eventKey == eEventKye.SetMoveModlePoint ||
                                                    elementParam.eventKey == eEventKye.SetMonsterPatrolPoint ||
                                                    elementParam.eventKey == eEventKye.SetMonsterPatrolPath
                                                    )
                                                {
                                                    XmlNodeList pointXmlList = elementGroupXml.ChildNodes;
                                                    foreach (XmlElement pointGroupXml in pointXmlList)
                                                    {
                                                        if (pointGroupXml.Name.Equals("PointParam"))
                                                        {
                                                            Transform transform = new Transform();

                                                            XMLToTransform(pointGroupXml, transform);

                                                            elementParam.patrolPoint.Add(transform);
                                                        }
                                                    }
                                                }

                                            }

                                            elementParam.eventValue = elementGroupXml.GetAttribute("eventValue");
                                        }
                                    }

                                    elementManager.SetGroup(elementGroup.curWave, elementGroup.curGroup, elementGroup);
                                }
                            }

                            elementManager.SetWave(elementWave.curWave, elementWave);

                        }
                    }
                }
            }

            if (OnLoadFinishEvent != null)
            {
                OnLoadFinishEvent(true);
            }
            return true;
        }

        private static void XMLToTransform(XmlElement xml, Transform perfab)
        {
            perfab.position = StringToVector3(xml.GetAttribute("position"));
            perfab.eulerAngles = StringToVector3(xml.GetAttribute("rotation"));
            perfab.localScale = StringToVector3(xml.GetAttribute("scale"));
        }

        /// <summary>
        /// Strings to vector3.
        /// </summary>
        /// <returns>The to vector3.</returns>
        /// <param name="text">Text.</param>
        private static Vector3 StringToVector3(string text)
        {
            string[] textArr = text.Split(';');
            Vector3 tmpv3 = new Vector3(float.Parse(textArr[0]), float.Parse(textArr[1]), float.Parse(textArr[2]));
            return tmpv3;
        }
    }
}
 
