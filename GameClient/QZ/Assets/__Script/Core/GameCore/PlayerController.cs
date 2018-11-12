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

        //设置前向;
        Vector3 forward = mGameUnit.mUnitController.transformCaChe.position - mainCamera.transform.position;
        mGameUnit.SetForward(forward);

        //客服端直接移动;
        mGameUnit.PlayRunAnimation(moveDir);

        C2SPlayerMoveMessage c2SPlayerMove = new C2SPlayerMoveMessage();
        c2SPlayerMove.uuid = mGameUnit.basicsData.uuid;
        c2SPlayerMove.fx = (int)(forward.x * 100);
        c2SPlayerMove.fy = (int)(forward.y * 100);
        c2SPlayerMove.fz = (int)(forward.z * 100);
        c2SPlayerMove.mx = (int)(moveDir.x * 100);
        c2SPlayerMove.my = (int)(moveDir.y * 100);
        c2SPlayerMove.mz = (int)(moveDir.z * 100);
        c2SPlayerMove.px = (int)(mGameUnit.mUnitController.transformCaChe.position.x * 100);
        c2SPlayerMove.py = (int)(mGameUnit.mUnitController.transformCaChe.position.y * 100);
        c2SPlayerMove.pz = (int)(mGameUnit.mUnitController.transformCaChe.position.z * 100);

        BattleProtocolEvent.SendPlayerMove(c2SPlayerMove);

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

    //技能开火控制;
    public void OnClickButton(int val)
    {
        if (mGameUnit == null)
            return;

        if (isInput == false)
            return;

        bool isSendSkill = mGameUnit.OnSkill(val);

        if (isSendSkill == true)
        {
            OnJoystickEvent(moveDir);

            C2SPlayerSkillMessage c2SPlayerSkill = new C2SPlayerSkillMessage();
            c2SPlayerSkill.uuid = mGameUnit.basicsData.uuid;
            c2SPlayerSkill.skillIndex = val;

            BattleProtocolEvent.SendPlayerSkill(c2SPlayerSkill);
        }

    }
}
