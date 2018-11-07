using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleLoad : MonoBehaviour
{

    private LoadResParam resParam = null;
    private AssetBundleCreateRequest bundleLoadRequest = null;
    private AssetBundleRequest bundleRequest = null;

    private int reloadNum = 5;

    void Start()
    {

    }

    public void StartLoad(LoadResParam param)
    {
        resParam = param;

        reloadNum = 5;

        bundleLoadRequest = null;
        bundleRequest = null;

        StartCoroutine(LoadResource());
    }

    private IEnumerator LoadResource()
    {
        bundleLoadRequest = AssetBundle.LoadFromFileAsync(resParam.path);
        yield return bundleLoadRequest;

        if (bundleLoadRequest.isDone == true)
        {
            if (bundleLoadRequest.assetBundle == null)
            {

                reloadNum--;

                if (reloadNum <= 0)
                {
                    EndLoad();
                }
                else
                {
                    StartCoroutine(LoadResource());
                }
            }
            else
            {
                StartCoroutine(LoadAllAssetsAsync());
            }

        }
    }

    private IEnumerator LoadAllAssetsAsync()
    {
        bundleRequest = bundleLoadRequest.assetBundle.LoadAllAssetsAsync();

        yield return bundleRequest;

        if (bundleRequest.isDone == true)
        {
            EndLoad();
        }
    }

    public void EndLoad()
    {
        resParam.ab = null;

        if (bundleLoadRequest != null)
        {
            resParam.ab = bundleLoadRequest.assetBundle;
        }

        resParam.bundleRequest = bundleRequest;

        resParam.loaded = true;

        AssetBundleLoadManager.Instance.LoadFinishEvent(this, resParam);
    }
}
