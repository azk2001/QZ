using UnityEngine;
using System.Collections;

public class BattleUnitRoot : SingleClass<BattleUnitRoot>
{
    private string spwanPoolName = "BattleUnitRoot";



    private SpwanPool _spwanPool = null;
    public SpwanPool spwanPool
    {
        get
        {
            if (_spwanPool == null)
            {
                _spwanPool = PoolManager.instance.GetSpwanPool(spwanPoolName);
            }
            return _spwanPool;
        }
    }

    public Transform SpwanPrefab(string prefabName)
    {
        Transform trans = spwanPool.SpwanPrefab(prefabName);

        return trans;
    }

    public Transform SpwanPrefab(string prefabName, float deSpwanTime)
    {
        Transform trans = spwanPool.SpwanPrefab(prefabName, deSpwanTime);

        return trans;
    }

    public void DeSpwan(Transform trans)
    {
        spwanPool.DeSpwanPrefab(trans);
    }

    public void DeSpwanAll()
    {
        spwanPool.DeSpwanAllPrefab();
    }
}
