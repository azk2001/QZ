using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoidWWWDelegate(WWW www);

public class LoadWWWParam
{
    public string path;
    public int loadCount;
    public WWW www;
    public bool loaded;
    public VoidWWWDelegate loadFinish;
}

public class WWWLoadManager
{
    private static WWWLoadManager instance = null;
    public static WWWLoadManager Instance
    {
        get
        {

            if (instance == null)
            {
                instance = new WWWLoadManager();
                instance.Init();
            }
            return instance;
        }
    }

    private static Dictionary<string, LoadWWWParam> loadResDic = new Dictionary<string, LoadWWWParam>(); //所有资源列表;
    private static int loadMaxCount = 5;
    private static List<WWWLoad> loaderList = new List<WWWLoad>();          //加载列表;
    private static List<WWWLoad> leisureList = new List<WWWLoad>();         //空闲列表;
    private static List<LoadWWWParam> loadQueue = new List<LoadWWWParam>(); //等待加载队列;

    public void Init()
    {
        GameObject go = new GameObject("WWWLoadManager");

        for (int i = 0, max = loadMaxCount; i < max; i++)
        {
            GameObject g = new GameObject("LoadWWW");
            WWWLoad lr = g.AddComponent<WWWLoad>();
            g.transform.parent = go.transform;
            leisureList.Add(lr);
        }

        GameObject.DontDestroyOnLoad(go);
    }

    public void LoadWWW(string path, VoidWWWDelegate loadFinish)
    {
        string name = System.IO.Path.GetFileName(path);

        LoadWWWParam param = null;
        if (loadResDic.ContainsKey(name) == true)
        {
            param = loadResDic[name];
            param.loadCount++;

            if (param.loaded == true)
            {
                loadFinish(param.www);
            }
            else
            {
                param.loadFinish += loadFinish;
            }

            return;
        }
        else
        {
            param = new LoadWWWParam();
            param.path = path;
            param.loadCount++;
            param.loadFinish = loadFinish;
            param.loaded = false;

            loadQueue.Add(param);

            loadResDic[name] = param;
        }

        StartLoadWWW();

    }

    private void StartLoadWWW()
    {
        if (loadQueue.Count > 0 && leisureList.Count > 0)
        {
            WWWLoad lr = leisureList[leisureList.Count - 1];
            lr.StartLoad(loadQueue[0]);
            loadQueue.RemoveAt(0);

            leisureList.Remove(lr);
            loaderList.Add(lr);
        }

    }

    public void UnloadResource(string name)
    {
        if (loadResDic.ContainsKey(name) == true)
        {
            LoadWWWParam resParam = loadResDic[name];
            resParam.loadCount--;
            if (resParam.loadCount <= 0)
            {
                //if (resParam.www != null)
                //{
                //    if (resParam.www.assetBundle != null)
                //        resParam.www.assetBundle.Unload(false);
                //}

                if (resParam.www != null)
                {
                    resParam.www.Dispose();
                }
                resParam.www = null;

                loadResDic.Remove(name);
            }
        }
    }

    public void UnloadWWW(string name)
    {
        if (loadResDic.ContainsKey(name) == true)
        {
            LoadWWWParam resParam = loadResDic[name];
            if (resParam.www != null)
            {
                resParam.www.Dispose();
            }
        }
    }

    public void LoadFinishEvent(WWWLoad loadWWW, LoadWWWParam param)
    {
        if (param.loadFinish != null)
        {
            param.loadFinish(param.www);
        }

        leisureList.Add(loadWWW);
        loaderList.Remove(loadWWW);

        StartLoadWWW();

    }

}
