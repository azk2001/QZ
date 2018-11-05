using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UITableItem : MonoBehaviour
{

    public UIText txt_text;     //显示文本;
    public int Select_TxtstyleId = 6;   //选中文本样式
    public Toggle tog_button;   //是否默认选中;
    public Image img_backSprite;    //背景显示图片; (不填就不显示)
    public Image img_showSprite;    //选中显示图片; (不填就不显示)
    public UIImage Img_Bt;
    [HideInInspector] public int param;
    [HideInInspector] public int NoSelect_TxtstyleId = 0;//未选中文本样式

    public UIImage img_State;
    public UITable subMenu;
    private bool _isShow;

    public UIImage img_green;   //绿点提示

    public bool IsShow
    {
        get
        {
            return _isShow;
        }

        set
        {
            _isShow = value;
            if (img_State != null)
            {
                img_State.transform.localEulerAngles = _isShow ? 180 * Vector3.right : Vector3.zero;
            }
            if (subMenu != null && subMenu.gameObject != null)
            {
                subMenu.gameObject.SetActive(_isShow);
            }
        }
    }

    public void Init(UITableStyle style)
    {
        txt_text.text = style.tabName;
        if (Select_TxtstyleId <= 0)
            Select_TxtstyleId = txt_text.styleId;
        if (NoSelect_TxtstyleId <= 0)
            NoSelect_TxtstyleId = txt_text.styleId;
        tog_button.isOn = style.isStartShow;
        if (style.backSprite != null) img_backSprite.sprite = style.backSprite;
        if (style.showSprite != null) img_showSprite.sprite = style.showSprite;
        if (Img_Bt != null)
        {
            Img_Bt.name = style.msgTxt;
        }
        tog_button.gameObject.name = style.msgTxt;
        param = style.param;
    }
}
