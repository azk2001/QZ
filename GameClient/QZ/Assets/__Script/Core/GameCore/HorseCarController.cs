using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseCarController : MonoBehaviour {

	private CharacterController _characterController = null;
	private Transform transformCaChe = null;
	private Animator mAnimator = null;
	private Renderer[] rendererList = null;

	void Awake()
	{
		_characterController = this.GetComponent<CharacterController>();
		transformCaChe = this.transform;
		mAnimator = this.GetComponentInChildren<Animator>();
		rendererList = transformCaChe.Find("Model").GetComponentsInChildren<Renderer>();
	}

	public void SetColor(Color color,float val)
	{
		for (int i = 0, max = rendererList.Length; i < max; i++)
		{
			rendererList[i].material.SetColor("_RimColor", color);
			rendererList[i].material.SetFloat("_RimParam", val);
		}
	}
	
	public void SetAlpha(float val)
	{
		for (int i = 0, max = rendererList.Length; i < max; i++)
		{
			rendererList[i].material.SetFloat("_Alpha", val);
		}
	}
	
	public void ShowRenderer(bool isShow)
	{
		for (int i=0,max = rendererList.Length; i<max; i++) {
			rendererList[i].enabled= isShow;
		}
	}

	public void PlayAnimation(string aniName,string layerName)
	{
		if (mAnimator == null)
			return;

		int layerIndex = mAnimator.GetLayerIndex (layerName);

		mAnimator.Play(aniName,layerIndex);
	}
	
	public void PlayAnimation(string param, bool flag)
	{
		if (mAnimator == null)
			return;
		
		mAnimator.SetBool(param, flag);
	}
	
	public void PlayAnimation(string param, int val)
	{
		if (mAnimator == null)
			return;
		
		mAnimator.SetInteger(param, val);
	}
	public void PlayAnimation(string param, float val)
	{
		if (mAnimator == null)
			return;
		
		mAnimator.SetFloat(param, val);
	}
	
	public void SetAnimationSpeed(float speed)
	{
		mAnimator.speed = speed;
	}
}
