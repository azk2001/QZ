#define SAFEMODE
using System.Collections;
using UnityEngine;
namespace MXEngine
{
    public static class StringExtensionMethods
    {
        //int相关
        public static bool TryToInt(this string that, out int value)
        {
            //系统方法
            //            int value;
            //            if (string.IsNullOrEmpty(that) || !int.TryParse(that, out value))
            //                return Debuger.PopWarning("string to int error, str:" + that + ", def result", 0);
            //            return value;
            //自己的方法
            value = 0;
            if(string.IsNullOrEmpty(that))
            {
                return false;
            }
            int tempValue = 0;
            int index = 0;
            bool ism = that[0] == '-';
            if(ism)
                ++index;
            for(int max = that.Length; index < max; ++index)
            {
                int v = that[index] - '0';

                #if SAFEMODE
                if(v < 0 || v > 9)
                    return false;
                #endif

                tempValue = tempValue * 10 + v;
            }
            value = ism ? -tempValue : tempValue;
            return true;
        }

        //int相关
        public static int ToInt(this string that, int defValue)
        {
//系统方法
//            int value;
//            if (string.IsNullOrEmpty(that) || !int.TryParse(that, out value))
            //                return defValue;
//            return value;

//自己的方法
            if(string.IsNullOrEmpty(that))
                return defValue;

            int value = 0;
            int index = 0;
            bool ism = that[0] == '-';
            if(ism)
                ++index;
            for(int max = that.Length; index < max; ++index)
            {
                int v = that[index] - '0';

                #if SAFEMODE
                if(v < 0 || v > 9)
                    return defValue;
                #endif

                value = value * 10 + v;
            }
            return ism ? -value : value;
        }

        public static int[] ToInts(this string[] that, int defValue)
        {
            if (that == null || that.Length == 0)
                return new int[0];              
            int[] values = new int[that.Length];
            for(int i = 0, max = that.Length; i < max; ++i)
                values[i] = that[i].ToInt(defValue);
            return values;
        }

        //float相关
        public static bool TryToFloat(this string that, out float value)
        {
//            float value;
//            if (string.IsNullOrEmpty(that) || !float.TryParse(that, out value))
//                return Debuger.PopWarning("string to float error, str:" + that + ", def result 0", defValue);
//            return value;


            value = 0f;
            if(string.IsNullOrEmpty(that))
            {
                return false;
            }

            int v;
            char c;
            int tempValue = 0;
            int index = 0;
            bool ism = that[0] == '-';
            if(ism)
                ++index;
            
            for(int max = that.Length; index < max; ++index)
            {
                c = that[index];
                if(c == '.')
                {
                    //解析小数部分
                    ++index;
                    int value2 = 0;
                    float len2 = 1;
                    while(index < max)
                    {
                        v = that[index] - '0';
                        #if SAFEMODE
                        if(v < 0 || v > 9)
                            return false;
                        #endif
                        value2 = value2 * 10 + v;
                        ++index;
                        len2 *= 10;
                    }
                    value = ism ? -(tempValue + value2 / len2) : (tempValue + value2 / len2);
                    return true;
                }

                v = c - '0';

                #if SAFEMODE
                if(v < 0 || v > 9)
                    return false;
                #endif

                tempValue = tempValue * 10 + v;
            }
            value = ism ? -tempValue : tempValue;
            return true;
        }

        //float相关
        public static float ToFloat(this string that, float defValue)
        {
            //            float value;
            //            if (string.IsNullOrEmpty(that) || !float.TryParse(that, out value))
            //                return Debuger.PopWarning("string to float error, str:" + that + ", def result 0", defValue);
            //            return value;

            if(string.IsNullOrEmpty(that))
            {
                return defValue;
            }

            int v;
            char c;
            int value = 0;
            int index = 0;
            bool ism = that[0] == '-';
            if(ism)
                ++index;

            for(int max = that.Length; index < max; ++index)
            {
                c = that[index];
                if(c == '.')
                {
                    //解析小数部分
                    ++index;
                    int value2 = 0;
                    float len2 = 1;
                    while(index < max)
                    {
                        v = that[index] - '0';
                        #if SAFEMODE
                        if(v < 0 || v > 9)
                            return defValue;
                        #endif
                        value2 = value2 * 10 + v;
                        ++index;
                        len2 *= 10;
                    }
                    return ism ? -(value + value2 / len2) : (value + value2 / len2);
                }

                v = c - '0';

                #if SAFEMODE
                if(v < 0 || v > 9)
                    return defValue;
                #endif

                value = value * 10 + v;
            }
            return ism ? -value : value;
        }

        public static float[] ToFloats(this string[] that, float defValue)
        {
            if (that == null || that.Length == 0)
                return new float[0];              
            float[] values = new float[that.Length];
            for(int i = 0, max = that.Length; i < max; ++i)
                values[i] = that[i].ToFloat(defValue);
            return values;
        }

		public static Vector3 ToVector3(this string that)
		{
			that = that.Replace("(", "").Replace(")", "");
			string[] li = that.Split(',');
			return new Vector3(float.Parse(li[0]),float.Parse(li[1]),float.Parse(li[2]));
		}
        //缓存取出int-------------------------------------------------
    }
}