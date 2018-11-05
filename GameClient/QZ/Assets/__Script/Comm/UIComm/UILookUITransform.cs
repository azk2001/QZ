using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookUITransform : MonoBehaviour
{
    public RectTransform contTrans = null;
    public RectTransform lookTrans = null;

    public Vector3 offset = Vector3.zero;
    

    private void Awake()
    {
        if (contTrans == null)
            contTrans = this.GetComponent<RectTransform>();
        
    }

    void LateUpdate() 
    {
        if (lookTrans == null|| contTrans == null)
            return;

        contTrans.position = lookTrans.position+ offset;
    }
}
