using System.Collections;


public static class IntExtensionMethods
{
    /// <summary>
    /// 获取一个数字长度, 位数
    /// </summary>
    public static int GetDigitCount(this int that)
    {
        that = that.ToAbs();
        if (that == 0)
            return 1;
        int count = 0;
        while (that != 0)
        {
            ++count;
            that /= 10;
        }
        return count;
    }

    public static int ToAbs(this int that)
    {
        return that > 0 ? that : -that;
    }


    /// <summary>
    /// 获取缓存字符串
    /// </summary>
    public static string GetString(this int that)
    {
        return sString.GetString(that);
    }

    /// <summary>
    /// 获取百分比 追加+和-
    /// </summary>
    public static string GetString0_1CD(this float that)
    {
        return sString.ParseCD(that);
    }

    /// <summary> 
    /// <para>-------缓存分配int字符串-------</para> 
    /// <para>TOOLS.sString.Parse(int),可以得到一个字符串</para> 
    /// <para>cm_iUNumberMax,cm_iSNumberMin成员可以查看缓存范围</para> 
    /// </summary>
    private static class sString
    {
        /// <summary> 
        /// 缓存最大值
        /// </summary>
        public const int cmUNumberMax = 131072;

        /// <summary> 
        /// 缓存最小值
        /// </summary>
        public const int cmSNumberMin = -1024;

        private static string[] smStrUNumbers = new string[cmUNumberMax];  //正数组
        private static string[] smStrSNumbers = new string[-cmSNumberMin]; //负数组

        private static string[] smStrTime = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09" };


        //对int进行扩展
        public static string GetString(int _value)
        {
            //如果数字大于一定程度 就不管理了
            if (_value >= cmUNumberMax || _value <= cmSNumberMin)
                return _value.ToString();

            if (_value < 0)//负数
            {
                if (smStrSNumbers[-_value] == null)
                    smStrSNumbers[-_value] = _value.ToString();
                return smStrSNumbers[-_value];
            }

            //正数
            if (smStrUNumbers[_value] == null)
                smStrUNumbers[_value] = _value.ToString();
            return smStrUNumbers[_value];
        }

        public static string ParseTime(int _value)
        {
            return _value >= 0 && _value < 10 ? smStrTime[_value] : GetString(_value);
        }

        private static string[] smStrTimeDecimals = { "0", "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9" };

        //解析float最后1秒保留0.1
        public static string ParseCD(float _value)
        {
            if (_value < 0.0f)
                _value = 0.0f;

            if (_value < 1.0f)
                return smStrTimeDecimals[(int)(_value * 10)];

            return GetString((int)_value);
        }
    }
}