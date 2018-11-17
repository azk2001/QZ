using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameMain : UIBase {

    public override eUIDepth uiDepth
    {
        get
        {
            return eUIDepth.ui_base;
        }
    }

    public override bool showing
    {
        get
        {
            return true;
        }
    }

    private enum eObjectIndex
    {
        modleParent,
    }

    public UIGameMain Instance = null;

    public SpinWithMouse modleParent = null;

    private Transform prefabModel = null;

    public override void OnAwake(GameObject obj)
    {

        base.OnAwake(obj);

        Instance = this;
        
        modleParent = gameObjectList.GetUIComponent<SpinWithMouse>((int)eObjectIndex.modleParent);
    }

    public override void OnInit()
    {
        base.OnInit();

        RefreshModle();
    }

    public void RefreshModle()
    {
        string str = LocalPlayer.Instance.netPlayer.GetModleStr();
        prefabModel = BattleUnitRoot.Instance.SpwanPrefab(str);
        prefabModel.SetParent(modleParent.transform);
        prefabModel.SetLayers("UI");
        prefabModel.Reset();
        prefabModel.localScale = Vector3.one*150;
        prefabModel.localEulerAngles = Vector3.up * 180;
        modleParent.target = prefabModel;

        UnitController mUnitController = prefabModel.GetComponent<UnitController>();
        mUnitController.Init();
        mUnitController.SetCharacterControllerEnable(false);
    }


    public override void OnDisable()
    {
        base.OnDisable();
   
        BattleUnitRoot.Instance.DeSpwan(prefabModel);
        
    }

}
