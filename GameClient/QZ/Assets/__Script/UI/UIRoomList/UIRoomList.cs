using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

class UIRoomList : UIBase
{
    public override eUIDepth uiDepth
    {
        get
        {
            return eUIDepth.ui_system;
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
        ModleParent,
    }


    public static UIRoomList Instance = null;

    public static S2CGetRoomMessage message;
    private UIShowModel showModel = null;
    private Transform prefabModel = null;

    public override void OnAwake(GameObject obj)
    {
        Instance = this;
        base.OnAwake(obj);

        showModel = gameObjectList.GetUIComponent<UIShowModel>((int)eObjectIndex.ModleParent);

    }

    public override void OnInit()
    {
        base.OnInit();

        RefreshModle();
    }

    public override void OnEnable()
    {
        base.OnEnable();

    }

    public void RefreshModle()
    {
        string resName = LocalPlayer.Instance.netPlayer.GetModleStr();
        prefabModel = BattleUnitRoot.Instance.SpwanPrefab(resName);
        showModel.ShowPrefab(prefabModel.gameObject, 0, Vector3.up * -1, Vector3.up * 180, 1.45f);

        UnitController mUnitController = prefabModel.GetComponent<UnitController>();
        mUnitController.Init();
        mUnitController.SetCharacterControllerEnable(false);
        mUnitController.enabled = false;
    }


    public override void OnDisable()
    {
        base.OnDisable();

        BattleUnitRoot.Instance.DeSpwan(prefabModel);

        Instance = null;

    }

    public override void OnDestroy()
    {
        base.OnDestroy();

    }

    //刷新房间列表;
    public void RefreshRoomList(S2CGetRoomMessage message)
    {

    }

    //创建房间列表;
    public void RefreshCreateRoom(S2CCreateRoomMessage message)
    {

    }

    //加入房间列表;
    public void RefreshAddRoom(S2CAddRoomMessage message)
    {

    }

    public override void OnClick(GameObject clickObject)
    {
        base.OnClick(clickObject);

        switch (clickObject.name)
        {
            case "Btn_CreateRoom":
                {
                    UIRoomListData.SendCreateRoom();
                }
                break;
            case "Btn_AddRoom":
                {
                    int roomIndex = 0;
                    UIRoomListData.SendAddRoom(roomIndex);
                }
                break;
        }
    }



}