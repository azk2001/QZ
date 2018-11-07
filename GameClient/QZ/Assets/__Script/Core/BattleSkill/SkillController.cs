using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour {

    public SkillTrigger onFire = null;

    private Vector2 mMarkPosition = Vector2.zero;
	private bool isCollider = false;

	void Start () {
        if(onFire != null)
        {
            onFire.onTriggerEnter += OnFireTrigger;
        }
        
    }

	public void OnFire ()
	{
		isCollider = true;

	}

    public void OnEnd()
	{
		isCollider = false;
	}

	public void OnFireTrigger(Collider other) 
	{
		if (isCollider == false)
			return;

        //碰撞检测;
	}

    private void OnDestroy()
    {
        onFire.onTriggerEnter -= OnFireTrigger;
    }

}
