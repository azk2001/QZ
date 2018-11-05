using System;
using System.Collections;
using System.Collections.Generic;

public static class GenericExtensionMethods
{
    #region 数组

    /// <summary>
    /// 添加元素到末尾
    /// </summary>
    public static T[] Add<T>(this T[] ts, T data)
    {
        if (ts == null)
            ts = new T[1] { data };
        else
        {
            int len = ts.Length;
            Array.Resize(ref ts, ts.Length + 1);
            ts[len] = data;
            //                int len = ts.Length;
            //                T[] newt = new T[len + 1];
            //                Array.Copy(ts, newt, len);
            //                newt[len] = data;
            //                ts = newt;
        }
        return ts;
    }

    /// <summary>
    /// 将元素插入指定的位置
    /// </summary>
    public static T[] Insert<T>(this T[] ts, T data, int index)
    {
        if (ts == null)
        {
            ts = new T[1] { data };
        }
        else
        {
            int len = ts.Length;
            if (len < index)
                index = len;

            T[] newt = new T[len + 1];
            Array.Copy(ts, newt, index);
            for (int i = len; i > index; --i)
                newt[i] = ts[i - 1];
            newt[index] = data;
            ts = newt;
        }
        return ts;
    }

    /// <summary>
    /// 移除数组中指定的对象, 并返回新的数组
    /// </summary>
    public static T[] RemoveAt<T>(this T[] ts, int index)
    {
        if (ts == null)
            return ts;
        else
        {
            int len = ts.Length;
            if (len <= index)
                return ts;

            T[] newt = new T[len - 1];
            Array.Copy(ts, newt, index);
            for (int i = ts.Length - 1; i > index; --i)
                newt[i - 1] = ts[i];
            ts = newt;
        }
        return ts;
    }

    /// <summary>
    /// 搜索指定的对象, 并返回数组中最后一个匹配项的索引, 查找索引
    /// </summary>
    public static int IndexOf<T>(this T[] ts, T data)
    {
        if (ts == null)
            return -1;
        return Array.IndexOf(ts, data);
    }

    /// <summary>
    /// 搜索指定的对象, 并返回数组中最后一个匹配项的索引, 查找索引
    /// </summary>
    public static int IndexOf<T>(this T[] ts, T data, int startIndex, int count)
    {
        if (ts == null || startIndex >= ts.Length || count < 0)
            return -1;
        if (startIndex + count > ts.Length)
            count = ts.Length - startIndex;
        return Array.IndexOf(ts, data, startIndex, count);
    }

    /// <summary>
    /// 从后往前搜索指定的对象, 并返回数组中最后一个匹配项的索引, 查找索引
    /// </summary>
    public static int LastIndexOf<T>(this T[] ts, T data)
    {
        if (ts == null)
            return -1;
        return Array.LastIndexOf(ts, data);
    }

    /// <summary>
    /// 从后往前搜索指定的对象, 并返回数组中最后一个匹配项的索引, 查找索引
    /// </summary>
    public static int LastIndexOf<T>(this T[] ts, T data, int startIndex, int count)
    {
        if (ts == null || startIndex >= ts.Length || count < 0)
            return -1;
        if (startIndex + count > ts.Length)
            count = ts.Length - startIndex;
        return Array.LastIndexOf(ts, data, startIndex, count);
    }

    /// <summary>
    /// 指定一个值, 查找索引
    /// </summary>
    public static int FindIndex<T>(this T[] ts, Predicate<T> madch)
    {
        if (ts == null)
            return -1;
        return Array.FindIndex(ts, madch);
    }

    /// <summary>
    /// 指定一个值, 查找索引
    /// </summary>
    public static int FindIndex<T>(this T[] ts, Predicate<T> madch, int startIndex, int count)
    {
        if (ts == null || startIndex >= ts.Length || count < 0)
            return -1;
        if (startIndex + count > ts.Length)
            count = ts.Length - startIndex;
        return Array.FindIndex(ts, startIndex, count, madch);
    }

