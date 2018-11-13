using UnityEngine;
using System.Collections;

public class CampfireLightBounce : MonoBehaviour {

	float random;
	private Light lt;
	public float Noise = 7;
	public float RangeDown = 3;
	public float RangeUp = 8;

	void Start() {

		random = Random.Range(0f, 65535f);
		lt = GetComponent<Light>();

	}
	
	// Update is called once per frame
	void Update () {
	
		float noise = Mathf.PerlinNoise(random, Time.time*Noise);
		lt.intensity = Mathf.Lerp(RangeDown, RangeUp, noise);

	}
}
