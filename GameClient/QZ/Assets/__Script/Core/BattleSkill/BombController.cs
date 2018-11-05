using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

	public int skillStaticId = 0;

	private Collider collider = null;

	private List<Collider> inColliderList= new List<Collider>();

	private Transform transformCache = null;
	private bool isMove = false;
	private Vector3 moveToPosition = Vector3.zero;
	private bool isInTrigger = false;

	private Renderer[] rendererList =null;

	void Awake () 
	{
		transformCache = this.transform;
		collider = this.GetComponent<Collider> ();
		rendererList = this.GetComponentsInChildren<Renderer>();
	}

	void Update()
	{
		if (isMove == false)
			return;

		transformCache.position = Vector3.Lerp (transformCache.position, moveToPosition, 0.5f);
		if (Vector3.Distance (transformCache.position,moveToPosition) < 0.01f) 
		{
			isMove = false;
		}
	}

	public void SetRenderer(bool isRenderer)
	{
		for (int i=0,max = rendererList.Length; i<max; i++) {
			rendererList[i].enabled = isRenderer;
		}
	}

	public void OnMoveBomb(Vector3 toPostion)
	{
		moveToPosition = toPostion;
		isMove = true;
	}

	void OnTriggerEnter(Collider der)
	{
		if (collider.isTrigger == true) 
		{
			inColliderList.Add (der);
		}
		isInTrigger = true;
	}

	void OnTriggerExit(Collider der)
	{
		inColliderList.Remove (der);

		if (inColliderList.Count < 1)
		{
			isInTrigger = false;

			if (collider != null)
			{
				SetTrigger(false);
			}
		}
	}

	void OnEnable()
	{
		isMove= false;
		isInTrigger = false;
	}

	void OnDisable ()
	{
		isMove= false;
		isInTrigger = false;
		SetRenderer(true);
	}

	public void SetTrigger(bool val)
	{
		if (isInTrigger == false && isMove == false) {
			inColliderList.Clear ();
			collider.isTrigger = val;
		}
	}
}
