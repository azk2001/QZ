using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using System;

public class LoadSaveMap : MonoBehaviour
{

    public static VoidBoolDelegate OnLoadFinishEvent;

    private static List<ElementParam> allObjectParam = new List<ElementParam>();
    private static Dictionary<int, ElementParam> allObjectParamDic = new Dictionary<int, ElementParam>();

    private static XmlDocument xmlDoc = null;

    public static int elementCount = 0;

    public static Dictionary<int, ElementParam> AllObjectParamDic
    {
        get
        {
            return allObjectParamDic;
        }
    }

    public static bool SaveMap(string path, string xmlName)
    {

        allObjectParam.Clear();

        xmlDoc = new XmlDocument();
        XmlElement xml = xmlDoc.CreateElement("XML");

        //写入怪物信息;
        GameObject element = GameObject.FindGameObjectWithTag("Element");
        if (element != null)
        {
            ElementWave[] monsterList = element.GetComponentsInChildren<ElementWave>();
            XmlElement elementXml = xmlDoc.CreateElement("Element");
            for (int i = 0; i < monsterList.Length; i++)
            {
                ElementWave elementWave = monsterList[i];   //波;
                XmlElement waveXml = xmlDoc.CreateElement("Wave");
                waveXml.SetAttribute("objectNeme", elementWave.gameObject.name);
                waveXml.SetAttribute("curWave", elementWave.curWave.ToString());
                //              waveXml.SetAttribute("lastWave", elementWave.lastWave.ToString());
                waveXml.SetAttribute("elementTriggerType", elementWave.elementTriggerType.ToString());
                waveXml.SetAttribute("sponsorWave", elementWave.sponsorWave.ToString());
                waveXml.SetAttribute("sponsorGroup", elementWave.sponsorGroup.ToString());
                waveXml.SetAttribute("loopNum", elementWave.loopNum.ToString());

                TransformToXML(waveXml, elementWave.transform); //写入波的位置;


                //写入怪物触发出生Trigger的类型和参数;
                XmlElement elementTriggerType = xmlDoc.CreateElement("ElementTriggerType");
                switch (elementWave.elementTriggerType)
                {
                    case eWaveTriggerType.trigger://trigger触发;
                        GameObject birthObj = elementWave.triggerGameObject;
                        if (birthObj == null)
                        {
                            Debug.LogError("你没有创建 怪物出生Trigger的条件碰撞体");
                            return false;
                        }

                        TransformToXML(elementTriggerType, birthObj.transform);
                        ColliderToXML(elementTriggerType, birthObj.transform);
                        break;
                    case eWaveTriggerType.radius://半径触发;
                        elementTriggerType.SetAttribute("radius", elementWave.radius.ToString());
                        break;
                    case eWaveTriggerType.lastStartTime://上一波怪出生;
                        elementTriggerType.SetAttribute("lastStartTime", elementWave.lastStartTime.ToString());
                        break;
                    case eWaveTriggerType.lastOutTime://上一波击杀完成;
                        elementTriggerType.SetAttribute("lastOutTime", elementWave.lastOutTime.ToString());
                        break;
                }
                waveXml.AppendChild(elementTriggerType);


                //写入怪物组的信息;
                ElementGroup[] communicateGroupList = elementWave.elementGroupList;
                for (int tmpGroup = 0, maxGroup = communicateGroupList.Length; tmpGroup < maxGroup; tmpGroup++)
                {
                    ElementGroup editorGroup = communicateGroupList[tmpGroup];
                    if (editorGroup != null)
                    {
                        //写入怪物的类型和参数;
                        XmlElement editorGroupType = xmlDoc.CreateElement("Group");
                        editorGroupType.SetAttribute("enemyName", editorGroup.gameObject.name); //写入组的名字;
                        editorGroupType.SetAttribute("curWave", elementWave.curWave.ToString());    //写入波编号;
                        editorGroupType.SetAttribute("curGroup", editorGroup.curGroup.ToString());  //写入组编号;
                        editorGroupType.SetAttribute("loopNum", editorGroup.loopNum.ToString());    //写入循环次数;
                        editorGroupType.SetAttribute("isNeglect", editorGroup.isNeglect.ToString());    //写入是否计入结束判断;
                        editorGroupType.SetAttribute("lastGroup", editorGroup.lastGroup.ToString());
                        editorGroupType.SetAttribute("elementType", editorGroup.elementType.ToString());
                        TransformToXML(editorGroupType, editorGroup.transform);

                        editorGroupType.SetAttribute("aiStart", editorGroup.aiStart.ToString());

                        //写入唤醒Trigger的条件;
                        editorGroupType.SetAttribute("elementTriggerType", editorGroup.elementTriggerType.ToString());    //写入唤醒Trigger类型;

                        XmlElement activateTriggerType = xmlDoc.CreateElement("ElementTriggerType");

                        switch (editorGroup.elementTriggerType)
                        {
                            case eElementTriggerType.lastElementNum:
                                activateTriggerType.SetAttribute("lastElementNum", editorGroup.lastElementNum.ToString());
                                break;
                            case eElementTriggerType.lastOutTime://倒计时;
                                activateTriggerType.SetAttribute("lastOutTime", editorGroup.lastOutTime.ToString());
                                break;
                            case eElementTriggerType.lastBossHp:
                                activateTriggerType.SetAttribute("lastBossHp", editorGroup.lastBossHp.ToString());
                                break;
                            case eElementTriggerType.lastStartTime:
                                activateTriggerType.SetAttribute("lastStartTime", editorGroup.lastStartTime.ToString());
                                break;
                            case eElementTriggerType.curOutTime:
                                activateTriggerType.SetAttribute("curOutTime", editorGroup.curOutTime.ToString());
                                break;
                            case eElementTriggerType.curStartTime:
                                activateTriggerType.SetAttribute("curStartTime", editorGroup.curStartTime.ToString());
                                break;
                        }
                        editorGroupType.AppendChild(activateTriggerType);

                        //写入所有怪物的位置信息;
                        ElementParam[] elementParamList = editorGroup.elementParamList;
                        for (int tmp = 0, max = elementParamList.Length; tmp < max; tmp++)
                        {
                            if (elementParamList[tmp] == null)
                                continue;

                            //高度适配;
                            UnityEngine.AI.NavMeshHit hit;
                            bool tmpb = UnityEngine.AI.NavMesh.Raycast(elementParamList[tmp].transform.position, elementParamList[tmp].transform.position, out hit, -1);
                            if (!tmpb)
                            {
                                if (hit.distance < 10000)
                                {
                                    elementParamList[tmp].transform.position = hit.position;
                                }
                            }

                            XmlElement elementParamXml = xmlDoc.CreateElement("ElementParam");
                            ElementParam communicateObject = elementParamList[tmp];

                            TransformToXML(elementParamXml, elementParamList[tmp].transform);

                            elementParamXml.SetAttribute("elementId", communicateObject.elementId.ToString());
                            elementParamXml.SetAttribute("eventKey", communicateObject.eventKey.ToString());
                            elementParamXml.SetAttribute("eventValue", communicateObject.eventValue.ToString());


                            if (communicateObject.eventKey == eEventKye.SetMoveModlePoint ||
                                communicateObject.eventKey == eEventKye.SetMonsterPatrolPoint ||
                                communicateObject.eventKey == eEventKye.SetMonsterPatrolPath
                             )
                            {
                                int childCount = communicateObject.transform.childCount;
                                if (childCount<1)
                                {
                                    Debug.LogError("事件类型"+ communicateObject.eventKey + "没有找到目标点");
                                    return false;
                                }

                                for (int index = 0, maxx = communicateObject.transform.childCount; index < maxx; index++)
                                {
                                    XmlElement pointParamXml = xmlDoc.CreateElement("PointParam");
                                    TransformToXML(pointParamXml, communicateObject.transform.GetChild(index));
                                    elementParamXml.AppendChild(pointParamXml);
                                }
                            }

                            allObjectParam.Add(communicateObject);
                            editorGroupType.AppendChild(elementParamXml);

                        }
                        waveXml.AppendChild(editorGroupType);
                    }
                }
                elementXml.AppendChild(waveXml);
            }
            xml.AppendChild(elementXml);
        }

        xmlDoc.AppendChild(xml);

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        xmlDoc.Save(path);

        return true;
    }

