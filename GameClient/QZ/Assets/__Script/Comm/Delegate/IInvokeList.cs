using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//模板事件类类
//适用于通用类和管理类
public class IInvokeList<DATA>
{
    //通知数据_该类应该可以写成共有的
    //该类应该可以写成共有的, 为了简化 目前不提供
    protected abstract class IInvoke
    {
        public object mObjectCode;
        public abstract void OnInvoke(DATA data);
        public abstract void OnRemove();
    }

    //全部数据
    List<IInvoke> mLoadRefList = new List<IInvoke>();

    //通知中的节点
    int mInvokeingIndex = -1;
    IInvoke mInvokeingData = null;
    bool mIsClearList = false;

    public int count
    {
        get { return mLoadRefList.Count; }
    }

    public void Clear()
    {
        for (int i = 0, max = mLoadRefList.Count; i < max; ++i)
        {
            mLoadRefList[i].OnRemove();
        }
        mLoadRefList.Clear();
    }

    //通知所有事件
    public void InvokeAll(DATA data)
    {
        mIsClearList = true;
        mInvokeingIndex = mLoadRefList.Count - 1;
        while (mInvokeingIndex >= 0)
        {
            mInvokeingData = mLoadRefList[mInvokeingIndex];
            if (mInvokeingData.mObjectCode != null)
            {
                //					#if UNITY_EDITOR
                //					try
                //					{
                //						mInvokeingData.OnInvoke(data);
                //					}
                //					catch(System.Exception e)
                //					{
                //						Debuger.LogErrorFormat("通知时发现异常, 需要立即处理-----, funs: {0} msg: {1}", mInvokeingData.mObjectCode.GetType().Name, e.Message);
                //					}
                //					#else
                mInvokeingData.OnInvoke(data);
                //#endif

                if (mInvokeingData.mObjectCode != null)
                    mIsClearList = false;
            }
            --mInvokeingIndex;
        }

        mInvokeingData = null;
        if (mIsClearList)
        {
            Clear();
        }
        else
        {
            mInvokeingIndex = mLoadRefList.Count - 1;
            while (mInvokeingIndex >= 0)
            {
                if (mLoadRefList[mInvokeingIndex].mObjectCode == null)
                {
                    mLoadRefList.RemoveAt(mInvokeingIndex);
                }
                --mInvokeingIndex;
            }
        }
    }

    protected void AddListener(IInvoke iInvoke)
    {
        mIsClearList = false;
        mLoadRefList.Add(iInvoke);
    }

    //移除所有引用
    public bool RemoveListener(object objectCode)
    {
        if (mInvokeingData != null)
        {
            if (object.Equals(mInvokeingData.mObjectCode, objectCode))
            {
                mInvokeingData.mObjectCode = null;
                return true;
            }
            else
            {
                for (int i = mLoadRefList.Count - 1; i >= 0; --i)
                {
                    if (object.Equals(mLoadRefList[i].mObjectCode, objectCode))
                    {
                        if (mInvokeingIndex < i)
                            mIsClearList = false;
                        mLoadRefList[i].mObjectCode = null;
                        return true;
                    }
                }
            }
        }
        else
        {
            for (int i = mLoadRefList.Count - 1; i >= 0; --i)
            {
                //委托转换会不能直接用==
                if (object.Equals(mLoadRefList[i].mObjectCode, objectCode))
                {
                    IInvoke iInvoke = mLoadRefList[i];
                    iInvoke.OnRemove();
                    mLoadRefList.RemoveAt(i);
                    return true;
                }
            }
        }

        Debug.LogError("没有找到 objectCode:{0}, 可能重复调用了RemoveListener");
        return false;
    }
}