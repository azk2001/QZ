using UnityEngine;
using System.Collections;

public class Water_waves : MonoBehaviour {
	
	public Color colorStart = Color.red;
	public Color colorEnd = Color.green;
	
	public float duration = 2.0f;
	private Renderer bud1;
	private float delay;

	void Start() {
		bud1 = GetComponent<Renderer>();
		delay = duration / 3;
		}
	void Update() 
	{
		float lerp = Mathf.PingPong(Time.time, duration) / duration;
		bud1.materials[1].color = Color.Lerp(colorStart, colorEnd, lerp);


		float lerp2 = Mathf.PingPong(Time.time+delay, duration) / duration;
		bud1.materials[0].color = Color.Lerp(colorStart, colorEnd, lerp2);

		float lerp3 = Mathf.PingPong(Time.time+delay+delay, duration) / duration;
		bud1.materials[2].color = Color.Lerp(colorStart, colorEnd, lerp3);

		
	}
}
