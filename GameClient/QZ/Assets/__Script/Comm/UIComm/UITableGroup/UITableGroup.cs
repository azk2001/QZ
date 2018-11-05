using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITableGroup : MonoBehaviour
{

    public enum Direction
    {
        Down,
        Up,
    }

    /// <summary>
    /// Which way the new lines will be added.
    /// </summary>

    public Direction direction = Direction.Down;

    /// <summary>
    /// Whether inactive children will be discarded from the table's calculations.
    /// </summary>

    public bool hideInactive = true;

    /// <summary>
    /// Padding around each entry, in pixels.
    /// </summary>

    public Vector2 padding = Vector2.zero;

    public float offsetY = 0;

    protected bool mInitDone = false;
    protected bool mReposition = false;


    /// <summary>
    /// Get the current list of the grid's children.
    /// </summary>

    public List<Transform> GetChildList(Transform trans)
    {
        List<Transform> list = new List<Transform>();

        for (int i = 0; i < trans.childCount; ++i)
        {
            Transform t = trans.GetChild(i);
            if(hideInactive == false || t.gameObject.activeSelf == true)
            {
                list.Add(t);
            }     
        }

        // list.Sort(SortVertical);

        return list;
    }

    private int SortVertical(Transform a, Transform b) { return b.localPosition.y.CompareTo(a.localPosition.y); }

    /// <summary>
    /// Position the grid's contents when the script starts.
    /// </summary>

    protected void Start()
    {
        Reposition();
        enabled = false;
    }


    /// <summary>
    /// Is it time to reposition? Do so now.
    /// </summary>

    protected void LateUpdate()
    {
        if (mReposition) Reposition();
        enabled = false;
    }

    /// <summary>
    /// Positions the grid items, taking their own size into consideration.
    /// </summary>

    protected float RepositionVariableSize(List<Transform> children)
    {
        float yOffset = 0;

        for (int i = 0, imax = children.Count; i < imax; ++i)
        {
            RectTransform rect = children[i] as RectTransform;
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);

            Vector2 sizeDelta = rect.sizeDelta;
            Vector3 pos = rect.localPosition;

            if (direction == Direction.Down)
            {
                pos.y = yOffset;
                yOffset -= sizeDelta.y + padding.y * 2f;
            }
            else
            {
                pos.y = yOffset;
                yOffset += sizeDelta.y + padding.y * 2f;
            }
            rect.localPosition = pos;

        }

        return yOffset;
    }



    /// <summary>
    /// Recalculate the position of all elements within the table, sorting them alphabetically if necessary.
    /// </summary>

    [ContextMenu("Execute")]
    public virtual void Reposition()
    {
        float yOffset = 0;

        UITableIndex[] tableIndexList = this.GetComponentsInChildren<UITableIndex>();

        for (int i = 0, max = tableIndexList.Length; i < max; i++)
        {
            UITableIndex tableIndex = tableIndexList[i];
            RectTransform myTrans = tableIndex.transform as RectTransform;
            myTrans.anchorMin = new Vector2(0, 0.5f);
            myTrans.anchorMax = new Vector2(0, 0.5f);
            myTrans.pivot = new Vector2(0, 0.5f);

            Vector3 pos = myTrans.anchoredPosition;
            pos.y = yOffset;
            pos.x = padding.x;
            myTrans.anchoredPosition = pos;

            List<Transform> ch = GetChildList(myTrans);
            
            if (ch.Count > 0)
                yOffset += RepositionVariableSize(ch);
        }

        mReposition = false;

        offsetY = Mathf.Abs(yOffset);
    }
}
