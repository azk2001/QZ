using UnityEditor;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

[CanEditMultipleObjects]
[CustomEditor(typeof(ElementWave))]
public class EditorElementWave : Editor
{

    public override void OnInspectorGUI()
    {
        ElementWave _target = (ElementWave)target;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(_target.transform.position, out hit, 100000, NavMesh.AllAreas))
        {
            _target.transform.position = hit.position;
        }

        _target.curWave = EditorGUILayout.IntField("当前波", _target.curWave);

        _target.loopNum = EditorGUILayout.IntField("循环次数", _target.loopNum);

        if (_target.curWave < 1)
            _target.curWave = 1;

        _target.gameObject.name = "wave_" + _target.curWave;

   //     EditorGUILayout.LabelField("-------------------------------");

   //     _target.lastWave = EditorGUILayout.IntField("关联上一波", _target.lastWave);
        EditorGUILayout.LabelField("-------------------------------");
        _target.sponsorWave = EditorGUILayout.IntField("触发者的波", _target.sponsorWave);
        _target.sponsorGroup = EditorGUILayout.IntField("触发者的组", _target.sponsorGroup);

        //触发类型;
        _target.elementTriggerType = (eWaveTriggerType)EditorGUILayout.EnumPopup("出生的类型", _target.elementTriggerType);

        switch (_target.elementTriggerType)
        {
            case eWaveTriggerType.trigger:
                _target.triggerGameObject = AddTrigger(_target, _target.triggerGameObject, "birthTrigger");

                break;
            case eWaveTriggerType.radius:
                _target.radius = EditorGUILayout.FloatField("半径", _target.radius);
                if (_target.triggerGameObject != null)
                {
                    GameObject.DestroyImmediate(_target.triggerGameObject);
                }

                Debug.DrawLine(_target.transform.position - Vector3.left * _target.radius, _target.transform.position + Vector3.left * _target.radius, Color.green);
                Debug.DrawLine(_target.transform.position - Vector3.forward * _target.radius, _target.transform.position + Vector3.forward * _target.radius, Color.green);


                break;
            case eWaveTriggerType.lastStartTime:
                _target.lastStartTime = EditorGUILayout.FloatField("关联上一波出生的时间", _target.lastStartTime);
                if (_target.triggerGameObject != null)
                {
                    GameObject.DestroyImmediate(_target.triggerGameObject);
                }

                break;
            case eWaveTriggerType.lastOutTime:
                _target.lastOutTime = EditorGUILayout.FloatField("关联上一波的结束时间", _target.lastOutTime);
                if (_target.triggerGameObject != null)
                {
                    GameObject.DestroyImmediate(_target.triggerGameObject);
                }
                break;
        }

        if (GUILayout.Button("创建一个组"))
        {
            CreateGroup(_target);
        }

        GUILayout.Label
                (
                "出生的类型:trigger    碰撞触发，(**多人副本类型不可以使用**)必须增加碰撞器。如果没有波组游戏开始触发，如果有波组则必定由指定波组触发\n" +
                "出生的类型:radius     半径触发，必须设置半径。如果没有波组游戏开始触发，如果有波组则必定由指定波组进入半径后触发\n" +
                "出生的类型:lastStartTime  上一波事件开始后触发。\n" +
                "出生的类型:lastOutTime    上一波事件借宿后触发（怪物：怪物全部死亡。事件：事件完全结束）\n" 
                );
    }

    void OnSceneGUI()
    {

        ElementWave _target = (ElementWave)target;
        switch (_target.elementTriggerType)
        {
            case eWaveTriggerType.radius:
                Handles.color = new Color(1, 1, 0, 0.2f);
                Handles.DrawSolidArc(_target.transform.position, _target.transform.up, -_target.transform.right, 360, _target.radius);
                Handles.color = Color.white;
                break;
        }
    }


    private GameObject AddTrigger(ElementWave _target, GameObject targetGameObject, string name)
    {
        if (GUILayout.Button("创建一个Trigger"))
        {
            if (targetGameObject != null)
            {
                GameObject.DestroyImmediate(targetGameObject);
            }

            GameObject game = new GameObject();
            game.name = name;
            game.AddComponent<BoxCollider>();
            CreatePrefab.AddParent(_target.transform.gameObject, game);
            targetGameObject = game;

            Selection.activeGameObject = game;
        }
        return targetGameObject;
    }

    private void CreateGroup(ElementWave _target)
    {
        GameObject g = new GameObject();
        g.name = "group_1";
        g.transform.parent = _target.transform;
        g.AddComponent<ElementGroup>();

        Selection.activeGameObject = g;
    }
}