using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager 
{
	private static BuffManager _instance = null;
	public static BuffManager instance {
		get {
			if(_instance == null)
			{
				_instance= new BuffManager();
			}
			return _instance;
		}
	}

	public Dictionary<int,System.Type> buffType = new Dictionary<int, System.Type>()
	{
		{1,typeof(BuffSpeedUp)},		//加速度;
		{2,typeof(BuffStrengthUp)},		//加炸弹威力;
		{3,typeof(BuffBombUp)},			//加炸弹数量;

	};


	private static int _buffStaticId = int.MaxValue;

	private static int buffStaticId {
		get {
			_buffStaticId--;
			return _buffStaticId;
		}
	}

	private Dictionary<int,BuffBase> buffTypeDic= new Dictionary<int, BuffBase>();

	private List<BuffBase> buffTypeList = new List<BuffBase> ();
	private Dictionary<Vector2,BuffBase> markPositionBuffType= new Dictionary<Vector2, BuffBase>();

	public BuffBase CreateLocalBuff(int buffId,Vector3 position)
	{
		int staticId = buffStaticId;
		cs_buff buffParam = cs_buff.GetThis(buffId);

		BuffBase bb = (BuffBase)( System.Activator.CreateInstance( buffType [buffId]) );
		if( bb != null )
		{
			bb.InitConfig(staticId,buffParam,position);
		}

		buffTypeDic [staticId] = bb;
		buffTypeList.Add (bb);
        
		return bb;
	}

	public BuffBase CreateServerBuff(int staticId,int buffId,Vector3 position)
	{
		cs_buff buffParam = cs_buff.GetThis(buffId);
		
		BuffBase bb = (BuffBase)( System.Activator.CreateInstance( buffType [buffId]) );
		if( bb != null )
		{
			bb.InitConfig(staticId,buffParam,position);
		}
		
		buffTypeDic [staticId] = bb;
		buffTypeList.Add (bb);

		return bb;
	}

	public BuffBase GetBuff(int buffStaticId)
	{
		if(buffTypeDic.ContainsKey(buffStaticId)==true)
		{
			return buffTypeDic[buffStaticId];
		}
		return null;
	}
	public BuffBase GetBuff(Vector2 markPosition)
	{
		if(markPositionBuffType.ContainsKey(markPosition)==true)
		{
			return markPositionBuffType[markPosition];
		}
		return null;
	}

	public bool RemoveBuff(int buffStaticId)
	{
		if(buffTypeDic.ContainsKey(buffStaticId)==true)
		{
			BuffBase bb = buffTypeDic[buffStaticId];
			if(buffTypeList.Contains(bb))
			{
				buffTypeList.Remove(bb);
			}

			markPositionBuffType.Remove(bb.markPosition);
			buffTypeDic.Remove(buffStaticId);
		}
		
		return false;
	}

	public void RemoveAll()
	{
		List<BuffBase> t = new List<BuffBase> (buffTypeDic.Values);

		foreach (var v in t) {
			v.RemoveBuff();
		} 
		t = null;

		markPositionBuffType.Clear ();
		buffTypeDic.Clear ();
		buffTypeList.Clear ();
	}

	public void Update()
	{
		for (int i=buffTypeList.Count-1; i>=0; i--) 
		{
			buffTypeList[i].Update(Time.deltaTime);
		}
	}
}
