using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{

    public SkillTrigger onFire = null;

    private bool isCollider = false;
    private Transform transformCache = null;
    private float speed = 0;
    private bool isRun = false;

    public VoidColliderDelegate OnColliderEnter = null;

    private void Awake()
    {
        transformCache = this.transform;
    }

    void Start()
    {
        if (onFire != null)
        {
            onFire.onTriggerEnter += OnFireTrigger;
        }
    }

    private void Update()
    {
        if (isRun == false)
            return;

        transformCache.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
    }

    public void OnFire(Vector3 forward, float speed, float showTime)
    {
        this.speed = speed;
        transformCache.forward = forward;
        isCollider = true;
        isRun = true;

        TimeManager.Instance.End(OnEnd);
        TimeManager.Instance.Begin(showTime, OnEnd);
    }

    public float OnEnd()
    {
        isRun = false;
        isCollider = false;

        BattleEffectRoot.Instance.DeSpwan(transformCache);

        return -1;
    }

    public void OnFireTrigger(Collider other)
    {
        if (isCollider == false)
            return;

        //碰撞检测;
        if (OnColliderEnter != null)
        {
            OnColliderEnter(other);
        }

        OnEnd();
    }

    private void OnDestroy()
    {
        onFire.onTriggerEnter -= OnFireTrigger;
    }

}
