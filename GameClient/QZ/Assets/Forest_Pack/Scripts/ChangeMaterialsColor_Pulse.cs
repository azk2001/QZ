using UnityEngine;
using System.Collections;

public class ChangeMaterialsColor_Pulse : MonoBehaviour {

	public bool ChangeElement0 = false;
	private Color colorStart1 = Color.red;
	public Color EndColor1 = Color.green;

	public bool ChangeElement1 = false;
	private Color colorStart2 = Color.red;
	public Color EndColor2 = Color.green;

	public bool ChangeElement2 = false;
	private Color colorStart3 = Color.red;
	public Color EndColor3 = Color.green;
	 
	public bool ChangeElement3 = false;
	private Color colorStart4 = Color.red;
	public Color EndColor4 = Color.green;
	 
	public bool ChangeElement4 = false;
	private Color colorStart5 = Color.red;
	public Color EndColor5 = Color.green;
	
	public float TimeToChange = 2.0f;

	private Renderer tree;

	void Start() {
		tree = GetComponent<Renderer>();
		colorStart1 = tree.materials[0].color;
		colorStart2 = tree.materials[1].color;
		colorStart3 = tree.materials[2].color;
		colorStart4 = tree.materials[3].color;
		colorStart5 = tree.materials[4].color;

		
	}

	void Update() 
	{
		float lerp = Mathf.PingPong(Time.time, TimeToChange) / TimeToChange;

		if (ChangeElement0 == true) {
			tree.materials[0].color = Color.Lerp(colorStart1, EndColor1, lerp);
		}
		if (ChangeElement1 == true) {
			tree.materials[1].color = Color.Lerp(colorStart2, EndColor2, lerp);
		}
		if (ChangeElement2 == true) {
			tree.materials[2].color = Color.Lerp(colorStart3, EndColor3, lerp);
		}
		if (ChangeElement3 == true) {
			tree.materials[3].color = Color.Lerp(colorStart4, EndColor4, lerp);
		}
		if (ChangeElement4 == true) {
			tree.materials[4].color = Color.Lerp(colorStart5, EndColor5, lerp);
		}
	}
}