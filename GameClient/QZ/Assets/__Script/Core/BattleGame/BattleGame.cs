using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BattleGame : MonoBehaviour {

	public static BattleGame instance = null;

	public GameUnit localGmeUnit = null;		//本地角色控制单位;

	public Dictionary<int,List<GameUnit>> battleUnitList = new Dictionary<int, List<GameUnit>> ();

	public cs_dungeon curDungeonParam = null;
	void Awake()
	{
		instance = this;
	}

	void Start () 
	{

	}

	public void Init(int dungeonId)
	{		
		battleUnitList.Clear ();

		// 随机播放战斗音乐
		int index = Random.Range(0, AudioPlayIDCollect.ad_battleMusic.Length);
		int id = AudioPlayIDCollect.ad_battleMusic[index];
		AudioManager.instance.Play(id, Vector3.zero, true);
        
	}

	//本地玩家创建完成;
	public void OnCreatePlayerFinish()
	{
		
	}

	public void AddBattleUnit (int campId,GameUnit gameUnit)
	{
		if (battleUnitList.ContainsKey (campId) == false) {
			battleUnitList[campId]=new List<GameUnit>();
		}
		battleUnitList [campId].Add (gameUnit);
	}


	void Update () {
		BuffManager.instance.Update ();
		TimeManager.Instance.Update ();
	}

	//战斗结束;
	public void BattleEnd()
	{
		BuffManager.instance.RemoveAll ();
		GameUnitManager.instance.RemoveAllGameUnit ();
		PlayerController.instance.OnRemovePlayerController();
		
		BattleBuffRoot.instance.DeSpwanAll ();
		BattleEffectRoot.instance.DeSpwanAll ();
		BattleUnitRoot.instance.DeSpwanAll ();
		UIBattleRoot.instance.DeSpwanAll ();
	}

	private void OnEndGame()
	{
		

	}

	public void ReturnScence()
	{
		ClearScence();
		
	}

	private void OnScenceLoadFinish(int dungeon)
	{
	
	}

	//场景结束;
	public void ClearScence()
	{
		
	}
}
