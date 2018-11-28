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
            return false;
        }
    }

    private enum eObjectIndex
    {
        modleParent,
    }

    public UIGameMain Instance = null;
    
    private UIShowModel showPrefab = null;

    private Transform prefabModel = null;

    public override void OnAwake(GameObject obj)
    {

        base.OnAwake(obj);

        Instance = this;
        
        showPrefab = gameObjectList.GetUIComponent<UIShowModel>((int)eObjectIndex.modleParent);
    }

    public override void OnInit()
    {
        base.OnInit();

        RefreshModle();
    }

    public void RefreshModle()
    {
        string resName = LocalPlayer.Instance.netPlayer.GetModleStr();
        prefabModel = BattleUnitRoot.Instance.SpwanPrefab(resName);
        showPrefab.ShowPrefab(prefabModel.gameObject, 0, Vector3.up * -1, Vector3.up * 180, 1.45f);

        UnitController mUnitController = prefabModel.GetComponent<UnitController>();
        mUnitController.Init();
        mUnitController.SetCharacterControllerEnable(false);
        mUnitController.enabled = false;
    }


    public override void OnDisable()
    {
        base.OnDisable();

        if(prefabModel !=null)
        {
            BattleUnitRoot.Instance.DeSpwan(prefabModel);
        }
        
    }

    public override void OnClick(GameObject clickObject)
    {
        switch(clickObject.name)
        {
            case "Btn_Custom":
                {
                    SendGetRoom();
                }
                break;
        }

        base.OnClick(clickObject);
    }


    public void SendGetRoom()
    {
        C2SGetRoomMessage c2SGetRoom = new C2SGetRoomMessage();
        c2SGetRoom.uuid = BattleProtocol.UUID;

        BattleProtocolEvent.SendGetRoom(c2SGetRoom);

    }
}
