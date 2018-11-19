using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TabFileData
{
    public bool ContainLine(string key)
    {
        return data.ContainsKey(key);
    }

    public int GetContentInt(string key, string colName)
    {
        if (!data.ContainsKey(key))
        {
            MyDebug.WriteLine(string.Format("parse failed! key:{0}", key));
            return 0;
        }

        string str = data[key].GetContentStr(colName);

        int res = 0;

        if (!str.TryToInt(out res))
        {
            MyDebug.WriteLine(string.Format("parse failed! key:{0} col:{1}", key, colName));
        }

        return res;
    }

    public float GetContentFloat(string key, string colName)
    {
        if (!data.ContainsKey(key))
        {
            MyDebug.WriteLine(string.Format("parse failed! key:{0}", key));
            return 0;
        }

        string str = data[key].GetContentStr(colName);

        float res = 0;

        if (!str.TryToFloat(out res))
        {
            MyDebug.WriteLine(string.Format("parse failed! key:{0} col:{1}", key, colName));
        }

        return res;
    }


    public string GetContentStr(string key, string colName)
    {
        if (!data.ContainsKey(key))
        {
            MyDebug.WriteLine(string.Format("parse failed! key:{0}", key));
            return string.Empty;
        }

        string str = data[key].GetContentStr(colName);

        return str;
    }

    public List<TabFileData.LineData> GetLineData()
    {
        return linedataList;
    }

    public class LineData
    {
        public LineData(string[] colName, string[] lineData)
        {
            for (int i = 0; i < colName.Length; i++)
            {
                try
                {
                    data.Add(colName[i], lineData[i]);
                }
                catch
                {
                    MyDebug.WriteLine("列名重复：" + colName[i]);
                }
            }
        }

        public string GetContentStr(string colName)
        {
            if (!data.ContainsKey(colName))
            {
                MyDebug.WriteLine(string.Format("parse failed! col:{0}", colName));
                return string.Empty;
            }

            return data[colName];
        }

        public int GetContentInt(string colName)
        {
            if (!data.ContainsKey(colName))
            {
                MyDebug.WriteLine(string.Format("parse failed! col:{0}", colName));
                return 0;
            }

            int res = 0;

            if (!data[colName].TryToInt(out res))
            {
                MyDebug.WriteLine(string.Format("not TryToInt! col:{0}", colName));
            }

            return res;
        }

        public int GetContentInt(string colName, int defaultValue)
        {
            if (!data.ContainsKey(colName))
            {
                MyDebug.WriteLine(string.Format("parse failed! col:{0}", colName));
                return 0;
            }

            int res = 0;

            if (!data[colName].TryToInt(out res))
            {
                MyDebug.WriteLine(string.Format("not TryToInt! col:{0}", colName));
                res = defaultValue;
            }

            return res;
        }
        public int[] GetIntArr(string colName)
        {
            List<int> tempData = new List<int>();
            var itor = data.Keys.GetEnumerator();
            while (itor.MoveNext())
            {
                if (itor.Current != null && itor.Current.StartsWith(colName))
                {
                    int value = GetContentInt(itor.Current, int.MinValue);
                    if (value != int.MinValue)
                    {
                        tempData.Add(value);
                    }
                }
            }
            itor.Dispose();
            return tempData.ToArray();
        }
        public float GetContentFloat(string colName)
        {
            if (!data.ContainsKey(colName))
            {
                MyDebug.WriteLine(string.Format("parse failed! col:{0}", colName));
                return 0.0f;
            }

            float res = 0;
            if (!data[colName].TryToFloat(out res))
            {
                MyDebug.WriteLine(string.Format("not TryToFloat! col:{0}", colName));
            }
            return (float)res;

        }

        public Dictionary<string, string> data = new Dictionary<string, string>();
    }

    // -----------------------------------------------------------

    public void AddLine(string[] lineData)
    {
        if (colName == null)
            return;

        LineData linedata = new LineData(colName, lineData);

        if (data.ContainsKey(lineData[0]))
        {
            MyDebug.WriteLine(string.Format("same key!  tab:{0}  name:{1}", tabName, lineData[0]));
        }
        try
        {
            data.Add(lineData[0], linedata);
        }
        catch
        {
            MyDebug.WriteLine("key not add:" + lineData[0]);
        }

        linedataList.Add(linedata);
    }

    public string tabName = "";

    public string[] colName = null;

    private Dictionary<string, LineData> data = new Dictionary<string, LineData>();
    private List<LineData> linedataList = new List<LineData>();
}

public static class TabFile
{
    public static TabFileData Parse(string txt, int igronLine = 2, string tabName = "")
    {
        TabFileData data = new TabFileData();

        data.tabName = tabName;

        string[] strLine = txt.Split('\n');

        string lineTemp = null;

        lineTemp = strLine[0].TrimEnd("\r".ToCharArray());
        data.colName = lineTemp.Split(("\t").ToCharArray());

        string[] lineSplit = null;

        for (int i = igronLine; i < strLine.Length; i++)
        {
            lineTemp = strLine[i].TrimEnd("\r".ToCharArray());

            if (lineTemp.Equals(""))
                continue;

            lineSplit = lineTemp.Split(("\t").ToCharArray());

            if (data.colName.Length != lineSplit.Length)
            {
                MyDebug.WriteLine("invalid data! line_data::line_data");
            }
            else
            {
                data.AddLine(lineSplit);
            }
        }
        
        return data;
    }
}
