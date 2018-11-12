using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SceneBase
{
    public virtual void OnBegin()
    {

    }

    public virtual void PlayerInScene(NetPlayer netPlayer)
    {

    }

    public virtual void PlayerOutScene(NetPlayer netPlayer)
    {

    }

    public virtual void OnEnd()
    {

    }
}