using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioItem : MonoBehaviour {

	private AudioSource aSource = null;
	private float frequency = 0.2f;
	private float dataTime =0;
	private bool isHide = true;
	void Awake()
	{
		aSource = this.GetComponent<AudioSource> ();
	}

	void OnEnable ()
	{
		isHide = false;
	}

	void Update()
	{
		dataTime += Time.deltaTime;
		if (dataTime < frequency)
			return;

		dataTime = 0;
		if (aSource.loop == false && aSource.isPlaying == false)
		{
			AudioRoot.instance.DeSpwan (this.transform);
			isHide = true;
		}
	}
}
