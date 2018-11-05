using UnityEngine;
using UnityEngine.UI;

public class UIRaycaster : GraphicRaycaster
{
    public override int sortOrderPriority
    {
        get
        {
            Canvas canvas = GetComponent<Canvas>();
            return canvas.sortingOrder;
        }
    }
}
