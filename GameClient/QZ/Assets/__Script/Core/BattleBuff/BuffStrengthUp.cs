using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStrengthUp : BuffBase
{
	private float offsetStrength = 1f;
	private float validTime = 0;
	private float dataTime = 0;
	private bool isAddStrength = false;
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
