using UnityEngine;
using System.Collections;

//玩家控制脚本;
public class PlayerController
{
    private static PlayerController _instance = null;
    public static PlayerController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerController();
            }
            return _instance;
        }
    }

    public GameUnit mGameUnit = null;
    public float aniTransSpeed = 5.0f;
    private Vector3 moveDir = Vector3.zero;

	public bool isInput = false;

    public void OnAddPlayerController(GameUnit gameUnit)
    {
		isInput = true;
        mGameUnit = gameUnit;
    }

	public void OnRemovePlayerController()
	{
		if (mGameUnit == null)
			return;
		
		mGameUnit.PlayRunAnimation (Vector3.zero);
		mGameUnit = null;

	}

	private void DizzinessStart(int gameUintId)
	{
		if (gameUintId == mGameUnit.mGameUintId) 
		{
			isInput=true;
		}
	}

	private void DizzinessEnd(int gameUintId)
	{
		
	}

	private void PlayerStartDead(int gameUintId)
	{
		if (gameUintId == mGameUnit.mGameUintId) 
		{
			isInput=false;
		}
	}

	private void PlayerStopDead(int gameUintId)
	{
		if (gameUintId == mGameUnit.mGameUintId) 
		{
			isInput=true;
		}
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

		//客服端直接移动;
        mGameUnit.PlayRunAnimation (moveDir);
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
