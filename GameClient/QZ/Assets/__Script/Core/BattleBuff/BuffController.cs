using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour {

	public int uuid = 0;
	public int buffTypeId = 0;

	private bool isMove = false;
	private bool isStartMove = false;
	private Transform transformCaChe = null;
	private Vector3 toPosition =Vector3.zero;
	private Transform imgAircraft = null;
		 
	void Awake()
	{
		isMove = false; 
		isStartMove = false;
		transformCaChe = this.transform;
	}

	// Use this for initialization
	void Start () {
		
	}

	void Update () {

		if (isMove == false)
			return;

		if (isStartMove == false) 
		{
			if (imgAircraft.position.x > transformCaChe.position.x)
				isStartMove = true;
		}

		if (isMove == true && isStartMove ==true)
		{
			transformCaChe.position = Vector3.Lerp (transformCaChe.position, toPosition, 0.08f);

			if (Vector3.Distance (transformCaChe.position, toPosition) < 0.1f) {
				isMove = false;
			}
		}
	}

	public void Move(GameObject aircraft, Vector3 position)
	{
		imgAircraft = aircraft.transform;
		toPosition = position;

		isMove = true;
	}
	
	void OnDisable()  
	{  
		isMove = false; 
		isStartMove = false;
	}  
	
	
	void OnDestroy()  
	{  
		isMove = false; 
		isStartMove = false;
	}
}
