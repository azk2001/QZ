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

    public float moveSpeed = 0.2f;
    public int gameUintId = 0;

	public Transform teamShadow = null;
	public Transform transformCaChe = null;
	
	public bool isMove = true;
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
    private Vector3 moveDirection = Vector3.down;
    private Vector3 directionCache = Vector3.zero;
    private Vector3 moveToDir = Vector3.zero;
    private Vector3 moveToPosition = Vector3.zero;
    private bool isMovePosition = false;
    public bool isJump = false;
    private float runValue = 0;
    private Collider colliderTiem = null;
    private Renderer[] rendererList = null;
	private JumpParam jumpParam;
	private Transform curShadow =null;

    void Awake()
    {
        Init();
    }

    void Start()
    {

    }

    public void Init()
    {
		_characterController = this.GetComponent<CharacterController>();
        transformCaChe = this.transform;
        mAnimator = this.GetComponentInChildren<Animator>();
        rendererList = transformCaChe.Find("Model").GetComponentsInChildren<Renderer>();
		isMove = true;
		SetCharacterControllerEnable (true);
		mAnimator.speed = 1;
		isMovePosition = false;

		directionCache = Vector3.zero;
		moveDirection = Vector3.zero;
		moveToPosition = Vector3.zero;

		teamShadow.gameObject.SetActive (false);
		ShowRenderer (true);
		CancelInvoke ("DizzinessEnd");
    }

    void Update()
    {

        if (_characterController == null)
            return;

        if (_characterController.isGrounded == false)
            moveDirection.y = -50;
        else
            moveDirection.y = 0;

        if (isMovePosition == true)
        {
            if (Vector3.Distance(transformCaChe.position, moveToPosition) > 0.2f)
            {
                moveToDir = moveToPosition - transformCaChe.position;
                MoveDirection(moveToDir * 8);
            }
            else
            {
                isMovePosition = false;
                MoveDirection(directionCache);
            }
        }

		if (isJump == true)
		{
			Vector3 dar = jumpParam.forward * jumpParam.speed*Time.deltaTime;
			
			transformCaChe.Translate (dar);
			
			jumpParam.curDistance+=dar.magnitude;
			
			if (jumpParam.curDistance>jumpParam.maxDistance)
			{
				transformCaChe.position = jumpParam.toPosition;
				
				isMove = true;
				isJump = false;

				MoveDirection(moveDirection);
			}
		}

		if (isMove ==true && _characterController.enabled == true) {
			_characterController.Move (moveDirection * moveSpeed * Time.deltaTime);
		}
    }

	public void SetTeamShadow(bool isFriend)
	{
		teamShadow.gameObject.SetActive (true);
		if (isFriend == true) {
			curShadow = teamShadow.Find ("shadow_friend");
		} else {
			curShadow = teamShadow.Find ("shadow_enemy");
		}
		curShadow.gameObject.SetActive (true);

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

    void ColliderEnd()
    {
       
        colliderTiem = null;
    }

    public void SetCharacterControllerEnable(bool isEnable)
    {
        if (_characterController == null)
            return;

        if (_characterController.enabled == isEnable)
            return;

        _characterController.enabled = isEnable;
    }

    public void SetColor(Color color,float val)
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

		for (int i=0,max = rendererList.Length; i<max; i++) {
			rendererList[i].enabled= isShow;
		}
	}

    public void SetPosition(Vector3 position)
    {
        transformCaChe.position = position;
    }

	public void SetAngle(float angle)
	{
		transformCaChe.eulerAngles = new Vector3 (0, angle, 0);
	}

	public void JumpPosition(Vector3 toPosition)
    {
        isJump = true;

		Vector3 forward = (toPosition - transformCaChe.position).normalized;

		SetForward(forward);

		jumpParam = new JumpParam ();
		jumpParam.forward = Vector3.forward;
		jumpParam.maxDistance = Vector3.Distance (toPosition,transformCaChe.position);
		jumpParam.curDistance = 0;
		jumpParam.speed = 8.0f;
		jumpParam.toPosition = toPosition;

		isMove = false;
    }

    public void MoveDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;

		if (isJump == true)
			return;

        SetForward(moveDirection);

        if (direction.Equals(Vector3.zero))
        {
            directionCache = Vector3.zero;
        }
    }

    public void MoveToPosition(Vector3 position, Vector3 _directionCache = default(Vector3))
    {
        directionCache = _directionCache;
        isMovePosition = true;
        position.y = transformCaChe.position.y;
        moveToPosition = position;

    }

	//不能控制;
	public void DizzinessTime(float dizzinessTime)
	{
		isJump =false;
		isMove =false;

		CancelInvoke ("DizzinessEnd");
		Invoke("DizzinessEnd",dizzinessTime);
	}

	private void DizzinessEnd()
	{
		isMove = true;
	}

    public void StopMove()
    {
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

	public void PlayAnimation(string aniName,eHorseCarType horseCarType)
	{
		if (mAnimator == null)
			return;
		int layerIndex = 0;

		switch (horseCarType) {
		case eHorseCarType.none:
			layerIndex = mAnimator.GetLayerIndex ("BaseLayer");
			break;
		case eHorseCarType.horseCar:
			layerIndex = mAnimator.GetLayerIndex ("HoresCarLayer");
			break;
		case eHorseCarType.max:
			break;
		}

		mAnimator.Play(aniName,layerIndex);
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

    public void RunAnimation(Vector3 v3)
    {
        runValue = v3.magnitude;
        PlayAnimation("MoveSpeed", runValue);
    }

}
