using UnityEngine.SceneManagement;
using UnityEngine;

class ProcessMain : ProcessBase
{
    public override void OnBegin()
    {
        GameSceneManager.Instance.Begin(SceneType.main);

        base.OnBegin();
    }

    public override void OnEnd()
    {
        GameSceneManager.Instance.OnEnd();

        base.OnEnd();
    }
}