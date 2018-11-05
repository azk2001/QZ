using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementList : MonoBehaviour {

    public List<Transform> elementList = new List<Transform>();

    public T GetUIComponent<T>(int index) where T : MonoBehaviour
    {
        if (index < 0 || index >= elementList.Count)
        {
            return default(T);
        }

        Transform trans = elementList[index];

        if (trans)
        {
            return trans.GetComponent<T>();
        }

        return default(T);
    }
}
