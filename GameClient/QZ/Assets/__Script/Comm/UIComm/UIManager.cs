using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    /// <summary>
    /// UI系统名，同预设名
    /// </summary>
    public enum eUIName
    {
        UnKnow = -1,
        UILogin,
        UISelectServer,
    }

    /// <summary>
    /// UI管理器
    /// </summary>
    public class UIManager : SingleClass<UIManager>
    {

        private Dictionary<eUIName, UIBase> allUIList = new Dictionary<eUIName, UIBase>();  //当前缓存的UI;
        private List<eUIName> curOpenUIList = new List<eUIName>();    //当前打开的UI;

        private List<eUIName> cacheUIList = new List<eUIName>();      //缓存UI列表;
        private int cacheUICount = 5;                              //缓存UI个数;

        private List<eUIName> openOrderList = new List<eUIName>();    //打开顺序列表;

        private eUIName curOpenUIName = eUIName.UnKnow;  //当前打开UI名字;

        private Transform uiRoot = null;

        private eUIName abNameLoading = eUIName.UnKnow;   //正在加载中的UI资源名字;

        public UIManager()
        {
            if (uiRoot == null)
            {
                uiRoot = GameObject.Find("UIRoot").transform;
                uiRoot.Reset();
            }

            allUIList.Add(eUIName.UILogin, new UILogin());
        }

        private Transform _uiParent = null;
        public Transform uiParent
        {
            get
            {
                if (_uiParent == null)
                    _uiParent = uiRoot.Find("Canvas");
                return _uiParent;
            }
        }

        /// <summary>
        /// 根据名字获取对应的UIBase;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uiName">UI 名字</param>
        /// <returns></returns>
        public T GetUIBase<T>(eUIName uiName) where T : UIBase
        {
            T uiBase = null;

            if (allUIList.ContainsKey(uiName))
            {
                uiBase = allUIList[uiName] as T;
            }

            return uiBase;
        }

        /// <summary>
        /// 打开UI
        /// </summary>
        /// <param name="uiName">UI名字</param>
        public void OpenUI(eUIName uiName)
        {
            Debug.Log(uiName);

            //是否已经打开;
            if (IsOpen(uiName) == true)
                return;

            UIBase uiBase = allUIList[uiName];

            //如果当前已经在缓存加载不是常显UI,当前需要打开的UI也不是常显UI就直接返回了
            if (abNameLoading != eUIName.UnKnow && uiBase.showing == false)
            {
                return;
            }

            //如果当前打开过UI就把当前打开的UI关闭，重新打开新的UI;
            if (uiBase.showing == false && curOpenUIName != eUIName.UnKnow)
            {
                CloseUI(curOpenUIName, false);
            }

            //如果有缓存，直接打开;
            if (cacheUIList.Contains(uiName) == true)
            {
                uiBase.OnInit();
                uiBase.isHide = false;

                if(curOpenUIList.Contains(uiName)==false)
                    curOpenUIList.Add(uiName);

                EventListenerManager.Invoke(EventEnum.openUI, uiName);
            }
            else
            {
                if (uiBase.showing == false)
                { 
                    abNameLoading = uiName;
                }

                string abName = uiName.ToString().ToLower();
                
            }

            //只添加不是长显UI;
            if (uiBase.showing == false)
            {
                //如果之前有这个UI排列顺序直接删除重新添加;
                if (openOrderList.Contains(uiName) == true)
                    openOrderList.Remove(uiName);

                openOrderList.Add(uiName);

                curOpenUIName = uiName;
            }

        }

        private void OnLoadUIFinish(AssetBundleRequest bundleRequest, object[] pars)
        {

            eUIName uiName = (eUIName)pars[0];

            UIBase uiBase = allUIList[uiName];

            if (curOpenUIList.Contains(uiName) == false)
                curOpenUIList.Add(uiName);

            GameObject go = GameObject.Instantiate(bundleRequest.GetAssets(uiName.ToString())) as GameObject;
            go.transform.SetParent(uiParent);
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;

            //设置UI的锚点显示方式;
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition3D = Vector3.zero;

            go.name = uiName.ToString();

            uiBase.OnAwake(go);

            //初始化UI系统的深度;
            uiBase.SetDepth((int)uiBase.uiDepth * 100);

            uiBase.OnInit();
            uiBase.OnEnable();

            EventListenerManager.Invoke(EventEnum.openUI, uiName);

            if (uiBase.showing == false)
            {
                abNameLoading = eUIName.UnKnow;
            }
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="uiName">关闭UI</param>
        /// <param name="uiName">是否要打开上一个UI</param>
        /// <param name="clearOrderList">是否要清空缓存</param>
        public void CloseUI(eUIName uiName, bool openPreUI = true, bool clearOrderList = false)
        {
            //是否已经打开;
            if (IsOpen(uiName) == false)
                return;

            //if (uiName == eUIName.UIHome)
            //    HomeData.Instance.curOpenType = UIHome.eObjectIndex.None;

            UIBase uiBase = allUIList[uiName];
            uiBase.isHide = true;

            curOpenUIList.Remove(uiName);


            //常显UI不加入UI队列的缓存列表;
            if (cacheUIList.Contains(uiName) == false && uiBase.showing == false)
            {
                cacheUIList.Add(uiName);
            }

            //清除缓存UI的个数;
            if (cacheUIList.Count > cacheUICount)
            {

                eUIName cacheUIName = cacheUIList[0];

                UnloadUIResouse(cacheUIName);

                cacheUIList.Remove(cacheUIName);
            }

            //如果当前关闭的UI是常显UI;就直接清理资源;
            if (uiBase.showing == true)
            {
                UnloadUIResouse(uiName);
            }

            //是否启动开启上一个UI;
            if (openPreUI == true)
            {
                //如果有打开上一个界面;
                for (int i = openOrderList.Count - 1; i >= 0; i--)
                {
                    eUIName preUIName = openOrderList[i];

                    //如果上一个打开的UI==当前关闭的UI就值接跳过,进行下一次寻找;
                    if (preUIName.Equals(uiName) == true)
                    {
                        //把之前打开的UI删掉;
                        openOrderList.Remove(preUIName);
                        continue;
                    }

                    UIBase perUIBase = allUIList[preUIName];

                    //如果上一个打开的UI的深度<=当前UI的深度;
                    if (perUIBase.uiDepth <= uiBase.uiDepth)
                    {
                        //重新打开UI;
                        OpenUI(preUIName);

                        //把之前打开的UI删掉;
                        //openOrderList.Remove(preUIName);

                        break;
                    }

                    //把之前打开的UI删掉;
                    openOrderList.Remove(preUIName);
                }
            }

            if (clearOrderList == true)
            {
                openOrderList.Clear();

                curOpenUIName = eUIName.UnKnow;
            }

            EventListenerManager.Invoke(EventEnum.closeUI, uiName);
        }


        /// <summary>
        /// 是否打开;
        /// </summary>
        /// <param name="uiName">UI名字</param>
        /// <returns></returns>
        public bool IsOpen(eUIName uiName)
        {
            if (curOpenUIList.Contains(uiName) == false)
                return false;

            return true;
        }


        /// <summary>
        /// 关闭所有UI，包括常显UI;
        /// </summary>
        public void CloseAll()
        { 
            for (int i = curOpenUIList.Count - 1; i >= 0; i--)
            {
                CloseUI(curOpenUIList[i], false, true);
            }

            openOrderList.Clear();
        }

        /// <summary>
        /// 关闭指定类型的UI
        /// </summary>
        /// <param name="depth"></param>
        public void CloseUI(eUIDepth depth)
        {
            for (int i = curOpenUIList.Count - 1; i >= 0; i--)
            {
                eUIName name = curOpenUIList[i];

                UIBase uiBase = allUIList[name];

                if (uiBase != null && uiBase.uiDepth == depth)
                {
                    CloseUI(name, false, true);
                }
            }
        }

        private void UnloadUIResouse(eUIName uiName)
        {
            UIBase uiBase = allUIList[uiName];

            string abName = uiName.ToString().ToLower() + ".unity3d";

            //删除游戏物体;
            if(uiBase.gameObject!=null)
            {
                GameObject.Destroy(uiBase.gameObject);
            }
        }
    }

}