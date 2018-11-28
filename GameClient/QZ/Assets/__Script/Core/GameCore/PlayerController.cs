using UnityEngine;
using System.Collections;

//玩家控制脚本;
public class PlayerController : SingleClass<PlayerController>
{
    public GameUnit mGameUnit = null;
    public float aniTransSpeed = 5.0f;

    private Vector3 moveDir = Vector3.zero;  //移动方向;
    public bool isInput = false;

    private Camera mainCamera
    {
        get
        {
            return Camera.main;
        }
    }

    private UIMoveJoystick joyStick
    {
        get
        {
            return UIMoveJoystick.Instance;
        }
    }

    private UIRollJoystick rollJoystick
    {
        get
        {
            return UIRollJoystick.Instance;
        }
    }

    private UIAtkJoystick atkJoystick
    {
        get
        {
            return UIAtkJoystick.Instance;
        }
    }

    private UISkill1Joystick skill1Joystick
    {
        get
        {
            return UISkill1Joystick.Instance;
        }
    }

    private UISkill2Joystick skill2Joystick
    {
        get
        {
            return UISkill2Joystick.Instance;
        }
    }

    //添加一个玩家控制;
    public void AddPlayerController(GameUnit gameUnit)
    {
        isInput = true;
        mGameUnit = gameUnit;

        CameraLookPlayer.Instance.SetTarget(gameUnit.mUnitController.transform);

        if (joyStick != null) joyStick.OnMoveDragEvent += OnMoveJoystickEvent;
        if (rollJoystick != null) rollJoystick.OnRollDragEvent += OnRollJoystickEvent;
        if (atkJoystick != null) atkJoystick.OnFireEvent += OnFireEvent;
        if (skill1Joystick != null) skill1Joystick.OnFireEvent += OnFireEvent;
        if (skill2Joystick != null) skill2Joystick.OnFireEvent += OnFireEvent;
    }

    //移除一个玩家控制;
    public void RemovePlayerController()
    {
        if (mGameUnit == null)
            return;

        mGameUnit.MoveDirection(Vector3.zero);
        mGameUnit = null;

        CameraLookPlayer.Instance.SetTarget(null);

        if (joyStick != null) joyStick.OnMoveDragEvent -= OnMoveJoystickEvent;
        if (rollJoystick != null) rollJoystick.OnRollDragEvent -= OnRollJoystickEvent;
        if (atkJoystick != null) atkJoystick.OnFireEvent -= OnFireEvent;
        if (skill1Joystick != null) skill1Joystick.OnFireEvent -= OnFireEvent;
        if (skill2Joystick != null) skill2Joystick.OnFireEvent -= OnFireEvent;
    }

    private void OnMoveJoystickEvent(Vector2 v2)
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

        Vector3 newMoveDir = Quaternion.AngleAxis(Angle(Vector3.forward, moveDir), Vector3.up) * forward;
        if (moveDir == Vector3.zero)
        {
            newMoveDir = Vector3.zero;
        }

        //客服端直接移动;
        mGameUnit.MoveDirection(newMoveDir);

        C2SPlayerMoveMessage c2SPlayerMove = new C2SPlayerMoveMessage();
        c2SPlayerMove.uuid = mGameUnit.basicsData.uuid;
        c2SPlayerMove.fx = (int)(forward.x * 100);
        c2SPlayerMove.fy = (int)(forward.y * 100);
        c2SPlayerMove.fz = (int)(forward.z * 100);
        c2SPlayerMove.mx = (int)(newMoveDir.x * 100);
        c2SPlayerMove.my = (int)(newMoveDir.y * 100);
        c2SPlayerMove.mz = (int)(newMoveDir.z * 100);
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
    public void OnFireEvent(int val, Vector3 forward)
    {
        if (mGameUnit == null)
            return;

        if (isInput == false)
            return;

        bool isSendSkill = mGameUnit.OnSkill(val, forward);

        if (isSendSkill == true)
        {
            OnMoveJoystickEvent(moveDir);

            C2SPlayerSkillMessage c2SPlayerSkill = new C2SPlayerSkillMessage();
            c2SPlayerSkill.uuid = mGameUnit.basicsData.uuid;
            c2SPlayerSkill.skillIndex = val;
            c2SPlayerSkill.px = (int)(mGameUnit.mUnitController.transformCaChe.position.x * 100);
            c2SPlayerSkill.py = (int)(mGameUnit.mUnitController.transformCaChe.position.y * 100);
            c2SPlayerSkill.pz = (int)(mGameUnit.mUnitController.transformCaChe.position.z * 100);
            c2SPlayerSkill.fx = (int)(forward.x * 100);
            c2SPlayerSkill.fy = (int)(forward.y * 100);
            c2SPlayerSkill.fz = (int)(forward.z * 100);

            BattleProtocolEvent.SendPlayerSkill(c2SPlayerSkill);
        }
    }

    public void OnRollJoystickEvent(Vector2 v2)
    {
        if (mGameUnit == null)
            return;

        if (isInput == false)
            return;

        if (v2 == Vector2.zero)
            return;

        Vector3 startPoint = mGameUnit.mUnitController.transformCaChe.position;
        Vector3 forward = mGameUnit.mUnitController.transformCaChe.position - mainCamera.transform.position;
        Vector3 dir = new Vector3(v2.x, 0, v2.y);

        Vector3 moveForward = Quaternion.AngleAxis(Angle(Vector3.forward, dir), Vector3.up) * forward;

        Vector3 endPoint = startPoint + moveForward.normalized * 1.5f;

        //客服端直接移动;
        mGameUnit.RollPoint(endPoint);

        C2SPlayerRollMessage c2SPlayerRoll = new C2SPlayerRollMessage();
        c2SPlayerRoll.uuid = mGameUnit.basicsData.uuid;
        c2SPlayerRoll.sx = (int)(startPoint.x * 100);
        c2SPlayerRoll.sy = (int)(startPoint.y * 100);
        c2SPlayerRoll.sz = (int)(startPoint.z * 100);
        c2SPlayerRoll.ex = (int)(endPoint.x * 100);
        c2SPlayerRoll.ey = (int)(endPoint.y * 100);
        c2SPlayerRoll.ez = (int)(endPoint.z * 100);

        BattleProtocolEvent.SendPlayerRoll(c2SPlayerRoll);
    }
}
