using UnityEngine;
using System.Collections;

//玩家控制脚本;
public class PlayerController : SingleClass<PlayerController>
{

    public GameUnit mGameUnit = null;
    public float aniTransSpeed = 5.0f;

    private Vector3 moveDir = Vector3.zero;  //移动方向;
    private bool isInput = false;

    private Camera mainCamera
    {
        get
        {
            return Camera.main;
        }
    }

    private UIJoyStick joyStick
    {
        get
        {
            return UIJoyStick.Instance;
        }
    }

    //添加一个玩家控制;
    public void AddPlayerController(GameUnit gameUnit)
    {
        isInput = true;
        mGameUnit = gameUnit;

        CameraLookPlayer.Instance.SetTarget(gameUnit.mUnitController.transform);

        UIGameMain.Instance.OnFireEvent += OnClickButton;
        joyStick.OnDragEvent += OnJoystickEvent;
    }

    //移除一个玩家控制;
    public void RemovePlayerController()
    {
        if (mGameUnit == null)
            return;

        mGameUnit.PlayRunAnimation(Vector3.zero);
        mGameUnit = null;

        CameraLookPlayer.Instance.SetTarget(null);

        UIGameMain.Instance.OnFireEvent -= OnClickButton;
        joyStick.OnDragEvent -= OnJoystickEvent;
    }

    private void OnJoystickEvent(Vector2 v2)
    {
        if (mGameUnit == null)
            return;

        if (isInput == false)
            return;

        v2 = v2.normalized;

        if (moveDir.x == v2.x && moveDir.z == v2.y)
        {
            return;
        }

        moveDir.x = v2.x;
        moveDir.z = v2.y;

        Debug.Log(moveDir);

        //客服端直接移动;
        mGameUnit.PlayRunAnimation(moveDir);

        //设置前向;
        Vector3 forward = mGameUnit.mUnitController.transformCaChe.position - mainCamera.transform.position;
        mGameUnit.SetForward(forward);

    }

    //技能开火控制;
    public void OnClickButton(int val)
    {
        if (mGameUnit == null)
            return;

        if (isInput == false)
            return;

        switch (val)
        {
            case 0:
                {
                    mGameUnit.OnSkill(0,false);
                }
                break;
            case 1:
                {
                    mGameUnit.OnSkill(0, true);
                }
                break;
            case 2:
                {
                    mGameUnit.OnSkill(1);
                }
                break;
            case 3:
                {
                    mGameUnit.OnSkill(2);
                }
                break;
        }

    }
}
