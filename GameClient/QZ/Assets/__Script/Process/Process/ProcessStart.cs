using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

class ProcessStart : ProcessBase
{
    public override void OnBegin()
    {
        UIManager.Instance.OpenUI(eUIName.UILogin);

        base.OnBegin();
    }

    public override void OnEnd()
    {
        base.OnEnd();
    }
}
