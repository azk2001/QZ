using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//不足2位保留2位00
public class UITime : MonoBehaviour
{
    //计算方法
    public enum EType
    {
        Error = -1,
        Second = 0,             //有秒
        SecondMinute,           //有秒,分
        SecondMinuteHour,       //有秒,分,时
        SecondMinuteHourDay,    //有秒,分,时,天

        CustomDay,              //自定义,有天只显示天,否则显示时:分:秒
        SecondNotFormat,        //只有秒不使用格式化

        LocalTimeMinuteHour,    //获取本地时间, 按mFormat0样式进行显示,,分,时

        //大于24小时显示天, 大于1小时显示小时, 大于1分钟显示1分钟, 否则显示秒
        //mFormat0, 为前缀, mFormats 为后缀, 下标[0天][1时][2分][3秒]
        SecondMinuteHourDaySole,
        DayHourMinuteSecond,

    }

    public Text mText; 		            		//被改变的对象
    public EType mType = EType.SecondMinute; 	//显示的类型
    public float mFrom = 120.0f;             	//开始时间
    public float mTo = 0;                  	    //结束时间
    public float mDuration = 1.0f;           	//每多少时间改变一次mUILabel
    public string mFormat0 = "{1}:{0}";     	//普通的显示
    public string mFormat1;                     //特殊显示
    public string[] mFormats;                   //特殊显示

    private float mNextTime = 0.0f;

    public int mRetainTimeCount = 2;

    public static float curTime { get { return Time.time; } }

    public OnUpdate onUpdate;                       //可以由动态库设置

    public Action<UITime> onFinish = null;
    //	[SerializeField]
    //   public List<EventDelegate> onFinish = new List<EventDelegate>();

    public bool isPause = false;

    public object param = null;

    private string[] smStrTime = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09" };

    //需要在Start或以后调用
    public void Begin(float _beginTime, float _endTime)
    {
        mFrom = _beginTime;
        mTo = _endTime;

        if (onUpdate == null)
            SetType(mType);

        onUpdate();
        mNextTime = curTime + mDuration;
        enabled = true;
    }

    public bool GetIsFinish()
    {
        return !enabled;
    }

    public void SetType(EType _type)
    {
        switch (mType = _type)
        {
            case EType.Second: onUpdate = OnSecond; break;
            case EType.SecondMinute: onUpdate = OnSecondMinute; break;
            case EType.SecondMinuteHour: onUpdate = OnSecondMinuteHour; break;
            case EType.SecondMinuteHourDay: onUpdate = OnSecondMinuteHourDay; break;
            case EType.CustomDay: onUpdate = OnCustomDay; break;
            case EType.SecondNotFormat: onUpdate = OnSecondNotFormat; break;
            case EType.LocalTimeMinuteHour: onUpdate = OnLocalTimeMinuteHour; break;
            case EType.SecondMinuteHourDaySole: onUpdate = OnSecondMinuteHourDaySole; break;
            case EType.DayHourMinuteSecond: onUpdate = OnDayHourMinuteSecond; break;
        }
    }


    void Awake()
    {
        if (mText == null)
            mText = GetComponent<Text>();

        if (onUpdate == null)
            SetType(mType);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause)
            return;

