using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

//还要写一个对象序列化 字典的方式解析的类, 适用于游戏中字典存储(2进制方式存储和读取)
public class Table : ITable
{
    /// <summary>
    /// 共享使用字段, 确保当前数据不重叠, 注用完可以share = null; 确保内存干净
    /// </summary>
    public static Table share
    {
        get
        {
            if (_share == null)
                _share = new Table();
            return _share;
        }
        set { _share = value; }
    }
    static Table _share;

    //解析组数据相关规则
    //例如vector2 xy == "x&y"
    public const char splitObject = '&';

    //例如List<int> 123 = "1;2;3"
    public const char splitArray = ';';

    //string类型的key换算符
    public const char splitStringKey = '&';
    public const string splitStringKeyStr = "&";

    //该字段为扩展字段
    public string fileName;

    public string[] colNames
    { private set; get; }

    public string[] colTypes
    { get { return mData[1]; } }

    public int rowWidth
    { private set; get; }

    public int rowHeight
    { private set; get; }


    #region 通用方法

    //序列化后, 列名不能更改, 添加， 或删除
    public bool Serialize(string jsonText)
    {
        if (string.IsNullOrEmpty(jsonText))
            return false;

        Clear();
        Parse(jsonText);
        return true;
    }

    public bool SerializeFileInUTF8(string filePath)
    {
        if (!System.IO.File.Exists(filePath))
            return false;

        string jsonText = System.IO.File.ReadAllText(filePath, Encoding.UTF8);
        fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
        return Serialize(jsonText);
    }

    public void Clear()
    {
        if (mRowNameToIndex != null)
            mRowNameToIndex.Clear();
        mColNameToIndex.Clear();
    }

    public bool ContainsRowName(string rowName)
    {
        if (mRowNameToIndex == null || mRowNameToIndex.Count == 0)
            AnalysisRow();

        return mRowNameToIndex.ContainsKey(rowName);
    }

    public bool ContainsColName(string colName)
    {
        return mColNameToIndex.ContainsKey(colName);
    }

    #endregion


    #region 用法1, 配置表生成操作相关
    public int TryGetColIndex(string colName)
    {
        int colIndex;
        if (!mColNameToIndex.TryGetValue(colName, out colIndex))
        {
            GetErrorNotExistsCol<int>(colName);
            return -1;
        }
        return colIndex;
    }

    public string TryGetString(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex<string>(rowIndex, colIndex, "");

        return mData[rowIndex][colIndex];
    }

    public int TryGetInt(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex<int>(rowIndex, colIndex, 0);

        int value;
        if (!mData[rowIndex][colIndex].TryToInt(out value))
            return GetErrorParseError<int>(rowIndex, colIndex, 0);

        return value;
    }

    public Int64 TryGetInt64(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex<int>(rowIndex, colIndex, 0);

        Int64 value;
        if (!Int64.TryParse(mData[rowIndex][colIndex], out value))
            return GetErrorParseError<int>(rowIndex, colIndex, 0);

        return value;
    }

    public float TryGetFloat(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex<float>(rowIndex, colIndex, 0.0f);

        float value;
        if (!mData[rowIndex][colIndex].TryToFloat(out value))
            return GetErrorParseError<float>(rowIndex, colIndex, 0.0f);

        return value;
    }

    //配置表参考"true"或"false"
    public bool TryGetBool(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex<bool>(rowIndex, colIndex, false);

        int value;
        if (!mData[rowIndex][colIndex].TryToInt(out value))
            return GetErrorParseError<bool>(rowIndex, colIndex, false);

        return value != 0;
    }

    public Vector2 TryGetVector2(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex<Vector2>(rowIndex, colIndex, new Vector2());

        return TryGetVector2(mData[rowIndex][colIndex], rowIndex, colIndex);
    }

