using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIMask :Mask 
{
	protected override void Start ()
	{
		base.Start ();
        Change();
    }
    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        Change();//重新再给材质赋裁切参数
    }

    private void Change()
    {

        Vector3[] corners = new Vector3[4];
        RectTransform rectTransform = transform as RectTransform;
        rectTransform.GetWorldCorners(corners);
        float minX = corners[0].x;
        float minY = corners[0].y;
        float maxX = corners[2].x;
        float maxY = corners[2].y;

        List<Renderer> rendererList = new List<Renderer>();
        rendererList.AddRange(transform.GetComponentsInChildren<Renderer>());



        //ParticleSystem[] particlesSystems = transform.GetComponentsInChildren<ParticleSystem>();
       
        ////这里 100  是因为ugui默认的缩放比例是100  你也可以去改这个值，但是我觉得最好别改。
        //foreach (ParticleSystem particleSystem in particlesSystems)
        //{
        //    rendererList.Add(particleSystem.GetComponent<Renderer>());
           
        //}
        foreach (Renderer renderer in rendererList)
        {
            Material m = renderer.sharedMaterial;
            m.SetFloat("_MinX", minX);
            m.SetFloat("_MinY", minY);
            m.SetFloat("_MaxX", maxX);
            m.SetFloat("_MaxY", maxY);
        }
       
    }
}