    /// <summary>
    /// 指定元素是否存在数组中
    /// </summary>
    public static bool Contains<T>(this T[] ts, T data)
    {
        if (ts == null)
            return false;
        return Array.IndexOf(ts, data) >= 0;
    }

    /// <summary>
    /// 获取最前一个元素
    /// </summary>
    public static T GetFront<T>(this T[] ts)
    {
        if (ts == null || ts.Length == 0)
            return default(T);
        return ts[0];
    }

    /// <summary>
    /// 获取最后一个元素
    /// </summary>
    public static T GetLast<T>(this T[] ts)
    {
        if (ts == null || ts.Length == 0)
            return default(T);
        return ts[ts.Length - 1];
    }

    /// <summary>
    /// 反转数组
    /// </summary>
    public static bool Reverse<T>(this T[] ts)
    {
        if (ts == null)
            return false;
        Array.Reverse(ts);
        return true;
    }

    /// <summary>
    /// 反转数组
    /// </summary>
    public static bool Reverse<T>(this T[] ts, int startIndex, int count)
    {
        if (ts == null)
            return false;
        if (ts == null || startIndex >= ts.Length || count < 0)
            return false;
        if (startIndex + count > ts.Length)
            count = ts.Length - startIndex;
        Array.Reverse(ts, startIndex, count);
        return true;
    }

    public static bool Sort<T>(this T[] ts)
    {
        if (ts == null)
            return false;

        Array.Sort(ts);
        return true;
    }

    public static bool Sort<T>(this T[] ts, Comparison<T> comparer)
    {
        if (ts == null)
            return false;

        Array.Sort(ts, comparer);
        return true;
    }

    //        public static bool Sort<T>(this T[] ts, int startIndex, int count, Comparison<T> comparer)
    //        {
    //            if (ts == null)
    //                return false;
    //
    //            if (ts == null || startIndex >= ts.Length || count < 0)
    //                return false;
    //            if (startIndex + count > ts.Length)
    //                count = ts.Length - startIndex;
    //            Array.Sort(ts, startIndex, count, comparer);
    //            return true;
    //        }

    /// <summary>
    /// 移除一个元素并且位移数据, 但保留原始内存
    /// </summary>
    public static bool RemoveAtRetainMem<T>(this T[] ts, int index)
    {
        if (ts == null)
            return false;

        int endIndex = ts.Length - 1;
        if (endIndex < index)
            return false;

        while (index < endIndex)
        {
            ts[index] = ts[index + 1];
            ++index;
        }
        //Array.Copy(ts, index + 1, ts, index, ts.Length - index - 1);
        return true;
    }

    /// <summary>
    /// 将指定长度的元素拷贝的新内存中 返回
    /// </summary>
    public static T[] SetAddMem<T>(this T[] ts, int len)
    {
        if (ts == null)
            ts = new T[len];
        else
        {
            if (len <= ts.Length)
                return ts;

            T[] t = new T[len];
            Array.Copy(ts, t, ts.Length);
            return t;
        }
        return ts;
    }

    #endregion


    #region Dictionary

    public static V TryGetValue<K, V>(this Dictionary<K, V> that, K key)
    {
        V value;
        that.TryGetValue(key, out value);
        return value;
    }
    public static V TryGetValue<K, V>(this Dictionary<K, V> that, K key, V defValue)
    {
        if (that == null)
            return defValue;
        V value;
        if (that.TryGetValue(key, out value))
            return value;
        return defValue;
    }

    #endregion


    #region List

    public static T GetFront<T>(this List<T> ts)
    {
        if (ts == null || ts.Count == 0)
            return default(T);
        return ts[0];
    }
    public static T GetLast<T>(this List<T> ts)
    {
        if (ts == null || ts.Count == 0)
            return default(T);
        return ts[ts.Count - 1];
    }

    #endregion

}