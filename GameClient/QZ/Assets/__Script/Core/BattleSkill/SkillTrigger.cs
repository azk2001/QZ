using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTrigger : MonoBehaviour {
    
	private bool isAddColldier = false;
	void Awake()
	{

	}

	void Start () {
		
	}
	
	public void OnTriggerEnter(Collider other)
	{

	}

	void OnDisable()
	{
		if (isAddColldier == true) {
			GameObject.Destroy(this);
		}
	}

	void OnDestroy()
	{
		if (isAddColldier == true) {
			GameObject.Destroy(this);
		}
	}

}