        if (mNextTime < curTime)
        {
            if (UpdateBeginTime())
            {
                if (onUpdate != null)
                    onUpdate();

                //if (onFinish.Count > 0)
                //    EventDelegate.Execute(onFinish);
                if (onFinish != null)
                {
                    onFinish(this);
                }
                enabled = false;
            }
            else if (onUpdate != null)
                onUpdate();

            mNextTime = curTime + mDuration + (curTime - mNextTime);
        }
    }

    private bool UpdateBeginTime()
    {
        if (mFrom < mTo)
        {
            mFrom += mDuration;

            if (mFrom >= mTo)
                return true;
        }
        else
        {
            mFrom -= mDuration;

            if (mFrom <= mTo)
                return true;
        }
        return false;
    }



    public delegate void OnUpdate();



    //方法多种---------------------------------
    public void OnSecond()
    {
        if (mRetainTimeCount == 1)
            mText.text = string.Format(mFormat0, (int)mFrom);
        else
            mText.text = string.Format(mFormat0, ParseTime((int)mFrom));
    }

    public void OnSecondMinute()
    {
        int second = (int)mFrom % 60;
        int minute = (int)mFrom / 60;
        mText.text = string.Format(mFormat0, ParseTime(second), ParseTime(minute));
    }

    public void OnSecondMinuteHour()
    {
        int second = (int)mFrom % 60;
        int minute = (int)mFrom / 60 % 60;
        int hour = (int)mFrom / (60 * 60);
        mText.text = string.Format(mFormat0, ParseTime(second), ParseTime(minute), ParseTime(hour));
    }

    public void OnSecondMinuteHourDay()
    {
        int second = (int)mFrom % 60;
        int minute = (int)mFrom / 60 % 60;
        int hour = (int)mFrom / 3600;
        int day = hour / 24;
        hour %= 24;
        mText.text = string.Format(mFormat0, ParseTime(second), ParseTime(minute), ParseTime(hour), ParseTime(day));
    }
    //123
    //213
    //12
    public void OnCustomDay()
    {
        int second = (int)mFrom % 60;
        int minute = (int)mFrom / 60 % 60;
        int hour = (int)mFrom / 3600;
        int day = hour / 24;
        hour %= 24;

        if (day > 2)
            mDuration = 60 * 60 * 24;//1天以后变换一次

        if (day > 0)
            mText.text = string.Format(mFormat1, day);
        else
            mText.text = string.Format(mFormat0, ParseTime(second), ParseTime(minute), ParseTime(hour));
    }

    public void OnSecondNotFormat()
    {
        mText.text = mFrom.ToString();
    }

    public void OnLocalTimeMinuteHour()
    {
        System.DateTime nowTime = System.DateTime.Now;

        mText.text = string.Format(mFormat0, ParseTime(nowTime.Hour), ParseTime(nowTime.Minute));
    }
    public void OnSecondMinuteHourDaySole()
    {
        if (mFormats.Length < 4)
        {
            return;
        }
        int second = (int)mFrom % 60;
        int minute = (int)mFrom / 60 % 60;
        int hour = (int)mFrom / (60 * 60);
        int day = hour / 24;

        //mFormats 为后缀, 下标[0天][1时][2分][3秒]
        if (day > 0)
        {
            mText.text = string.Concat(mFormat1, day, mFormats[0]);

            if (day > 1)
                mDuration = 60 * 60 * 24 * 1;//1天以后变换一次
        }
        else if (hour > 0)
        {
            mText.text = string.Concat(mFormat1, hour, mFormats[1]);

            if (hour > 1)
                mDuration = 60 * 60;//1小时之后变化一次
        }
        else if (minute > 0)
        {
            mText.text = string.Concat(mFormat1, minute, mFormats[2]);

            if (minute > 1)
                mDuration = 60;//1分钟之后变化一次
        }
        else
        {
            mDuration = 1;
            mText.text = string.Concat(mFormat1, second, mFormats[3]);
        }
    }
    //大于24时，显示N天N时N分
    // 大于1小时，显示N时N分N秒
    //大于60秒，显示N分N秒
    //大于0秒，不足1分钟
    public void OnDayHourMinuteSecond()
    {
        if (mFormats.Length < 4)
        {
            return;
        }
        int second = (int)mFrom % 60;
        int minute = (int)mFrom / 60 % 60;
        int hour = (int)mFrom / (60 * 60);
        int day = hour / 24;
        hour %= 24;

        //mFormats 为后缀, 下标[0天][1时][2分][3秒]
        if (day > 0)
        {
            mDuration = 60;
            mText.text = string.Format(mFormats[0], day, hour, minute);
        }
        else if (hour > 0)
        {
            mDuration = 1;
            mText.text = string.Format(mFormats[1], hour, minute, second);
        }
        else if (minute > 0)
        {
            mDuration = 1;
            mText.text = string.Format(mFormats[2], minute, second);
        }
        else
        {
            mDuration = 1;
            mText.text = mFormats[3];
        }
    }

    public void SetEnable()
    {
        gameObject.SetActive(true);
    }

    public void SetDisable()
    {
        gameObject.SetActive(false);
    }

    public string ParseTime(int _value)
    {
        return _value >= 0 && _value < 10 ? smStrTime[_value] : _value.ToString();
    }

    public void SetZero(string txt = "00:00:00")
    {
        enabled = false;
        mText.text = txt;
    }

}
