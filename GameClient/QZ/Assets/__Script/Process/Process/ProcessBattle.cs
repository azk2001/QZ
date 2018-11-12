using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ProcessBattle : ProcessBase
{
    public override void OnBegin()
    {
        GameSceneManager.Instance.Begin(SceneType.battle);

        base.OnBegin();
    }

    public override void OnEnd()
    {
        GameSceneManager.Instance.OnEnd();

        base.OnEnd();
    }
}
