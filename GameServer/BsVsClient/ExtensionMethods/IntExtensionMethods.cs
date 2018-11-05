using System;
using System.Collections;
using Int8 = System.SByte;
using UInt8 = System.Byte;

namespace MXEngine
{
    public static class IntExtensionMethods
    {
        #region int to string

        public static bool TryParse(this UInt8 that, out string value)
        {
            value = sString.GetString(that);
            return true;
        }

        public static bool TryParse(this Int8 that, out string value)
        {
            value = sString.GetString(that);
            return true;
        }

        public static bool TryParse(this Int16 that, out string value)
        {
            value = sString.GetString(that);
            return true;
        }

        public static bool TryParse(this Int32 that, out string value)
        {
            value = sString.GetString(that);
            return true;
        }

        public static bool TryParse(this Int64 that, out string value)
        {
            value = that.ToString();
            return true;
        }

        #endregion


        /// <summary>
        /// 获取一个数字长度, 位数
        /// </summary>
        public static int GetDigitCount(this int that)
        {
            that = that.ToAbs();
            if (that == 0)
                return 1;
            int count = 0;
            while(that != 0)
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

        public static int Clamp(this int that, int min, int max)
        {
            return that < min ? min : (that > max ? max : that);
        }

        public static bool TryParse(this Int32 that, byte[] value, int startIndex)
        {
            if(startIndex + sizeof(Int32) > value.Length)
                return false;
            
            value[startIndex] = (byte)(that);
            value[startIndex + 1] = (byte)(that >> 8);
            value[startIndex + 2] = (byte)(that >> 16);
            value[startIndex + 3] = (byte)(that >> 24);
            return true;
        }

        public static bool TryParse(this byte[] that, out Int32 value, int startIndex)
        {
            if(startIndex + sizeof(Int32) > that.Length)
            {
                value = 0;
                return false;
            }

            value = (Int32)(that[startIndex] + 
                    ((Int32)that[startIndex + 1] << 8) + 
                    ((Int32)that[startIndex + 2] << 16) +
                    ((Int32)that[startIndex + 3] << 24));
            return true;
        }




















        /// <summary>
        /// 获取缓存字符串
        /// </summary>
        public static string GetString(this int that)
        {
            return sString.GetString(that);
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
        }
    }
}