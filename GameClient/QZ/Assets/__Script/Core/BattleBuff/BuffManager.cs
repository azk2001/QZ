using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : SingleClass<BuffManager>
{

    public Dictionary<int, System.Type> buffType = new Dictionary<int, System.Type>()
    {
        {1,typeof(BuffChangeSpeed)},		//速度变化;

	};


    private static int _uuid = int.MaxValue;
    private static int uuid
    {
        get
        {
            _uuid--;
            return _uuid;
        }
    }

    private Dictionary<int, BuffBase> buffTypeDic = new Dictionary<int, BuffBase>();
    private List<BuffBase> buffTypeList = new List<BuffBase>();

    public BuffBase AddLocalBuff(int buffId, Vector3 position)
    {
        int uuid = BuffManager.uuid;
        cs_buff buffParam = cs_buff.GetThis(buffId);

        BuffBase bb = (BuffBase)(System.Activator.CreateInstance(buffType[buffId]));
        if (bb != null)
        {
            bb.CreateBuff(uuid, buffParam, position);
        }

        buffTypeDic[uuid] = bb;
        buffTypeList.Add(bb);

        return bb;
    }

    public BuffBase AddServerBuff(int uuid, int buffId, Vector3 position)
    {
        cs_buff buffParam = cs_buff.GetThis(buffId);

        BuffBase bb = (BuffBase)(System.Activator.CreateInstance(buffType[buffId]));
        if (bb != null)
        {
            bb.CreateBuff(uuid, buffParam, position);
        }

        buffTypeDic[uuid] = bb;
        buffTypeList.Add(bb);

        return bb;
    }

    public BuffBase GetBuff(int uuid)
    {
        if (buffTypeDic.ContainsKey(uuid) == true)
        {
            return buffTypeDic[uuid];
        }
        return null;
    }

    public bool RemoveBuff(BuffBase bb)
    {
        return RemoveBuff(bb.uuid);
    }

    public bool RemoveBuff(int uuid)
    {
        if (buffTypeDic.ContainsKey(uuid) == true)
        {
            BuffBase bb = buffTypeDic[uuid];
            if (buffTypeList.Contains(bb))
            {
                buffTypeList.Remove(bb);
            }

            buffTypeDic.Remove(uuid);
        }

        return false;
    }

    public void RemoveAll()
    {
        List<BuffBase> t = new List<BuffBase>(buffTypeDic.Values);

        foreach (var v in t)
        {
            v.RemoveBuff();
        }
        t = null;

        buffTypeDic.Clear();
        buffTypeList.Clear();
    }

    public void Update()
    {
        for (int i = buffTypeList.Count - 1; i >= 0; i--)
        {
            buffTypeList[i].Update(Time.deltaTime);
        }
    }
}
