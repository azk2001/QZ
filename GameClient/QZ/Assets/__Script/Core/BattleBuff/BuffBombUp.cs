using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBombUp : BuffBase
{
	private int offsetBomb = 1;
	private float validTime = 0;
	private float dataTime = 0;
	private bool isAddBomb = false;
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
