using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eEventKye
{
    TimeLine,
    FinishGroup,
    AIController,
    ReplaceAI,
    SetObstruct,                      //设置是否产生阻挡;
    SetCamera,                        //设置照相机旋转角度
    MapGuide,                         //怪物指引;
    PlayAnimation,                    //播放动作;
    BirthBoosAnimation,               //boss出场动画;
    SceneWarning,                     //场景警告
    MoveModle,                        //移动模型;
    SetMoveModlePoint,                //AI移动模型;
    SetMonsterPatrolPoint,            //AI定点巡逻;
    SetMonsterPatrolPath,             //AI路径巡逻;
    KillMonster,                      //击杀怪物;
    NoviceGuide,                      //新手引导,
    DestroyGameObject,                //删除游戏对象
}

public class ElementParam : MonoBehaviour
{
    public int elementIndex = 0; //关卡元件索引;

    public eElementType elementType;//原件类型;

    public int elementId = 1;//刷怪器表ID;

    public eEventKye eventKey = eEventKye.TimeLine;//触发事件Key;
    public string eventValue = "";//触发事件Value;

    public bool isMagnet = true;


    void Start()
    {

    }
}
