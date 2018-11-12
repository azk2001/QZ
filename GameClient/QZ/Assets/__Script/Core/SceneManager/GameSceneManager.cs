using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum SceneType
{
    main,
    battle,
}

class GameSceneManager : SingleClass<GameSceneManager>
{
    private Dictionary<SceneType, SceneBase> procDic = new Dictionary<SceneType, SceneBase>();

    public SceneBase  curScene = null; //当前所在的流程;

    public void Init()
    {
        MainScene mainScene = new MainScene();
        AddScene(SceneType.main, mainScene);

        BattleScene battleScene = new BattleScene();
        AddScene(SceneType.battle, battleScene);
    }

    public void AddScene(SceneType type, SceneBase pb)
    {
        if (procDic.ContainsKey(type) == false)
        {
            procDic[type] = pb;
        }
    }

    public void RemoveScene(SceneType type)
    {
        if (procDic.ContainsKey(type))
        {
            procDic.Remove(type);
        }
    }
    public SceneBase GetScene(SceneType type)
    {
        if (procDic.ContainsKey(type))
        {
            return procDic[type];
        }

        return null;
    }

    //开始一个流程
    public void Begin(SceneType type)
    {
        //结束当前流程;
        if (curScene != null)
        {
            curScene.OnEnd();
        }

        //运行下一个流程;
        curScene = GetScene(type);
        curScene.OnBegin();
    }
}