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
    public int gameUintId = 0;

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
    private PlayerState playerState = PlayerState.idle;
    private Renderer[] rendererList = null;
    private JumpParam jumpParam;
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
        playerState = PlayerState.idle;

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
        
        ShowRenderer(true);
        CancelInvoke("DizzinessEnd");
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

        if (playerState == PlayerState.jump)
        {
            Vector3 dar = jumpParam.forward * jumpParam.speed * Time.deltaTime;

            transformCaChe.Translate(dar);

            jumpParam.curDistance += dar.magnitude;

            if (jumpParam.curDistance > jumpParam.maxDistance)
            {
                transformCaChe.position = jumpParam.toPosition;

                playerState = PlayerState.idle;

                MoveDirection(moveDirection);
            }
        }

        if (playerState != PlayerState.jump && playerState != PlayerState.roll) 
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
        playerState = PlayerState.jump;

        Vector3 forward = (toPosition - transformCaChe.position).normalized;

        SetForward(forward);

        jumpParam = new JumpParam();
        jumpParam.forward = Vector3.forward;
        jumpParam.maxDistance = Vector3.Distance(toPosition, transformCaChe.position);
        jumpParam.curDistance = 0;
        jumpParam.speed = 8.0f;
        jumpParam.toPosition = toPosition;
        
    }

    public void RollPosition(Vector3 toPosition)
    {
        playerState = PlayerState.roll;


    }

    //朝着指定方向移动
    public void MoveDirection(Vector3 direction)
    {
        if (playerState == PlayerState.jump || playerState == PlayerState.roll)
        {
            return;
        }

        moveDirection = direction.normalized;

        //SetForward(moveDirection);

        if (direction.Equals(Vector3.zero))
        {
            directionCache = Vector3.zero;
            playerState = PlayerState.idle;
        }
    }

    //朝着指定点移动
    public void MoveToPoint(Vector3 point, Vector3 _directionCache = default(Vector3))
    {
        if (playerState == PlayerState.jump || playerState == PlayerState.roll)
        {
            return;
        }

        playerState = PlayerState.run;

        directionCache = _directionCache;
        isMovePoint = true;
        point.y = transformCaChe.position.y;
        moveToPoint = point;

    }

    public void StopMove()
    {
        playerState = PlayerState.idle;
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
