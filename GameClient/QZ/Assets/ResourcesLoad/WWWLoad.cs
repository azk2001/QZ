using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWWLoad : MonoBehaviour
{
    private LoadWWWParam loadWWWParam;
    private WWW www;

    private int reloadNum = 1;

    void Start()
    {

    }

    public void StartLoad(LoadWWWParam wwwParam)
    {
        reloadNum = 5;

        loadWWWParam = wwwParam;
        
        StartCoroutine(LoadResource());
    }

    private IEnumerator LoadResource()
    {
        www = new WWW(loadWWWParam.path);

        yield return www;
        
        if(www.error == null)
        {
            EndLoad();
        }
        else
        {
            reloadNum--;

            if(reloadNum<=0)
            {
                EndLoad();
            }
        }
    }

    public void EndLoad()
    {
        loadWWWParam.www = www;

        bool loaded = www.error == null;

        loadWWWParam.loaded = loaded;

        WWWLoadManager.Instance.LoadFinishEvent(this, loadWWWParam);
    }
}
