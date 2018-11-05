using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEvent : MonoBehaviour {

	public float mLength = 0;

	public Transform startEffect = null;
	public Transform endEffect = null;

	public List<Transform> startEffectList = new List<Transform>();

	private BoxCollider boxCollider = null;

	void Awake()
	{
		boxCollider = this.GetComponent<BoxCollider>();
		startEffect.gameObject.SetActive (false);
		endEffect.gameObject.SetActive (false);

	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void HideEffect()
	{
		for (int i=0,max = startEffectList.Count; i<max; i++)
		{
			startEffectList[i].gameObject.SetActive(false);
		}
		endEffect.gameObject.SetActive (false);
	}

	private Transform GetStartEffect()
	{
		Transform temp = null;
		for (int i=0,max = startEffectList.Count; i<max; i++)
		{
			if(startEffectList[i].gameObject.activeSelf == false)
			{
				temp =  startEffectList[i];
				break;
			}
		}

		if (temp == null) {
			GameObject go = GameObject.Instantiate(startEffect.gameObject) as GameObject;
			go.transform.parent = this.transform;
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = new Vector3(90,0,0);
			go.transform.localScale = new Vector3(0.7f,1.01f,1);
			temp = go.transform;

			startEffectList.Add(go.transform);
		}

		temp.gameObject.SetActive (true);

		return temp;
	}

	public void Init(int length)
	{
		mLength = length;

		HideEffect ();

		float startOffset = 1.0f;

		boxCollider.center = new Vector3 (0, 0, length / 2);
		boxCollider.size = new Vector3 (0.5f, 2, length-0.1f);

		float maxStartOffext = startOffset;

		endEffect.gameObject.SetActive (true);

	}

	public void OnTriggerEnter(Collider other)
	{

	}
}
