using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public delegate void VoidAssetBundleDelegate(AssetBundleRequest bundleRequest, object[] pars);

public class LoadResParam
{
    public string name;
    public string path;
    public int loadCount;
    public AssetBundle ab;
    public AssetBundleRequest bundleRequest;
    public bool loaded;
    public eLoadPriority loadPriority;  //加载优先级;
    public object[] pars;
    public VoidAssetBundleDelegate loadFinish;
}

public enum eLoadPriority
{
    none,       //无意义

    common,     //一般优先级
    ordinary,   //普通优先级
    advanced,   //最高优先级
    relation,   //(只能用于关联资源加载使用)

    max,        //无意义
}

public class AssetBundleLoadManager
{
    private static AssetBundleLoadManager instance = null;
    public static AssetBundleLoadManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AssetBundleLoadManager();
            }
            return instance;
        }
    }


    private Dictionary<string, LoadResParam> loadResDic = new Dictionary<string, LoadResParam>(); //所有加载过的资源列表;

    private AssetBundleManifest manifest = null;    //主文件;
    private static int loadMaxCount = 5;            //开启几个加载线程
    private static List<AssetBundleLoad> loaderList = new List<AssetBundleLoad>();  //加载列表;
    private static List<AssetBundleLoad> leisureList = new List<AssetBundleLoad>(); //空闲列表;
    private static Dictionary<string, LoadResParam> loadingList = new Dictionary<string, LoadResParam>();       //加载中的资源列表,有可能一个资源同时被多次加载

    private static Dictionary<eLoadPriority, List<LoadResParam>> loadQueue = new Dictionary<eLoadPriority, List<LoadResParam>>(); //等待加载队列;

    private VoidBoolDelegate InitFinish = null;

    public void Init(VoidBoolDelegate InitFinish)
    {
        this.InitFinish = InitFinish;

        GameObject go = new GameObject("ResourcesLoadManager");

        for (int i = 0, max = loadMaxCount; i < max; i++)
        {
            GameObject g = new GameObject("LoadAssetBundle");
            AssetBundleLoad lr = g.AddComponent<AssetBundleLoad>();
            g.transform.parent = go.transform;
            leisureList.Add(lr);
        }

        string name = "StreamingAssets";

        string path = ResLoadPath(name);// custom_app_data.LoadStreamAsstesPath + name;

        LoadResParam resParam = new LoadResParam();
        resParam.loadPriority = eLoadPriority.advanced;
        resParam.name = name;
        resParam.loadCount++;
        resParam.path = path;
        resParam.loadFinish = LoadManifestFinish;
        resParam.pars = null;

        if (loadQueue.ContainsKey(eLoadPriority.advanced) == false)
        {
            loadQueue[eLoadPriority.advanced] = new List<LoadResParam>();
        }
        loadQueue[eLoadPriority.advanced].Add(resParam);

        LoadAssetBundle();

        GameObject.DontDestroyOnLoad(go);
    }

    private void LoadManifestFinish(AssetBundleRequest bundleRequest, object[] pars)
    {
        manifest = (AssetBundleManifest)bundleRequest.asset;

        if (InitFinish != null)
        {
            InitFinish(true);
        }
    }


    public string ResLoadPath(string name)
    {
        string urlPath = name;

        return urlPath;
    }

    public void LoadObject(string name, eLoadPriority advanced, object[] pars, Action<UnityEngine.Object, object[]> loadFinish)
    {
        UnityEngine.Object ob = Resources.Load<UnityEngine.Object>(name);
        loadFinish(ob, pars);
    }


    /// <summary>
    /// 所有需要加载的资源都需要放在StreamingAssets文件夹下加载
    /// </summary>
    /// <param name="name">加载的资源名字</param>
    /// <param name="loadPriority">加载优先级</param>
    /// <param name="pars">加载返回的参数</param>
    /// <param name="loadFinish">加载完成回调</param>
    public void LoadAssetBundle(string name, eLoadPriority loadPriority, object[] pars, VoidAssetBundleDelegate loadFinish)
    {
        if (name.Contains(".unity3d") == false)
        {
            name = name + ".unity3d";  //添加后缀名
        }

        //获取依赖文件列表;
        string[] dependList = manifest.GetDirectDependencies(name);
        for (int i = 0, max = dependList.Length; i < max; i++)
        {
            string tName = dependList[i];
            if (loadResDic.ContainsKey(tName) == true)
            {
                LoadResParam dependRes = loadResDic[tName];
                dependRes.loadCount++;
            }
            else
            {
                string dependPath = ResLoadPath(dependList[i]);// custom_app_data.LoadStreamAsstesPath + dependList[i];

                eLoadPriority priority = eLoadPriority.relation;

                LoadResParam param = new LoadResParam();
                param.loadPriority = priority;
                param.path = dependPath;
                param.loaded = false;
                param.name = tName;

                //当这个资源第一次被加载的时候加入到加载队列中
                if (loadQueue.ContainsKey(priority) == false)
                {
                    loadQueue[priority] = new List<LoadResParam>();
                }
                loadQueue[priority].Add(param);
            }
        }

        //加载我们需要的文件;
        LoadResParam resParam = null;
        if (loadResDic.ContainsKey(name) == true)
        {
            resParam = loadResDic[name];
            resParam.loadCount++;
        }

        if (resParam != null && resParam.loaded == true)
        {
            loadFinish(resParam.bundleRequest, pars);
        }
        else
        {
            string resPath = ResLoadPath(name); // custom_app_data.LoadStreamAsstesPath + name;

            resParam = new LoadResParam();
            resParam.loadPriority = loadPriority;
            resParam.path = resPath;
            resParam.pars = pars;
            resParam.loadFinish = loadFinish;
            resParam.name = name;

            //当这个资源第一次被加载的时候加入到加载队列中
            if (loadQueue.ContainsKey(loadPriority) == false)
            {
                loadQueue[loadPriority] = new List<LoadResParam>();
            }
            loadQueue[loadPriority].Add(resParam);
        }

        LoadAssetBundle();
    }

    private void LoadAssetBundle()
    {
        List<LoadResParam> paramList;

        //必须把公用资源包加载完成 才加载其他资源
        if (loadQueue.ContainsKey(eLoadPriority.relation) == true && loadQueue[eLoadPriority.relation].Count > 0)
        {
            paramList = loadQueue[eLoadPriority.relation];

            for (int i = 0, max = paramList.Count; i < max; i++)
            {
                LoadResParam resParam = paramList[i];

                //如果正在加载的队列中已经有这个资源，就跳过
                if (loadingList.ContainsKey(resParam.name) == true)
                    continue;

                if (leisureList.Count > 0)
                {
                    loadingList[resParam.name] = resParam;  //添加正在加载的资源

                    AssetBundleLoad lab = leisureList[leisureList.Count - 1];
                    lab.StartLoad(resParam);

                    leisureList.Remove(lab);
                    loaderList.Add(lab);
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            for (int i = (int)eLoadPriority.max - 1; i > (int)eLoadPriority.none; i--)
            {
                eLoadPriority loadPriority = (eLoadPriority)i;

                if (loadQueue.ContainsKey(loadPriority) == true && loadQueue[loadPriority].Count > 0)
                {
                    paramList = loadQueue[loadPriority];

                    for (int x = 0, maxx = paramList.Count; x < maxx; x++)
                    {
                        LoadResParam resParam = paramList[x];

                        //如果正在加载的队列中已经有这个资源，就跳过
                        if (loadingList.ContainsKey(resParam.name) == true)
                            continue;

                        if (leisureList.Count > 0)
                        {
                            loadingList[resParam.name] = resParam;  //添加正在加载的资源

                            AssetBundleLoad lab = leisureList[leisureList.Count - 1];
                            lab.StartLoad(resParam);

                            leisureList.Remove(lab);
                            loaderList.Add(lab);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 摧毁资源
    /// </summary>
    /// <param name="name">资源名字</param>
    /// <param name="unloadAll">是否摧毁场景上的所有资源</param>
    public void UnloadAssetBundle(string name, bool unloadAll = true)
    {
        if (name.Contains(".unity3d") == false)
        {
            name = name + ".unity3d";  //添加后缀名
        }

        //先减去公用资源的引用;
        string[] dependList = manifest.GetDirectDependencies(name);
        for (int i = 0, max = dependList.Length; i < max; i++)
        {
            string tName = dependList[i];
            if (loadResDic.ContainsKey(tName) == true)
            {
                LoadResParam dependRes = loadResDic[tName];
                dependRes.loadCount--;
                if (dependRes.loadCount <= 0)
                {
                    dependRes.ab.Unload(unloadAll);
                    loadResDic.Remove(tName);
                }
            }
        }

        //减去自己的引用;
        if (loadResDic.ContainsKey(name) == true)
        {
            LoadResParam resParam = loadResDic[name];
            resParam.loadCount--;
            if (resParam.loadCount <= 0)
            {
                resParam.ab.Unload(unloadAll);
                resParam.ab = null;
                loadResDic.Remove(name);
            }
        }
    }

    public void LoadFinishEvent(AssetBundleLoad loadAssetBundle, LoadResParam resParam)
    {
        string name = resParam.name;

        //通知所有的加载中的队列，资源加载完成
        List<LoadResParam> loadedReslist = loadQueue[resParam.loadPriority];

        for (int i = loadedReslist.Count - 1; i >= 0; i--)
        {
            LoadResParam loadedRes = loadedReslist[i];

            if (loadedRes.name == name)
            {
                if (loadedRes.loadFinish != null)
                {
                    loadedRes.loadFinish(resParam.bundleRequest, loadedRes.pars);
                }
                resParam.loadCount++;

                loadedReslist.Remove(loadedRes);
            }
        }

        //删除正在加载中这个加载的资源;
        loadingList.Remove(name);

        leisureList.Add(loadAssetBundle);
        loaderList.Remove(loadAssetBundle);


        loadResDic[name] = resParam;

        //重新启动下一个加载
        LoadAssetBundle();
    }
}
