using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EditorManager : MonoBehaviour
{

    public GameObject elementPoints;

    private static EditorManager _instance = null;
    static public EditorManager GetInstance()
    {
        if (_instance == null)
        {
            GameObject mapManager = GameObject.Find("MapManager");

            if (mapManager == null)
            {

                mapManager = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("MapManager"));
                mapManager.name = "MapManager";

                _instance = mapManager.GetComponent<EditorManager>();
                if (_instance == null)
                {
                    _instance = mapManager.AddComponent<EditorManager>();
                }

            //    Debug.LogError("没有找到MapManager这个物体,请参考___res_pool/_ScenesPool/ScenesTemp_Arts/level_template 场景");
            }
            else
            {
                _instance = mapManager.GetComponent<EditorManager>();
            }
        }
        return _instance;
    }

    void Awake()
    {
        _instance = this;
    }

}
