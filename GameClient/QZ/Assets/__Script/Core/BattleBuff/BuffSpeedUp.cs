using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpeedUp : BuffBase
{
	private float dataTime = 0;
	private float validTime = 10;
	private float offsetSpeed = 1f;
	private bool isAddSpeed = false;
	protected override bool OnInitConfig (Dictionary<string,string> textParam)
	{

		return base.OnInitConfig (textParam);
	}

	protected override bool OnBegin (GameUnit actor)
	{

		return base.OnBegin (actor);
	}

	protected override void UpdatePerDelta (GameUnit actor)
	{
		
		base.UpdatePerDelta (actor);
	}

	protected override void OnEnd (GameUnit actor)
	{
		
		base.OnEnd (actor);
	}
}
