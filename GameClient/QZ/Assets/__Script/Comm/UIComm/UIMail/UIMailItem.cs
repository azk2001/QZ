using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMailItem : MonoBehaviour
{
    public UIText senderName;
    public UIText validTime;
    public UIText title;
    public UIImage readFlag;
    public UIImage mailIcon;
    public UIImage state;
    public UIImage haveRes;
    public UIText body;
    /// <summary>
    /// 传递参数
    /// </summary>
    public int mgrPara;
}
