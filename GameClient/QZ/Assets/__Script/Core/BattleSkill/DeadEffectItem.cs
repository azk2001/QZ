using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEffectItem : MonoBehaviour {
    
	public UnitController mUnitController = null;

	void Awake () 
	{

	}
	
	public void Play (UnitController unitController) 
	{
		mUnitController = unitController;
	}

	void OnTriggerEnter(Collider col)
	{

	}


}
