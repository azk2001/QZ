using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//buff 改变移动速度
public class BuffChangeSpeed : BuffBase
{
	private float birthTime = 0;    //出生显示多长时间;
	private float validTime = 10;   //持续多长的时间;
    private float dataSpeed = 1f;   //改变速度值;
	protected override bool OnInitConfig (Dictionary<string,string> textParam)
	{
        birthTime = textParam["birthTime"].ToFloat(0);
        validTime = textParam["validTime"].ToFloat(0);
        dataSpeed = textParam["dataSpeed"].ToFloat(0);

        return base.OnInitConfig (textParam);
	}

	protected override bool OnBegin (GameUnit actor)
	{

		return base.OnBegin (actor);
	}

	protected override void Update (float deltaTime)
	{
		
		base.Update (deltaTime);
	}

	protected override void OnEnd (GameUnit actor)
	{
		
		base.OnEnd (actor);
	}
}
