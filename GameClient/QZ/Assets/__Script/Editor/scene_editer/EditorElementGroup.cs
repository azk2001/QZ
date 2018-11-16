using UnityEditor;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

[CanEditMultipleObjects]
[CustomEditor(typeof(ElementGroup))]
public class EditorElementGroup : Editor
{

    public override void OnInspectorGUI()
    {
        ElementGroup _target = (ElementGroup)target;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(_target.transform.position, out hit, 100000, NavMesh.AllAreas))
        {
            _target.transform.position = hit.position;
        }

        _target.elementType = (eElementType)EditorGUILayout.EnumPopup("这一组的类型", _target.elementType);

        _target.loopNum = EditorGUILayout.IntField("循环次数", _target.loopNum);

        _target.curGroup = EditorGUILayout.IntField("当前组", _target.curGroup);
        if (_target.curGroup < 1)
        {
            _target.curGroup = 1;
        }

        _target.gameObject.name = "group_" + _target.elementType + "_" + _target.curGroup;

        _target.isNeglect = EditorGUILayout.Toggle("是否忽略这一组不记录游戏结束", _target.isNeglect);

        _target.aiStart = EditorGUILayout.Toggle("是否开始就启动AI", _target.aiStart);

        EditorGUILayout.LabelField("-------------------------------");

        _target.lastGroup = EditorGUILayout.IntField("关联的上一组", _target.lastGroup);
        _target.elementTriggerType = (eElementTriggerType)EditorGUILayout.EnumPopup("激活触发的类型", _target.elementTriggerType);
        switch (_target.elementTriggerType)
        {
            case eElementTriggerType.lastOutTime:
                _target.lastOutTime = EditorGUILayout.FloatField("上一组的结束时间", _target.lastOutTime);
                break;
            case eElementTriggerType.lastStartTime:
                _target.lastStartTime = EditorGUILayout.FloatField("上一组的开始时间", _target.lastStartTime);
                break;
            case eElementTriggerType.lastElementNum:
                _target.lastElementNum = EditorGUILayout.IntField("上一组的怪物数量", _target.lastElementNum);
                break;
            case eElementTriggerType.lastBossHp:
                _target.lastBossHp = EditorGUILayout.IntField("上一组BOSS的血量(百分比)", _target.lastBossHp);
                break;
            case eElementTriggerType.curOutTime:
                _target.curOutTime = EditorGUILayout.FloatField("当前组结束的时间", _target.curOutTime);
                break;
            case eElementTriggerType.curStartTime:
                _target.curStartTime = EditorGUILayout.FloatField("当前组开始的时间", _target.curStartTime);
                break;
        }

        if (GUILayout.Button("创建一个游戏物体"))
        {
            GameObject createObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            createObject.name = "createObject";
            createObject.transform.parent = _target.transform;

            ElementParam enemyObject = createObject.GetComponent<ElementParam>();
            if (enemyObject == null)
            {
                enemyObject = createObject.AddComponent<ElementParam>();
            }

            Selection.activeGameObject = createObject;
        }


        GUILayout.Label
                        (
                        "激活触发的类型:lastOutTime    关联的上一波的事件结束（怪物：全部死亡 事件：完成）\n" +
                        "激活触发的类型:lastStartTime   关联的上一波事件开始 (怪物：出生 事件：启动)\n" +
                        "激活触发的类型:lastElementNum  关联的上一波事件元素剩余数量（怪物：数量 事件：剩余触发事件数量）\n" +
                        "激活触发的类型:lastBossHp     关联的上一波指定怪物剩余血量百分比，怪物有且只能拥有一个，不能指定事件 \n" +
                        "激活触发的类型:curOutTime      当前波指定时间结束后，只能用于当前循环刷怪事件 \n" +
                        "激活触发的类型:curStartTime     当前波指定时间开始后，只能用于当前循环刷怪事件 \n"
                        
                        );
    }

}