    public static bool LoadMap(string path, bool isRunGame)
    {

        EditorManager.GetInstance();

        //运行游戏的时候清除;
        if (isRunGame == true)
        {
            ElementManager.Instance.Init();
        }

        GameObject element = GameObject.FindGameObjectWithTag("Element");
        if (element != null)
        {
            for (int i = element.transform.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(element.transform.GetChild(i).gameObject);
            }
        }

        XmlDocument xmlDoc = new XmlDocument();
        if (isRunGame == false)
        {
            xmlDoc.Load(path);
        }
        else
        {
            xmlDoc.LoadXml(path);
        }

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

                        GameObject elementWaveObject = null;
                        if (isRunGame == false)
                        {
                            elementWaveObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        }
                        else
                        {
                            elementWaveObject = new GameObject();
                        }

                        ElementWave elementWave = elementWaveObject.AddComponent<ElementWave>();
                        elementWaveObject.name = communicateName;
                        XMLToTransform(elementXml, elementWaveObject.transform);
                        elementWaveObject.transform.parent = element.transform;


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

                                GameObject waveTrigger = null;
                                if (isRunGame == true)
                                {
                                    //保存触发Trigger检测的物体;
                                    waveTrigger = new GameObject();
                                    waveTrigger.name = "WaveTrigger";
                                    waveTrigger.transform.parent = elementWaveObject.transform;

                                    WaveBirthTrigger waveBirthTrigger = waveTrigger.AddComponent<WaveBirthTrigger>();
                                    waveBirthTrigger.SetElementTrigger(elementWave);

                                    elementWave.triggerGameObject = waveTrigger;

                                    //游戏一开始就直接执行;
                                    waveBirthTrigger.isRun = true;
                                }

                                switch (elementWave.elementTriggerType)
                                {
                                    case eWaveTriggerType.trigger:

                                        if (isRunGame == false)
                                        {
                                            waveTrigger = new GameObject();
                                            waveTrigger.name = "WaveTrigger";
                                            waveTrigger.transform.parent = elementWaveObject.transform;
                                            elementWave.triggerGameObject = waveTrigger;
                                        }

                                        XMLToTransform(groupXml, waveTrigger.transform);
                                        XMLToCollider(groupXml, waveTrigger.transform);

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
                                GameObject groupObject = new GameObject();
                                string elementGroupName = groupXml.GetAttribute("enemyName");   //组的名字;
                                ElementGroup elementGroup = groupObject.AddComponent<ElementGroup>();
                                groupObject.name = elementGroupName;
                                groupObject.transform.parent = elementWaveObject.transform;
                                XMLToTransform(groupXml, groupObject.transform);

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

                                //写入唤醒Trigger类型;
                                elementGroup.elementTriggerType = (eElementTriggerType)System.Enum.Parse(typeof(eElementTriggerType), groupXml.GetAttribute("elementTriggerType"));

                                //获取下一级的属性;
                                XmlNodeList groupXmlList = groupXml.ChildNodes;
                                foreach (XmlElement elementGroupXml in groupXmlList)
                                {
                                    //写入唤醒出生触发的类型;
                                    if (elementGroupXml.Name.Equals("ElementTriggerType"))
                                    {
                                        if (isRunGame == true)
                                        {
                                            //保存触发Trigger检测的物体;
                                            GameObject groupObejctTrigger = new GameObject();
                                            groupObejctTrigger.name = "GroupTrigger";
                                            groupObejctTrigger.transform.parent = groupObject.transform;
                                            GroupBirthTrigger groupBirthTrigger = groupObejctTrigger.AddComponent<GroupBirthTrigger>();
                                            elementGroup.triggerGameObject = groupObejctTrigger;

                                            //游戏一开始就关掉执行;
                                            groupBirthTrigger.isRun = false;
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

                                        GameObject tmpobject = null;
                                        if (isRunGame == false)
                                        {
                                            tmpobject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                        }
                                        else
                                        {
                                            tmpobject = new GameObject();
                                        }

                                        tmpobject.transform.parent = groupObject.transform;

                                        XMLToTransform(elementGroupXml, tmpobject.transform);

                                        ElementParam elementParam = tmpobject.AddComponent<ElementParam>();

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
                                                        GameObject point = null;
                                                        if (isRunGame == false)
                                                        {
                                                            point = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                                        }
                                                        else
                                                        {
                                                            point = new GameObject();
                                                        }

                                                        point.name = "创建目标点";
                                                        point.transform.SetParent(elementParam.transform);

                                                        XMLToTransform(pointGroupXml, point.transform);
                                                    }
                                                }
                                            }

                                        }

                                        elementParam.eventValue = elementGroupXml.GetAttribute("eventValue");
                                    }
                                }

                                if (isRunGame == true)
                                {
                                    ElementManager.Instance.SetGroup(elementGroup.curWave, elementGroup.curGroup, elementGroup);
                                }
                            }
                        }

                        if (isRunGame == true)
                        {
                            ElementManager.Instance.SetWave(elementWave.curWave, elementWave);
                        }

                    }
                }
            }
        }

        if (isRunGame == true)
        {
            if (OnLoadFinishEvent != null)
            {
                OnLoadFinishEvent(true);
            }
        }

        return true;
    }

    public static void ClearScenePrefab()
    {
        GameObject monster = GameObject.FindGameObjectWithTag("Element");
        if (monster != null)
        {
            for (int i = 0; i < monster.transform.childCount; i = i)
            {
                GameObject.DestroyImmediate(monster.transform.GetChild(i).gameObject);
            }
        }
    }

    private static void TransformToXML(XmlElement xml, Transform perfab)
    {
        string position = ((int)(perfab.position.x * 100)) / 100.0f + ";" + ((int)(perfab.position.y * 100)) / 100.0f + ";" + ((int)(perfab.position.z * 100)) / 100.0f;
        xml.SetAttribute("position", position);

        string rotation = ((int)(perfab.eulerAngles.x * 100)) / 100.0f + ";" + ((int)(perfab.eulerAngles.y * 100)) / 100.0f + ";" + ((int)(perfab.eulerAngles.z * 100)) / 100.0f;
        xml.SetAttribute("rotation", rotation);

        string scale = ((int)(perfab.localScale.x * 100)) / 100.0f + ";" + ((int)(perfab.localScale.y * 100)) / 100.0f + ";" + ((int)(perfab.localScale.z * 100)) / 100.0f;
        xml.SetAttribute("scale", scale);
    }

    private static void ColliderToXML(XmlElement xml, Transform perfab)
    {
        BoxCollider triggerCollider = perfab.GetComponent<BoxCollider>();

        if (triggerCollider == null)
        {
            Debug.LogError("Not find BoxCollider!");
        }

        string center = ((int)(triggerCollider.center.x * 100)) / 100.0f + ";" + ((int)(triggerCollider.center.y * 100)) / 100.0f + ";" + ((int)(triggerCollider.center.z * 100)) / 100.0f;
        xml.SetAttribute("center", center);

        string size = ((int)(triggerCollider.size.x * 100)) / 100.0f + ";" + ((int)(triggerCollider.size.y * 100)) / 100.0f + ";" + ((int)(triggerCollider.size.z * 100)) / 100.0f;
        xml.SetAttribute("size", size);
    }


    private static void XMLToTransform(XmlElement xml, Transform perfab)
    {
        perfab.position = StringToVector3(xml.GetAttribute("position"));
        perfab.eulerAngles = StringToVector3(xml.GetAttribute("rotation"));
        perfab.localScale = StringToVector3(xml.GetAttribute("scale"));
    }

    public static void XMLToCollider(XmlElement xml, Transform perfab)
    {
        BoxCollider triggerCollider = perfab.GetComponent<BoxCollider>();
        if (triggerCollider == null)
        {
            triggerCollider = perfab.gameObject.AddComponent<BoxCollider>();
        }
        triggerCollider.center = StringToVector3(xml.GetAttribute("center"));
        triggerCollider.size = StringToVector3(xml.GetAttribute("size"));
        triggerCollider.isTrigger = true;
        //	perfab.gameObject.layer = LayerMask.NameToLayer("SceneCollision");
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

    private static string Vector3ToString(Vector3 vector3)
    {
        string textArr = vector3.x + ";" + vector3.y + ";" + vector3.z;
        return textArr;
    }
}