    public Vector3 TryGetVector3(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex(rowIndex, colIndex, new Vector3());

        return TryGetVector3(mData[rowIndex][colIndex], rowIndex, colIndex, splitObject);
    }

    public Vector2 TryGetVector2(int rowIndex, int colIndex, char so)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex(rowIndex, colIndex, new Vector2());

        string[] text = mData[rowIndex][colIndex].Split(so);

        return new Vector2(float.Parse(text[0]), float.Parse(text[1]));
    }

    /// <summary>
    /// 临时流程
    /// </summary>
    public Vector3 TryGetVector3(int rowIndex, int colIndex, char so)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex(rowIndex, colIndex, new Vector3());

        return TryGetVector3(mData[rowIndex][colIndex], rowIndex, colIndex, so);
    }

    public Vector4 TryGetVector4(int rowIndex, int colIndex, char so)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex(rowIndex, colIndex, new Vector3());

        return TryGetVector4(mData[rowIndex][colIndex], rowIndex, colIndex, so);
    }


    /// <summary>
    /// 临时流程
    /// </summary>
    public Color TryGetColor(int rowIndex, int colIndex, char sp)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex(rowIndex, colIndex, new Color());

        return TryGetColor(mData[rowIndex][colIndex], rowIndex, colIndex, sp);
    }

    public Color TryGetColor(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex(rowIndex, colIndex, new Color());

        return TryGetColor(mData[rowIndex][colIndex], rowIndex, colIndex, splitObject);
    }

    public string[] TryGetStringArray(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex<string[]>(rowIndex, colIndex, new string[0]);

        return mData[rowIndex][colIndex].Split(splitArray);
    }

    public int[] TryGetIntArray(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex<int[]>(rowIndex, colIndex, new int[0]);

        string[] strValues = mData[rowIndex][colIndex].Split(splitArray);
        int[] values = new int[strValues.Length];
        for (int i = 0, max = strValues.Length; i < max; ++i)
        {
            int value;
            if (strValues[i].TryToInt(out value))
                values[i] = value;
            else
                return GetErrorParseError<int[]>(rowIndex, colIndex, new int[0]);
        }
        return values;
    }

    public float[] TryGetFloatArray(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex<float[]>(rowIndex, colIndex, new float[0]);

        string[] strValues = mData[rowIndex][colIndex].Split(splitArray);
        float[] values = new float[strValues.Length];
        for (int i = 0, max = strValues.Length; i < max; ++i)
        {
            float value;
            if (strValues[i].TryToFloat(out value))
                values[i] = value;
            else
                return GetErrorParseError<float[]>(rowIndex, colIndex, new float[0]);
        }
        return values;
    }

    //配置表参考"true;flase"
    public bool[] TryGetBoolArray(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex<bool[]>(rowIndex, colIndex, new bool[0]);

        string[] strValues = mData[rowIndex][colIndex].Split(splitArray);
        bool[] values = new bool[strValues.Length];
        for (int i = 0, max = strValues.Length; i < max; ++i)
        {
            int value;
            if (strValues[i].TryToInt(out value))
                values[i] = value != 0;
            else
                return GetErrorParseError<bool[]>(rowIndex, colIndex, new bool[0]);
        }
        return values;
    }

    public Vector2[] TryGetVector2Array(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex(rowIndex, colIndex, new Vector2[0]);

        string[] strValues = mData[rowIndex][colIndex].Split(splitArray);
        Vector2[] values = new Vector2[strValues.Length];
        for (int i = 0, max = strValues.Length; i < max; ++i)
            values[i] = TryGetVector2(strValues[i], rowIndex, colIndex);
        return values;
    }

    public Vector3[] TryGetVector3Array(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex(rowIndex, colIndex, new Vector3[0]);

        string[] strValues = mData[rowIndex][colIndex].Split(splitArray);
        Vector3[] values = new Vector3[strValues.Length];
        for (int i = 0, max = strValues.Length; i < max; ++i)
            values[i] = TryGetVector3(strValues[i], rowIndex, colIndex, splitObject);
        return values;
    }

    public Color[] TryGetColorArray(int rowIndex, int colIndex)
    {
        if (colIndex < 0)
            return GetErrorNotExistsRowIndexOrColIndex(rowIndex, colIndex, new Color[0]);

        string[] strValues = mData[rowIndex][colIndex].Split(splitArray);
        Color[] values = new Color[strValues.Length];
        for (int i = 0, max = strValues.Length; i < max; ++i)
            values[i] = TryGetColor(strValues[i], rowIndex, colIndex, splitObject);
        return values;
    }

    Vector2 TryGetVector2(string jsonText, int rowIndex, int colIndex)
    {
        Vector2 value = new Vector2();
        if (string.IsNullOrEmpty(jsonText))
            return GetErrorParseError(rowIndex, colIndex, value);

        string[] tests = jsonText.Split(splitObject);
        if (tests.Length != 2)
            return GetErrorParseError(rowIndex, colIndex, value);

        if (!tests[0].TryToFloat(out value.x) ||
            !tests[1].TryToFloat(out value.y))
            return GetErrorParseError(rowIndex, colIndex, value);
        return value;
    }

    Vector3 TryGetVector3(string jsonText, int rowIndex, int colIndex, char so)
    {
        Vector3 value = new Vector3();
        if (string.IsNullOrEmpty(jsonText))
            return GetErrorParseError(rowIndex, colIndex, value);

        string[] tests = jsonText.Split(so);
        if (tests.Length != 3)
            return GetErrorParseError(rowIndex, colIndex, value);

        if (!tests[0].TryToFloat(out value.x) ||
            !tests[1].TryToFloat(out value.y) ||
            !tests[2].TryToFloat(out value.z))
            return GetErrorParseError(rowIndex, colIndex, value);
        return value;
    }

    Vector4 TryGetVector4(string jsonText, int rowIndex, int colIndex, char so)
    {
        Vector4 value = new Vector4();
        if (string.IsNullOrEmpty(jsonText))
            return GetErrorParseError(rowIndex, colIndex, value);

        string[] tests = jsonText.Split(so);
        if (tests.Length != 4)
            return GetErrorParseError(rowIndex, colIndex, value);

        if (!tests[0].TryToFloat(out value.x) ||
            !tests[1].TryToFloat(out value.y) ||
            !tests[2].TryToFloat(out value.z) ||
            !tests[3].TryToFloat(out value.w))
            return GetErrorParseError(rowIndex, colIndex, value);
        return value;
    }

    Color TryGetColor(string jsonText, int rowIndex, int colIndex, char sp)
    {
        Color value = new Color();
        if (string.IsNullOrEmpty(jsonText))
            return GetErrorParseError(rowIndex, colIndex, value);

        string[] tests = jsonText.Split(sp);
        if (tests.Length != 4)
            return GetErrorParseError(rowIndex, colIndex, value);

        if (!tests[0].TryToFloat(out value.r) ||
            !tests[1].TryToFloat(out value.g) ||
            !tests[2].TryToFloat(out value.b) ||
            !tests[3].TryToFloat(out value.a))
            return GetErrorParseError(rowIndex, colIndex, value);
        return value;
    }

    #endregion


    #region 用法2, 配置表直接操作相关(此操作rowName必须是唯一的)

    //序列化合并, 将数据合并到当前数据中
    public bool SerializeMerge(string jsonText, bool isAddCol)
    {
        if (string.IsNullOrEmpty(jsonText))
            return false;

        if (mColNameToIndex.Count == 0)
            return Serialize(jsonText);
        else
            ParseMerge(jsonText, isAddCol);

        return true;
    }

    public string Deserialize()
    {
        return Deserialize(false);
    }

    public string Deserialize(bool isSort)
    {
        StringBuilder sb = new StringBuilder();
        string k = "\t";

        if (isSort)
        {
            int[] rowIndexs = new int[rowHeight];
            for (int i = 0; i < rowHeight; ++i)
                rowIndexs[i] = i;
            Array.Sort(rowIndexs, Comparison);

            for (int i = 0; i < rowHeight; ++i)
                sb.AppendLine(String.Join(k, mData[rowIndexs[i]], 0, rowWidth));
        }
        else
        {
            for (int i = 0; i < rowHeight; ++i)
                sb.AppendLine(String.Join(k, mData[i], 0, rowWidth));
        }

        return sb.ToString();
    }

    public string TryGetString(string rowName, string colName)
    {
        if (mRowNameToIndex == null || mRowNameToIndex.Count == 0)
            AnalysisRow();

        int colIndex, rowIndex;
        if (!mColNameToIndex.TryGetValue(colName, out colIndex))
            return GetErrorNotExistsCol<string>(colName);
        if (!mRowNameToIndex.TryGetValue(rowName, out rowIndex))
            return GetErrorNotExistsRow<string>(rowName);
        return mData[rowIndex][colIndex];
    }

    public bool TrySetString(string rowName, string colName, string value)
    {
        if (mRowNameToIndex == null || mRowNameToIndex.Count == 0)
            AnalysisRow();

        int colIndex, rowIndex;
        if (!mColNameToIndex.TryGetValue(colName, out colIndex))
            return GetErrorNotExistsCol<bool>(colName);
        if (!mRowNameToIndex.TryGetValue(rowName, out rowIndex))
            return GetErrorNotExistsRow<bool>(rowName);
        mData[rowIndex][colIndex] = value;
        return true;
    }

    public bool TrySetRow(params string[] values)
    {
        if (mRowNameToIndex == null || mRowNameToIndex.Count == 0)
            AnalysisRow();

        int rowIndex;
        string rowName = values[0];
        if (!mRowNameToIndex.TryGetValue(rowName, out rowIndex))
            return false;

        Array.Copy(values, mData[rowIndex], rowWidth > values.Length ? values.Length : rowWidth);
        return true;
    }

    public bool TrySetRow(string jsonText)
    {
        return TrySetRow(jsonText.Split('\t'));
    }

    public bool SetString(string rowName, string colName, string value)
    {
        if (mRowNameToIndex == null || mRowNameToIndex.Count == 0)
            AnalysisRow();

        int colIndex, rowIndex;
        if (!mColNameToIndex.TryGetValue(colName, out colIndex))
            return GetErrorNotExistsCol<bool>(colName);
        if (!mRowNameToIndex.TryGetValue(rowName, out rowIndex))
        {
            rowIndex = AddRow(rowName);
            mData[rowIndex][0] = rowName;
        }
        mData[rowIndex][colIndex] = rowName;
        return true;
    }

    public bool RemoveRow(string rowName)
    {
        if (mRowNameToIndex == null || mRowNameToIndex.Count == 0)
            AnalysisRow();

        int rowIndex;
        if (!mRowNameToIndex.TryGetValue(rowName, out rowIndex))
            return false;

        mData.RemoveAtRetainMem(rowIndex);
        mRowNameToIndex.Remove(rowName);
        --rowHeight;

        while (rowIndex < rowHeight)
        {
            mRowNameToIndex[mData[rowIndex][0]] = rowIndex;
            ++rowIndex;
        }
        return true;
    }

    public void SetRow(params string[] values)
    {
        if (mRowNameToIndex == null || mRowNameToIndex.Count == 0)
            AnalysisRow();

        int rowIndex;
        string rowName = values[0];
        if (!mRowNameToIndex.TryGetValue(rowName, out rowIndex))
            rowIndex = AddRow(rowName);

        Array.Copy(values, mData[rowIndex], rowWidth > values.Length ? values.Length : rowWidth);
    }

    public void SetRow(string jsonText)
    {
        SetRow(jsonText.Split('\t'));
    }

    #endregion


    #region 私有部分

    string[][] mData = null;
    Dictionary<string, int> mColNameToIndex = new Dictionary<string, int>();
    Dictionary<string, int> mRowNameToIndex;//用的时候才有值

    void Parse(string josnText)
    {
        mData = GetArrar(josnText);
        rowHeight = mData.Length;

        colNames = mData[0];
        rowWidth = colNames.Length;
        for (int i = 0; i < rowWidth; ++i)
        {
            string colName = colNames[i];
            if (mColNameToIndex.ContainsKey(colNames[i]))
                GetErrorExistsRow(mColNameToIndex[colName], i);
            else
                mColNameToIndex.Add(colName, i);
        }
    }

    int AddCol(string colName)
    {
        if (colNames.Length == rowWidth)
        {
            for (int i = 0; i < rowHeight; ++i)
                mData[i] = mData[i].SetAddMem(rowWidth << 1);

            colNames = mData[0];
        }
        mData[0][rowWidth] = colName;
        mColNameToIndex.Add(colName, rowWidth++);
        return rowWidth - 1;
    }

    int AddRow(string rowName)
    {
        if (mData.Length == rowHeight)
            mData = mData.SetAddMem(rowHeight << 1);

        mData[rowHeight] = new string[colNames.Length];
        mRowNameToIndex.Add(rowName, rowHeight++);
        return rowHeight - 1;
    }

    void AnalysisRow()
    {
        if (mRowNameToIndex == null)
            mRowNameToIndex = new Dictionary<string, int>();

        for (int i = 0; i < rowHeight; ++i)
        {
            string rowName = mData[i][0];
            if (mRowNameToIndex.ContainsKey(rowName))
                GetErrorExistsRow(mRowNameToIndex[rowName], i);
            else
                mRowNameToIndex.Add(rowName, i);
        }
    }

    void ParseMerge(string text, bool isAddCol)
    {
        char[] rowEndTab = "\r\n".ToCharArray();
        string[] lines = text.Split(rowEndTab, System.StringSplitOptions.RemoveEmptyEntries);

        int height = lines.Length;

        string[] colNames = lines[0].Split('\t');

        if (!string.Equals(colNames[0], this.colNames[0]))
        {
            //合并失败
            Debuger.LogError(fileName + "文件合并列名不能重复: " + colNames[0]);
            return;
        }

        if (isAddCol)
        {
            for (int i = 1, max = colNames.Length; i < max; ++i)
            {
                if (!mColNameToIndex.ContainsKey(colNames[i]))
                    AddCol(colNames[i]);
            }
        }

        List<int> colIndexList = new List<int>();
        List<int> colIndexListDest = new List<int>();
        for (int i = 1, max = colNames.Length; i < max; ++i)
        {
            int index;
            if (mColNameToIndex.TryGetValue(colNames[i], out index))
            {
                colIndexList.Add(i);
                colIndexListDest.Add(index);
            }
        }

        if (mRowNameToIndex == null)
            AnalysisRow();

        for (int i = 1; i < height; ++i)
        {
            //因为是合并查找对应的列和行进行设置
            string[] rowData = lines[i].Split('\t');

            int rowIndex;
            string rowName = rowData[0];
            if (!mRowNameToIndex.TryGetValue(rowName, out rowIndex))
            {
                rowIndex = AddRow(rowName);
                mData[rowIndex][0] = rowName;
            }

            string[] rowDataDest = mData[rowIndex];
            for (int j = 0, jMax = colIndexList.Count; j < jMax; ++j)
                rowDataDest[colIndexListDest[j]] = rowData[colIndexList[j]];
        }
    }

    T GetErrorNotExistsCol<T>(string colName)
    {
        Debuger.LogError("fileName: " + fileName + ", colName: " + colName + ", not exists");
        return default(T);
    }

    T GetErrorNotExistsRow<T>(string rowName)
    {
        Debuger.LogError("fileName: " + fileName + ", rowName: " + rowName + ", not exists");
        return default(T);
    }

    T GetErrorNotExistsRowIndexOrColIndex<T>(int rowIndex, int colIndex, T defValue)
    {
        string colName = colIndex < colNames.Length ? colNames[colIndex] : colIndex.ToString();
        colName += "(" + colIndex + ")";
        Debuger.LogError("fileName: " + fileName + ", colName: " + colName + ", rowIndex: " + rowIndex + ", not exists");
        return defValue;
    }

    void GetErrorExistsRow(int prevRowIndex, int curRowIndex)
    {
        Debuger.LogError("fileName: " + fileName + ", row exists " + ", prevRowIndex: " + prevRowIndex + ", curRowIndex: " + curRowIndex);
    }

    T GetErrorParseError<T>(int rowIndex, int colIndex, T defValue)
    {
        string colName = colIndex < colNames.Length ? colNames[colIndex] : colIndex.ToString();
        colName += "(" + colIndex + ")";
        Debuger.LogError("fileName: " + fileName + ", rowIndex: " + rowIndex + ", colName: " + colName + ", parse error");
        return defValue;
    }

    int Comparison(int a, int b)
    {
        if (a == 0)
            return -1;
        else if (b == 0)
            return 1;

        return string.Compare(mData[a][0], mData[b][0]);
    }

    public static string[][] GetArrar(string josnText)
    {
        char[] rowEndTab = "\r\n".ToCharArray();
        string[] lines = josnText.Split(rowEndTab, System.StringSplitOptions.RemoveEmptyEntries);

        int rowHeight = lines.Length;
        string[][] data = new string[rowHeight][];
        for (int i = 0; i < rowHeight; ++i)
            data[i] = lines[i].Split('\t');

        return data;
    }

    #endregion


    #region key解析方法

    public static Int64 GetDicKey(int key1, int key2)
    {
        return ((Int64)key1 << 32) + key2;
    }

    public static Int64 GetDicKey(int key1, int key2, int key3)
    {
        return ((Int64)key1 << 40) + ((Int64)key2 << 20) + key3;
    }

    public static string GetDicKey(string key1, string key2)
    {
        return string.Concat(key1, splitStringKey, key2);
    }

    public static string GetDicKey(string key1, string key2, string key3)
    {
        return string.Concat(key1, splitStringKey, key2, splitStringKey, key3);
    }

    public static string GetDicKey(int key1, string key2)
    {
        return GetDicKey(key1.ToString(), key2);
    }

    public static string GetDicKey(string key1, int key2)
    {
        return GetDicKey(key1, key2.ToString());
    }

    //        public static string GetDicKey(params object[] objects)
    //        {
    //            return String.Join(splitStringKeyStr, objects, 0, objects.Length);
    //        }

    /// <summary>
    /// 使用此接口必须实现自己的ToString()   每行数据
    /// </summary>
    /// <returns>返回jsonText</returns>
    public static string Deserialize<K, V>(Dictionary<K, V> fileTable, params string[] headInfo)
    {
        StringBuilder stringBuilder = ObjectPool<StringBuilder>.Instance.Allot();
        stringBuilder.Length = 0;
        for (int i = 0, max = headInfo.Length; i < max; ++i)
            stringBuilder.AppendLine(headInfo[i]);
        foreach (KeyValuePair<K, V> kvp in fileTable)
            stringBuilder.AppendLine(kvp.Value.ToString());
        string strValue = stringBuilder.ToString();
        ObjectPool<StringBuilder>.Instance.Recycle(stringBuilder);
        return strValue;
    }

    /// <summary>
    /// 使用此接口必须实现自己的ToString()   每行数据
    /// </summary>
    /// <returns>返回jsonText</returns>
    public static string Deserialize<T>(List<T> fileTable, params string[] headInfo)
    {
        StringBuilder stringBuilder = ObjectPool<StringBuilder>.Instance.Allot();
        stringBuilder.Length = 0;
        for (int i = 0, max = headInfo.Length; i < max; ++i)
            stringBuilder.AppendLine(headInfo[i]);
        for (int i = 0, max = fileTable.Count; i < max; ++i)
            stringBuilder.AppendLine(fileTable[i].ToString());
        string strValue = stringBuilder.ToString();
        ObjectPool<StringBuilder>.Instance.Recycle(stringBuilder);
        return strValue;
    }

    /// <summary>
    /// 序列化数组
    /// </summary>
    /// <typeparam name="T">使用的类型必须完成ToString()函数</typeparam>
    /// <param name="value"></param>
    /// <returns>返回jsonText</returns>
    public static string Parse<T>(T[] value)
    {
        StringBuilder stringBuilder = ObjectPool<StringBuilder>.Instance.Allot();
        stringBuilder.Length = 0;
        int endIndex = value.Length - 1;
        for (int i = 0; i < endIndex; ++i)
        {
            stringBuilder.Append(Parse(value[i]));
            stringBuilder.Append(Table.splitArray);
        }
        stringBuilder.Append(Parse(value[endIndex]));
        string strValue = stringBuilder.ToString();
        ObjectPool<StringBuilder>.Instance.Recycle(stringBuilder);
        return strValue;
    }

    //默认类型
    public static string Parse<T>(T value)
    {
        return value.ToString();
    }

    //指定vector2的解析
    public static string Parse(UnityEngine.Vector2 value)
    {
        return string.Concat(value.x, Table.splitObject, value.y);
    }
    public static string Parse(Vector3 value)
    {
        return string.Concat(value.x, Table.splitObject, value.y, Table.splitObject, value.z);
    }

    public static string Parse(Color value)
    {
        return string.Concat(value.r, Table.splitObject, value.g, Table.splitObject, value.b, Table.splitObject, value.a);
    }

    public static string Parse(Vector2[] value)
    {
        StringBuilder stringBuilder = ObjectPool<StringBuilder>.Instance.Allot();
        stringBuilder.Length = 0;
        int endIndex = value.Length - 1;
        for (int i = 0; i < endIndex; ++i)
        {
            stringBuilder.Append(Parse(value[i]));
            stringBuilder.Append(Table.splitArray);
        }
        stringBuilder.Append(Parse(value[endIndex]));
        string strValue = stringBuilder.ToString();
        ObjectPool<StringBuilder>.Instance.Recycle(stringBuilder);
        return strValue;
    }

    public static string Parse(Vector3[] value)
    {
        StringBuilder stringBuilder = ObjectPool<StringBuilder>.Instance.Allot();
        stringBuilder.Length = 0;
        int endIndex = value.Length - 1;
        for (int i = 0; i < endIndex; ++i)
        {
            stringBuilder.Append(Parse(value[i]));
            stringBuilder.Append(Table.splitArray);
        }
        stringBuilder.Append(Parse(value[endIndex]));
        string strValue = stringBuilder.ToString();
        ObjectPool<StringBuilder>.Instance.Recycle(stringBuilder);
        return strValue;
    }

    public static string Parse(Color[] value)
    {
        StringBuilder stringBuilder = ObjectPool<StringBuilder>.Instance.Allot();
        stringBuilder.Length = 0;
        int endIndex = value.Length - 1;
        for (int i = 0; i < endIndex; ++i)
        {
            stringBuilder.Append(Parse(value[i]));
            stringBuilder.Append(Table.splitArray);
        }
        stringBuilder.Append(Parse(value[endIndex]));

        string strValue = stringBuilder.ToString();
        ObjectPool<StringBuilder>.Instance.Recycle(stringBuilder);
        return strValue;
    }

    #endregion

}