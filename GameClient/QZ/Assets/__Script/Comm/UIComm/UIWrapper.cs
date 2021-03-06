using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum EventIdentity
{
    uiStart,
    uiDestroy,
    uiClick,
    uiLongClick,
    uiPress,
    uiEnable,
    uiDisable,
    uiRelease,
}


public class UIEventParam
{
	public EventIdentity eventType;
	public GameObject gameobject;
}


[RequireComponent(typeof(UIGameObjectList))]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
/// <summary>
/// 和动态库UI接通信息的唯一途径;
/// </summary>
public class UIWrapper : MonoBehaviour 
{
    private GameObject curPressObject = null;
	private float startPressTime = 0;	     		//开始挤压按钮时间;
	private float clickCheckTime = 1.0f;			//点击按钮间隔;
	private bool beingPress = false;				//开始挤压;
	private bool isDragged = false;					//是否拖动;

	private int instanceId =0;

	private UIEventParam eventParam = new UIEventParam(); //UI 事件投递类型;

	void Awake()
	{
		instanceId = gameObject.GetInstanceID ();
	}

    void Start()
    {	
		eventParam.eventType = EventIdentity.uiStart;
		eventParam.gameobject = gameObject;

        EventListenerManager.Invoke(instanceId, eventParam);
    }

    void OnDestroy()
    {
		eventParam.eventType = EventIdentity.uiDestroy;
		eventParam.gameobject = gameObject;
        EventListenerManager.Invoke(instanceId, eventParam);
    }

	public void OnTrigClick()
    {

		eventParam.eventType = EventIdentity.uiClick;
		eventParam.gameobject = EventSystem.current.currentSelectedGameObject;

		float delta = Time.time - startPressTime;
		if (!beingPress || (delta <= clickCheckTime)) {
			if (!isDragged) {

                EventListenerManager.Invoke(instanceId,eventParam);
			} 
		}
		beingPress = false;
	}

    public void OnClickObject(GameObject obj)
    {

        eventParam.eventType = EventIdentity.uiClick;
        eventParam.gameobject = obj;

        float delta = Time.time - startPressTime;
        if (!beingPress || (delta <= clickCheckTime))
        {
            if (!isDragged)
            {
                EventListenerManager.Invoke(instanceId, eventParam);
            }
        }
        beingPress = false;
    }

    void OnEnable()
	{
		eventParam.eventType = EventIdentity.uiEnable;
		eventParam.gameobject = gameObject;

        EventListenerManager.Invoke( instanceId,eventParam);
	}

	void OnDisable()
	{
		eventParam.eventType = EventIdentity.uiDisable;
		eventParam.gameobject = gameObject;

        EventListenerManager.Invoke( instanceId, eventParam);
	}
	
	public void OnTriggerPress(GameObject obj)
	{
		isDragged = false;
		startPressTime = Time.time;
		beingPress = true;
		CancelInvoke("CheckLongPress");
		Invoke ("CheckLongPress", 1.0f);

        curPressObject = obj;// EventSystem.current.currentSelectedGameObject;

		eventParam.eventType = EventIdentity.uiPress;
		eventParam.gameobject = obj;

        EventListenerManager.Invoke( instanceId, eventParam);

	}

	public void OnDrag()
	{
		isDragged = true;
	}
	
	private void CheckLongPress()
	{
		if (beingPress && curPressObject != null && !isDragged)
		{
			eventParam.eventType = EventIdentity.uiLongClick;
			eventParam.gameobject = curPressObject;

            EventListenerManager.Invoke(instanceId,eventParam);

			beingPress = false;
		}
	}
	
	public void OnTriggerRelease(GameObject obj)
	{	
		eventParam.eventType = EventIdentity.uiRelease;
		eventParam.gameobject = obj;

        EventListenerManager.Invoke( instanceId,eventParam);

		curPressObject = null;

        beingPress = false;
    }
}



