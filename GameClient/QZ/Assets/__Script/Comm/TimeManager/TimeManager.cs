using UnityEngine;
using System.Collections;

public class TimeManager
{
    private float time_time = 0;
    private static TBuffer<Node> smNodeBuffers = new TBuffer<Node>(OnInstantiation);
    private static Node gHead = new Node();

    private static TimeManager _instance = null;
    public static TimeManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new TimeManager();
                gHead.mNext = gHead.mPrev = gHead;
            }
            return _instance;
        }
    }

  

    //等待start通知onCall
    //委托返回值决定下一次通知时间
    //返回小于0,代表结束
    public void Begin(float start, FloatDelegate onCall)
    {
        if (start < 0.0f || onCall == null)
            return;

        Node p = smNodeBuffers.Get();
        p.mEndTime = Time.realtimeSinceStartup + start;
        p.onCall = onCall;
        p.Start();

    }

    //一般使用返回值决定结束 -1.0f
    public bool End(FloatDelegate onCall)
    {
        bool result = false;
        Node p = gHead.mNext;
        while (p != gHead)
        {
            if (onCall == p.onCall)
            {
                p.mEndTime = -1.0f;//表示删除
                p.onCall = null;
                result = true;
            }
            p = p.mNext;
        }

        return result;
    }

    // Update is called once per frame
    public void Update()
    {
        if (gHead == gHead.mNext)
            return;

        time_time = Time.realtimeSinceStartup;
        Node p = gHead.mNext;
        while (p != gHead)
        {
            if (p.mEndTime > 0.0f && p.mEndTime <= time_time)
            {
                if (p.onCall != null)
                {
                    float endTime = p.mEndTime;
                    float f = p.onCall();
                    if (f < 0.0f)
                    {
                        p.mEndTime = -1.0f;

                        if (p._isBufferObject)
                            p.onCall = null;
                    }
                    else
                    {
                        endTime = time_time - endTime;
                        if (endTime > 0.0f)
                            p.mEndTime = time_time + f - endTime;//endTime为多用的时间 下一次需要少等待
                        else
                            p.mEndTime = time_time + f;
                    }
                }
                else
                    p.mEndTime = -1.0f;
            }
            p = p.mNext;
        }

        //检查移除节点,放入缓存
        p = gHead.mNext;

        while (p != gHead)
        {
            if (p.mEndTime < 0.0f)
            {
                p.Stop();

                if (p._isBufferObject)
                    smNodeBuffers.Release(p);
            }
            p = p.mNext;
        }
    }

    //成员
    public class Node
    {
        public Node mNext;
        public Node mPrev = null;
        public FloatDelegate onCall;
        public float mEndTime;

        public bool _isBufferObject;

        public Node(bool isBufferObject = false)
        {
            _isBufferObject = isBufferObject;
        }

        public bool isStart
        {
            get
            {
                return mPrev != null;
            }
        }

        //外部可以使用该方法(Stop自动的)
        public void Begin(float endTime)
        {
            mEndTime = Time.realtimeSinceStartup + endTime;

            if (mPrev == null)
                Start();
        }

        public void End()
        {
            mEndTime = -1.0f;
        }


        public void Stop()
        {
            if (mPrev == null)
            {
                Debug.LogError("时间管理器停止失败");
                return;
            }

            mNext.mPrev = mPrev;
            mPrev.mNext = mNext;

            mPrev = null;
        }

        public void Start()
        {
            if (mPrev != null)
            {
                Debug.LogError("时间管理器开启失败");
                return;
            }

            mNext = gHead.mNext;
            mPrev = gHead;
            gHead.mNext = gHead.mNext.mPrev = this;
        }
    }

    private static Node OnInstantiation()
    {
        return new Node(true);
    }

}
