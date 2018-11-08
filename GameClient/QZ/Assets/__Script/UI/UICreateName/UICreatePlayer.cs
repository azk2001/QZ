using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreatePlayer : UIBase
{
    public override eUIDepth uiDepth
    {
        get
        {
            return eUIDepth.ui_system;
        }
    }
    public override bool TweenAni
    {
        get { return false; }
    }
    public override bool showing
    {
        get
        {
            return false;
        }
    }

    private enum eObjectIndex
    {
        UITable_Sex,
        PrafabParent,
        Txt_Name,
        TableDesc,
    }

    private enum eElementTableDescIndex
    {
        ManDesc,
        WomanDesc,
    }

    private UITable UITable_Sex = null;
    private SpinWithMouse PrafabParent = null;
    private InputField Input_Name = null;
    private UIElementList TableDesc = null;

    public string curName = "";
    public string randNames = "";
    private bool isMan = false;

    private string[] resModleList = new string[2] { "PlayerNan1", "PlayerNv1" };

    public UnitController mUnitControl = null;
    private string curModleName = "";

    private Transform ManDesc = null;
    private Transform WomanDesc = null;
    private Transform curDesc = null;

    private Transform lastModle = null;

    public override void OnAwake(GameObject obj)
    {
        base.OnAwake(obj);

        UITable_Sex = gameObjectList.gameObjectList[(int)eObjectIndex.UITable_Sex].GetComponent<UITable>();
        PrafabParent = gameObjectList.gameObjectList[(int)eObjectIndex.PrafabParent].GetComponent<SpinWithMouse>();
        Input_Name = gameObjectList.gameObjectList[(int)eObjectIndex.Txt_Name].GetComponent<InputField>();
        TableDesc = gameObjectList.GetUIComponent<UIElementList>((int)eObjectIndex.TableDesc);


        ManDesc = TableDesc.elementList[(int)eElementTableDescIndex.ManDesc];
        WomanDesc = TableDesc.elementList[(int)eElementTableDescIndex.WomanDesc];

        ManDesc.gameObject.SetActive(false);
        WomanDesc.gameObject.SetActive(false);

        List<UITableStyle> styles = new List<UITableStyle>();

        UITableStyle style = new UITableStyle();
        style.msgTxt = "btn_nan";
        style.tabName = "男";
        style.isStartShow = true;
        styles.Add(style);

        UITableStyle style1 = new UITableStyle();
        style1.msgTxt = "btn_nv";
        style1.tabName = "女";
        styles.Add(style1);

        UITable_Sex.SetTableNum(styles);

        isMan = true;
    }

    public override void OnInit()
    {
        RefreshUI();

        RandomName();

        base.OnInit();
    }

    public override void OnDisable()
    {
      
    }

    public void RefreshUI()
    {
        string prefabName = "";
        if (isMan)
        {
            prefabName = resModleList[0];
        }
        else
        {
            prefabName = resModleList[1];
        }

        if (curModleName == prefabName)
            return;

        curModleName = prefabName;

        AssetBundleLoadManager.Instance.LoadObject(prefabName, eLoadPriority.advanced, new object[] { }, OnLoadControlFinish);
    }

    private void OnLoadControlFinish(UnityEngine.Object obj, object[] pars)
    {
        if(lastModle!= null)
        {
            GameObject.Destroy(lastModle.gameObject);
        }

        GameObject go = (GameObject)GameObject.Instantiate<Object>(obj);
        
        go.SetActive(true);
        go.transform.Reset();
        go.transform.localEulerAngles = Vector3.up * 180;

        PrafabParent.target = go.transform;

        lastModle = go.transform;
    }


    public void RandomName()
    {
       

    }

    private void CreatePlayer()
    {
        curName = Input_Name.text;

        
        int namecharlen = CountStringNum(curName);

        byte sex = isMan == true ? (byte)1 : (byte)2;

        C2SCreatePlayerMessage c2SCreatePlayer = new C2SCreatePlayerMessage();
        c2SCreatePlayer.name = curName;
        c2SCreatePlayer.sex = sex;
        c2SCreatePlayer.level = 1;

        BattleProtocolEvent.SendCreatePlayer(c2SCreatePlayer);

    }

    private int CountStringNum(string str)
    {
        int num = 0;

        if (HasInvalidChar(str))
            return -1;

        for (int i = 0; i < str.Length; i++)
        {
            if (IsChineseLetter(str, i))
            {
                num += 2;
            }
            else
            {
                ++num;
            }
        }
        return num;
    }

    private bool HasInvalidChar(string str)
    {
        return (str.IndexOf(' ') >= 0) || str != str.Trim() || str.IndexOf('\r') >= 0 || str.IndexOf('\n') >= 0;
    }


    private bool IsChineseLetter(string input, int index)
    {
        int code = 0;
        int chfrom = System.Convert.ToInt32("4e00", 16);
        int chend = System.Convert.ToInt32("9fff", 16);

        code = System.Char.ConvertToUtf32(input, index);
        if (code >= chfrom && code <= chend)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public override void OnClick(GameObject clickObject)
    {
        switch (clickObject.name)
        {
            case "Btn_Create":

                CreatePlayer();

                break;
            case "Btn_Rand":

                RandomName();

                break;
            case "btn_nan":

                isMan = true;

                RefreshUI();

                break;
            case "btn_nv":

                isMan = false;

                RefreshUI();

                break;
            case "Btn_Return":
                {
                   
                }
                break;
        }

        base.OnClick(clickObject);
    }

    public void OnCreateFinish()
    {
        UIManager.Instance.CloseUI(eUIName.UICreatePlayer, false, true);
        UIManager.Instance.OpenUI(eUIName.UIGameMain);

        ProcessManager.Instance.Begin(ProcessType.processmain);
    }
}

