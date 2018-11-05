using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSkillPrefab : MonoBehaviour {

	private Transform transformCache = null;
	private Vector3 toPosition = Vector3.zero;
	private bool isMove = false;
	private float time= 0.1f;
	private float dataTime =0;
	private Vector3 startPosition = Vector3.zero;
	private Animator animator= null;
	void Awake()
	{
		transformCache = this.transform;
		animator = GetComponentInChildren<Animator> ();
	}

	void Start () {
		
	}

	void Update () {
		if (isMove == false)
			return;

		dataTime += 1f / time * Time.deltaTime;

		transformCache.position = Vector3.Lerp (startPosition,toPosition,dataTime);

		if (Vector3.Distance (transformCache.position, toPosition) < 0.02f) {
			isMove=false;
		}
	}

	public void MoveTo(Vector3 targetPosition,float moveTime)
	{
		time = moveTime;
		dataTime = 0;
		startPosition = transformCache.position;
		toPosition = targetPosition;
		isMove = true;
	}

	public void PlayAnimation(string aniName)
	{
		if (animator == null)
			return;

		animator.Play (aniName);
	}

	void OnDisable()
	{
		isMove = false ;
	}

	void OnDestroy()
	{
		isMove = false ;
	}

}
