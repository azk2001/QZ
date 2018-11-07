using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTrigger : MonoBehaviour {

    public VoidColliderDelegate onTriggerEnter = null;

    void Awake()
	{

	}

	// Use this for initialization
	void Start () {

	}

	public void OnTriggerEnter(Collider other)
	{
        if(onTriggerEnter!=null)
        {
            onTriggerEnter(other);
        }
	}
}
