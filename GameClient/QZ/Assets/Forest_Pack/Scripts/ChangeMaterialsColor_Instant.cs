using UnityEngine;
using System.Collections;

public class ChangeMaterialsColor_Instant : MonoBehaviour {

	public bool ChangeElement0 = false;
	public Color EndColor1 = Color.green;

	public bool ChangeElement1 = false;
	public Color EndColor2 = Color.green;

	public bool ChangeElement2 = false;
	public Color EndColor3 = Color.green;
	 
	public bool ChangeElement3 = false;
	public Color EndColor4 = Color.green;
	 
	public bool ChangeElement4 = false;
	public Color EndColor5 = Color.green;
	
	private Renderer tree;

	void Start() {
		tree = GetComponent<Renderer>();
		}

	void Update() 
	{

		if (ChangeElement0 == true) {
			tree.materials[0].color = EndColor1;
		}
		if (ChangeElement1 == true) {
			tree.materials[1].color = EndColor2;
		}
		if (ChangeElement2 == true) {
			tree.materials[2].color = EndColor3;
		}
		if (ChangeElement3 == true) {
			tree.materials[3].color = EndColor4;
		}
		if (ChangeElement4 == true) {
			tree.materials[4].color = EndColor5;
		}
	}
}