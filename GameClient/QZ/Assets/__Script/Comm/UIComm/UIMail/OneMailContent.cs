using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 一封具体邮件内容
/// </summary>
public class OneMailContent : MonoBehaviour
{
    /// <summary>
    /// 邮件附件
    /// </summary>
    public UIGeneralItem[] adjunctArr;

    public UIScrollView adjScrollView;
    /// <summary>
    /// 邮件概述
    /// </summary>
    public UIMailItem mailOverView;
    /// <summary>
    /// 邮件内容
    /// </summary>
    public Text contentStr;
    /// <summary>
    /// 功能按钮
    /// </summary>
    public UIButton btn_funtion;
    private GridLayoutGroup _contentGrid;
    public GridLayoutGroup ContentGrid
    {
        get
        {
            if (_contentGrid == null)
            {
                if (adjScrollView != null && adjScrollView.content != null)
                    _contentGrid = adjScrollView.content.GetComponent<GridLayoutGroup>();
            }
            return _contentGrid;
        }
    }
    /// <summary>
    /// 显示隐藏
    /// </summary>
    /// <param name="show">true显示</param>
    public void ShowScrollView(bool show)
    {
        if (adjScrollView != null) adjScrollView.gameObject.SetActive(show);
    }
}
