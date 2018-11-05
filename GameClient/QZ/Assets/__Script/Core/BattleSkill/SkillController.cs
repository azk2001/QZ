using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour {

	public OnTriggerEvent upEffect = null;
	public OnTriggerEvent downEffect = null;
	public OnTriggerEvent leftEffect = null;
	public OnTriggerEvent rightEffect = null;

	private Vector2 mMarkPosition = Vector2.zero;
	private bool isCollider = false;
	void Start () {
		
	}

	public void Play (Vector2 markPosition, int upLength,int downLength,int leftLength,int rigthLength,float detectionTime)
	{
		isCollider = true;
		mMarkPosition = markPosition;

		upEffect.Init (upLength);
		downEffect.Init (downLength);
		leftEffect.Init (leftLength);
		rightEffect.Init (rigthLength);

		CancelInvoke ("DetectionTimeEnd");
		Invoke ("DetectionTimeEnd", detectionTime);//检测碰撞时间;
	}

	void DetectionTimeEnd()
	{
		isCollider = false;
	}

	void OnTrigger(Collider other) 
	{
		if (isCollider == false)
			return;
	}

	void OnDisable()
	{

	}
}
