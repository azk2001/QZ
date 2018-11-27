using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour
{
    public struct JumpParam
    {
        public float curDistance;
        public float maxDistance;
        public Vector3 forward;
        public Vector3 toPosition;
        public float speed;
    }

    public struct RollParam
    {
        public float curDistance;
        public float maxDistance;
        public Vector3 forward;
        public Vector3 toPosition;
        public float speed;
    }


    public enum PlayerState
    {
        idle,   //站立状态;
        run,    //奔跑状态;
        jump,   //跳跃状态;
        roll,   //翻滚状态;
        fire,   //释放技能状态;
        die,    //死亡状态;
    }

    public float moveSpeed = 0.2f;
    public int uuid = 0;

    public Transform teamShadow = null;         //队伍标志;
    public Transform transformCaChe = null;     //角色;
    
    public Vector2 markPosition = Vector2.zero;

    private CharacterController _characterController = null;
    public CharacterController characterController
    {
        get
        {
            return _characterController;
        }
    }

    private Animator mAnimator = null;
    private Vector3 moveDirection = Vector3.forward;
    private Vector3 directionCache = Vector3.zero;
    private Vector3 moveToDir = Vector3.zero;
    private Vector3 moveToPoint = Vector3.zero;
    private bool isMovePoint = false;    //是否朝着某一点移动;

    private PlayerState _playerState = PlayerState.idle;
    public PlayerState playerState
    {
        get
        {
            return _playerState;
        }
        set
        {
            _playerState = value;
        }
    }

    private Renderer[] rendererList = null;
    private JumpParam jumpParam;
    private RollParam rollParam;

    private Transform curShadow = null;

    void Awake()
    {
        Init();
    }

    void Start()
    {

    }

    public void Init()
    {
        _playerState = PlayerState.idle;

        _characterController = this.GetComponent<CharacterController>();
        transformCaChe = this.transform;
        mAnimator = this.GetComponentInChildren<Animator>();
        rendererList = transformCaChe.Find("Modle").GetComponentsInChildren<Renderer>();
        SetCharacterControllerEnable(true);
        mAnimator.speed = 1;
        isMovePoint = false;

        directionCache = Vector3.zero;
        moveDirection = Vector3.zero;
        moveToPoint = Vector3.zero;

        this.enabled = true;

        ShowRenderer(true);
    }

    void Update()
    {
        if (_characterController == null)
            return;

        //是否在地上;
        if (_characterController.isGrounded == false)
            moveDirection.y = -50;
        else
            moveDirection.y = 0;

        if (isMovePoint == true)
        {
            if (Vector3.Distance(transformCaChe.position, moveToPoint) > 0.2f)
            {
                moveToDir = moveToPoint - transformCaChe.position;
                MoveDirection(moveToDir * 8);
            }
            else
            {
                isMovePoint = false;
                MoveDirection(directionCache);
            }
        }

        if (_playerState == PlayerState.jump)
        {
            Vector3 dar = jumpParam.forward * jumpParam.speed * Time.deltaTime;

            transformCaChe.Translate(dar);

            jumpParam.curDistance += dar.magnitude;

            if (jumpParam.curDistance > jumpParam.maxDistance)
            {
                transformCaChe.position = jumpParam.toPosition;

                _playerState = PlayerState.idle;

                MoveDirection(moveDirection);
            }
        }

        if (_playerState == PlayerState.roll)
        {
            Vector3 dar = rollParam.forward * rollParam.speed * Time.deltaTime;

            transformCaChe.Translate(dar);

            rollParam.curDistance += dar.magnitude;

            if (rollParam.curDistance > rollParam.maxDistance)
            {
                transformCaChe.position = rollParam.toPosition;

                _playerState = PlayerState.idle;

                MoveDirection(moveDirection);
            }
        }


        if (_playerState != PlayerState.jump && _playerState != PlayerState.roll) 
        {
            if(_characterController.enabled == true)
                _characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    public void SetTeamShadow(bool isFriend)
    {
        teamShadow.gameObject.SetActive(true);
        if (isFriend == true)
        {
            curShadow = teamShadow.Find("shadow_friend");
        }
        else
        {
            curShadow = teamShadow.Find("shadow_enemy");
        }
        curShadow.gameObject.SetActive(true);

        Transform isself = teamShadow.Find("isself");
        isself.gameObject.SetActive(false);
    }

    //进入碰撞;
    void OnTriggerEnter(Collider other)
    {

    }

    //离开碰撞;
    void OnTriggerExit(Collider other)
    {

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            return;
        }

        CancelInvoke("ColliderEnd");
        Invoke("ColliderEnd", 0.1f);
    }

    public void SetCharacterControllerEnable(bool isEnable)
    {
        if (_characterController == null)
            return;

        if (_characterController.enabled == isEnable)
            return;

        _characterController.enabled = isEnable;
    }

    public void SetColor(Color color, float val)
    {
        return;

        for (int i = 0, max = rendererList.Length; i < max; i++)
        {
            rendererList[i].material.SetColor("_RimColor", color);
            rendererList[i].material.SetFloat("_RimParam", val);
        }
    }

    public void SetAlpha(float val)
    {
        return;

        for (int i = 0, max = rendererList.Length; i < max; i++)
        {
            rendererList[i].material.SetFloat("_Alpha", val);
        }
    }

    public void ShowRenderer(bool isShow)
    {
        return;

        for (int i = 0, max = rendererList.Length; i < max; i++)
        {
            rendererList[i].enabled = isShow;
        }
    }

    public void SetPosition(Vector3 position)
    {
        transformCaChe.position = position;
    }

    public void SetAngle(float angle)
    {
        transformCaChe.eulerAngles = new Vector3(0, angle, 0);
    }

    public void JumpPosition(Vector3 toPosition)
    {
        _playerState = PlayerState.jump;

        Vector3 forward = (toPosition - transformCaChe.position).normalized;

        SetForward(forward);

        jumpParam = new JumpParam();
        jumpParam.forward = Vector3.forward;
        jumpParam.maxDistance = Vector3.Distance(toPosition, transformCaChe.position);
        jumpParam.curDistance = 0;
        jumpParam.speed = 8.0f;
        jumpParam.toPosition = toPosition;

        PlayAnimation("jump");
    }

    public void RollPosition(Vector3 toPosition)
    {
        _playerState = PlayerState.roll;

        Vector3 forward = (toPosition - transformCaChe.position).normalized;
        
        rollParam = new RollParam();
        rollParam.forward = toPosition - transformCaChe.position;
        rollParam.maxDistance = Vector3.Distance(toPosition, transformCaChe.position);
        rollParam.curDistance = 0;
        rollParam.speed = 8.0f;
        rollParam.toPosition = toPosition;

        PlayAnimation("roll");
    }

    //朝着指定方向移动
    public void MoveDirection(Vector3 direction)
    {
        if (_playerState == PlayerState.jump || _playerState == PlayerState.roll || _playerState == PlayerState.die )
        {
            return;
        }

        moveDirection = direction.normalized;

        if (direction.Equals(Vector3.zero) == true)
        {
            directionCache = Vector3.zero;
            _playerState = PlayerState.idle;

            PlayAnimation("IsRun", false);
        }
        else
        {
            PlayAnimation("IsRun", true);
            float angle = Angle(transformCaChe.forward, direction);
            RunAnimation(angle);
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

    //朝着指定点移动
    public void MoveToPoint(Vector3 point, Vector3 _directionCache = default(Vector3))
    {
        if (_playerState == PlayerState.jump || _playerState == PlayerState.roll || _playerState == PlayerState.die)
        {
            return;
        }

        _playerState = PlayerState.run;

        directionCache = _directionCache;
        isMovePoint = true;
        point.y = transformCaChe.position.y;
        moveToPoint = point;

    }

    public void StopMove()
    {
        _playerState = PlayerState.idle;
        directionCache = Vector3.zero;
        moveDirection = Vector3.zero;
    }

    public void SetForward(Vector3 direction)
    {
        direction.y = 0;
        if (direction.Equals(Vector3.zero) == false)
        {
            transformCaChe.forward = direction;
        }
    }

    public void PlayAnimation(string aniName)
    {
        if (mAnimator == null)
            return;

        mAnimator.Play(aniName);
    }

    public void PlayAnimation(string param, bool flag)
    {
        if (mAnimator == null)
            return;

        mAnimator.SetBool(param, flag);
    }

    public void PlayAnimation(string param, int val)
    {
        if (mAnimator == null)
            return;

        mAnimator.SetInteger(param, val);
    }

    public void PlayAnimation(string param, float val)
    {
        if (mAnimator == null)
            return;

        mAnimator.SetFloat(param, val);
    }

    public void SetAnimationSpeed(float speed)
    {
        mAnimator.speed = speed;
    }

    public void RunAnimation(float angle)
    {
        PlayAnimation("moveTree");
        PlayAnimation("MoveSpeed", angle);
    }

}
