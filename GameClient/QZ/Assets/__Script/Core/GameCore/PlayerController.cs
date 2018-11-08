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

        CameraLookPlayer.Instance.targetTrans = gameUnit.mUnitController.transform;

        joyStick.OnDragEvent += OnJoystickEvent;
    }

    //移除一个玩家控制;
    public void RemovePlayerController()
    {
        if (mGameUnit == null)
            return;

        mGameUnit.PlayRunAnimation(Vector3.zero);
        mGameUnit = null;

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

        float angle = Angle(Vector3.forward, moveDir);
        if(moveDir.magnitude>0)
        {
            mGameUnit.mUnitController.RunAnimation(angle);
        }
        else
        {
            mGameUnit.mUnitController.RunAnimation(0);
        }
    }

    private float Angle(Vector3 from, Vector3 to)
    {

        float angle = Vector3.Angle(from, to);
        angle *= Mathf.Sign(Vector3.Cross(from, to).y);

        if (angle < 0)
        {
            angle = 360 - Mathf.Abs(angle);
        }

        return angle;
    }

    public bool OnClickButton(int val)
    {
        if (mGameUnit == null)
            return false;

        if (isInput == false)
            return false;

        ///发炸弹
		return mGameUnit.OnSkill(val);
    }
}
