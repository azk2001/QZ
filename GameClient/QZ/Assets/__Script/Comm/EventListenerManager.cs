using System;
using UnityEngine;
using System.Collections.Generic;

// -------------------事件添加在下面---------------------------
public enum EventEnum
{
    getHint = 100000,   //获取触发条件;
    setHint,            //向外投递已经设置完成的红绿灯功能;
    onclickItem,        //点击item 事件;
    sysOpen,            //系统开放设置;
    refreshSysOpen,     //刷新系统开放;
    openUI,             //打开UI;
    closeUI,            //关闭UI;
    birthPlayer,        //战斗中玩家出生事件;
    birthMonster,       //战斗中怪物出生事件;
    birthEvent,         //战斗中事件触发事件;
    birthObstruct,      //战斗中阻挡触发事件;
    battleElementFinish,//战斗中所有原件都已经完成;
    onChangeWeapon,     //切换神兵;
    gameUnitReduceLife, //角色血量发生变化事件,
    gameUnitBirth,      //角色出生事件;
    gameUnitDeath,      //角色死亡事件;
    gameUnitDestory,    //删除角色事件;
    localPlayerStartMovePoint,//角色摇杆开始移动到指定点回调;
    localPlayerEndMovePoint,//角色摇杆停止移动到指定点回调;
    gameUnitStartMovePoint,//角色开始移动到指定点回调
    gameUnitMoveToPoint,//角色移动到指定点回调;
    onTimeLinePlayStart,//开始播放剧情;
    onTimeLinePlayEnd,  //剧情播放完成;
    onDungeonRunTime,   //副本运行时间发生变化;
    onTimeLineEvent,    //剧情中触发的事件;
    onDungeonCondition, //关卡任务事件发生改变;
    onBagDataChange,    //背包数据改变
    onGetSoul,          //获得武魂
    on3DClickSingle,    //点击3d场景模型;
    onStoveChange,     //乾坤炉改变
    onSuitPowerChange, //套装总战斗力改变 
    onNotifyModified,  //通知改变
    onChangeSuit,      //切换套装
    onChangeSoul,      //切换武魂
    onGameFinish,      //关卡完成事件;
    onSysNotify,       //同步通知
    onGamePause,       //游戏暂停
    onGodWeaponOp,      //神兵操作   
    onTaskDialog,       //任务对话操作;  
    onRigidFlag,        //破定值改变;
    onComboAlter,       //连击改变;
    onShiftGodWeapon,   //切换神兵;
    onBuffAlter,        //buff改变;
    onGuildChange,      //帮会名字改变;
    onGetInvite,        //邀请信息
    onClickHref,        //点击超链接
    on3DClickMultiple,       //点击3d场景模型(返回所有);
    onLocalPlayerInMainCity,//本地玩家进入主场景
    onTaskRefresh,      //任务刷新;
    onChangeLevel,      //等级发送改变;
    onReachTarget,      //目标达成;
    onAutoBattle,       //自动战斗;
    onLocalDoSkill,     //本地玩家施放技能
    onTitleChange,     //当称号改变
    onSetHintSystem,    //修改一个系统开发系统;
    onSystemGoToScene,  //系统进入场景;
    onBirthLocalPlayer,  //本地玩家出生事件;
    onGuideIDTrigger,      //引导ID触发引导事件
    onNetPlayerInScene,     //网络玩家进入场景;
    onNetPlayerOutScene,    //网络玩家离开场景
    onGuidStart,             //开始触发引导
    onSysOpenTweener,       //系统开放飘动动画播放完成;
    onGetSysGift,           //领取系统送神兵，送美人成功
    onEnterScene,           //服务器通知玩家进入场景;
    onProtocolReceive,          //接受到服务器消息返回;
    onNetPlayerModleLoadFinish, //网络玩家的模型加载完成
};


// -------------------------------------------------------------------------------------

public delegate void Callback<T>(T arg1);
static public class EventListenerManager
{
    private static Dictionary<int, Delegate> mProtocolEventTable = new Dictionary<int, Delegate>();

    public static void AddListener(EventEnum protocolKey, Callback<System.Object[]> kHandler)
    {
        AddListener((int)protocolKey, kHandler);
    }

    public static void AddListener(int protocolKey, Callback<System.Object[]> kHandler)
    {
        lock (mProtocolEventTable)
        {
            if (!mProtocolEventTable.ContainsKey(protocolKey))
            {
                mProtocolEventTable.Add(protocolKey, null);
            }

            mProtocolEventTable[protocolKey] = (Callback<System.Object[]>)mProtocolEventTable[protocolKey] + kHandler;
        }
    }

    public static void RemoveListener(EventEnum protocolKey, Callback<System.Object[]> kHandler)
    {
        RemoveListener((int)protocolKey, kHandler);
    }

    public static void RemoveListener(int protocolKey, Callback<System.Object[]> kHandler)
    {
        lock (mProtocolEventTable)
        {
            if (mProtocolEventTable.ContainsKey(protocolKey))
            {
                mProtocolEventTable[protocolKey] = (Callback<System.Object[]>)mProtocolEventTable[protocolKey] - kHandler;

                if (mProtocolEventTable[protocolKey] == null)
                {
                    mProtocolEventTable.Remove(protocolKey);
                }
            }
        }
    }

    public static void Invoke(EventEnum protocolKey, params System.Object[] args)
    {
        Invoke((int)protocolKey, args);
    }

    public static void Invoke(int protocolKey, params System.Object[] args)
    {
        try
        {
            Delegate kDelegate;
            if (mProtocolEventTable.TryGetValue(protocolKey, out kDelegate))
            {
                Callback<System.Object[]> kHandler = (Callback<System.Object[]>)kDelegate;

                if (kHandler != null)
                {
                    kHandler(args);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void UnInit()
    {
        mProtocolEventTable.Clear();
    }
}
