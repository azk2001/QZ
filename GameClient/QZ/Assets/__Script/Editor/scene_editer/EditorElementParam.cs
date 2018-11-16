using UnityEditor;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

[CanEditMultipleObjects]
[CustomEditor(typeof(ElementParam))]
public class EditorElementParam : Editor
{
    public static int _elementIndex = 1;
    public static int elementIndex
    {
        get
        {
            return _elementIndex++;
        }
    }

    private static int elementId = 0;

    public override void OnInspectorGUI()
    {
        ElementParam _target = (ElementParam)target;

        if (_target.transform.parent == null)
            return;
        _target.isMagnet = EditorGUILayout.Toggle("是否吸附在NvaMesh上面", _target.isMagnet);

        if (_target.isMagnet)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(_target.transform.position, out hit, 100000, NavMesh.AllAreas))
            {
                _target.transform.position = hit.position;
            }
        }


        ElementGroup group = _target.transform.parent.GetComponent<ElementGroup>();
        if (group != null)
        {

            EditorGUILayout.IntField("关卡元件索引", _target.elementIndex);

            if (_target.elementIndex < 1)
            {
                _target.elementIndex = elementIndex;
            }

            _target.elementId = EditorGUILayout.IntField("怪物名字(ID)", _target.elementId);

            _target.elementType = group.elementType;

            _target.gameObject.name = "element_" + _target.elementIndex + "_" + _target.elementId;

            switch (_target.elementType)
            {

                case eElementType.monster:
                case eElementType.player:
                    {

                    }
                    break;
                case eElementType.eventTrigger:

                    _target.eventKey = (eEventKye)EditorGUILayout.EnumPopup("触发唯一标识", _target.eventKey);
                    _target.eventValue = EditorGUILayout.TextField("触发参数", _target.eventValue);

                    if (_target.eventKey == eEventKye.SetMoveModlePoint && GUILayout.Button("创建AI目标点"))
                    {
                        if (_target.transform.childCount > 0)
                        {
                            _target.transform.ClearChild();
                        }

                        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        g.name = "AI目标点";
                        g.transform.SetParent(_target.transform);
                    }

                    if (_target.eventKey == eEventKye.SetMonsterPatrolPoint && GUILayout.Button("创建AI中心点"))
                    {
                        if (_target.transform.childCount > 0)
                        {
                            _target.transform.ClearChild();
                        }

                        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        g.name = "AI中心点";
                        g.transform.SetParent(_target.transform);
                    }

                    if (_target.eventKey == eEventKye.SetMonsterPatrolPath && GUILayout.Button("创建AI路径点"))
                    {
                        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        g.name = "AI路径点";
                        g.transform.SetParent(_target.transform);
                    }

                    GUILayout.Label
                        (
                        "触发唯一标识:TimeLine      触发参数:剧情编辑器资源名字 （**多人副本剧情不能参与刷怪器逻辑，并且不能配置跳过按钮**）(播放剧情)\n" +
                        "触发唯一标识:FinishGroup   触发参数:完成的波;完成的组  (直接完成哪一组)\n" +
                        "触发唯一标识:AIController  触发参数:完成的波;完成的组;? (?=0暂停,?=1继续)  (是否激活AI)\n" +
                        "触发唯一标识:ReplaceAI     触发参数:完成的波;完成的组;替换AI的资源名字  (替换AI)\n" +
                        "触发唯一标识:SetObstruct   触发参数:完成的波;完成的组;是否产生碰撞 (是否产生阻挡) \n" +
                        "触发唯一标识:SetCamera     触发参数:x;y    x:旋转到的目标角度(0-360) y:旋转时间 (设置照相机旋转)\n" +
                        "触发唯一标识:MapGuide      触发参数:x;y;z  x:是否显示指引1显示，y:要指引的怪物波，z:要指引的怪物组 (怪物指引)\n" +
                        "触发唯一标识:PlayAnimation     触发参数:x;y;z x:要播放动作的怪物波，y:要播放动作的怪物组 z:要播放的动作名字(播放动作)\n" +
                        "触发唯一标识:BirthBoosAnimation     触发参数:n;x;y  n:剧情资源名字，不填显示默认的剧情；x:要看到的波;y:要看到的组 (boss出场动画)\n" +
                        "触发唯一标识:SceneWarning     触发参数:x  x:显示的图片名字ID配置在timeline_c (场景警告)\n" +
                        "触发唯一标识:MoveModlePoint     触发参数:m,n;x,y,z  m:波n:组xyz:移动的目标点位置  (模型移动到指定点)\n" +
                        "触发唯一标识:SetMoveModlePoint     触发参数:m,n  m:波n:组  (AI命令模型移动)\n" +
                        "触发唯一标识:SetMonsterPatrolPoint     触发参数:m,n,w  m:波n:组w:半径  (AI命令模型顶点巡逻)\n" +
                        "触发唯一标识:SetMonsterPatrolPath     触发参数:m,n  m:波n:组  (AI命令模型顶点巡逻)\n" +
                        "触发唯一标识:KillMonster     触发参数:m,n  m:波n:组  (击杀指定波组的怪物)\n" +
                        "触发唯一标识:DestroyGameObject     触发参数:m,n  m:波n:组  (直接删除指定波组的怪物)\n" +
                        "触发唯一标识:NoviceGuide     触发参数:m,n  m:新手引导ID n:1开始0结束  (触发新手引导)\n", "0"
                        );

                    break;
            }
        }
    }
    //
    //	void OnSceneGUI () {
    //		
    //		ElementParam _target = (ElementParam)target;
    //
    //		switch (_target.elementType) {
    //		case eElementType.monster:
    //		case eElementType.player:
    //			Handles.color = new Color (1, 1, 0, 0.2f);
    //			Handles.DrawSolidArc (_target.gameObject.transform.position, _target.gameObject.transform.up,-_target.gameObject.transform.right, 360,10);
    //			Handles.color = Color.white;
    //
    //			Handles.color = new Color (1, 0, 0, 0.2f);
    //			Handles.DrawSolidArc (_target.gameObject.transform.position, _target.gameObject.transform.up,-_target.gameObject.transform.right, 360,10);
    //			Handles.color = Color.white;
    //		break;
    //
    //		}
    //	}
}