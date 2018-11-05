using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class tab_file_data
{
    public bool contain_line(string __key)
    {
        return _data.ContainsKey(__key);
    }

    public int get_content_int(string __key, string __col_name)
    {
        if (!_data.ContainsKey(__key))
        {
            MyDebug.WriteLine(string.Format("parse failed! key:{0}", __key));
            return 0;
        }

        string ___str = _data[__key].get_content_str(__col_name);

        int ___res = 0;

        if (!___str.TryToInt(out ___res))
        {
            MyDebug.WriteLine(string.Format("parse failed! key:{0} col:{1}", __key, __col_name));
        }

        return ___res;
    }

    public float get_content_float(string __key, string __col_name)
    {
        if (!_data.ContainsKey(__key))
        {
            MyDebug.WriteLine(string.Format("parse failed! key:{0}", __key));
            return 0;
        }

        string ___str = _data[__key].get_content_str(__col_name);

        double ___res = 0;

        if (!___str.TryToDouble(out ___res))
        {
            MyDebug.WriteLine(string.Format("parse failed! key:{0} col:{1}", __key, __col_name));
        }

        return (float)___res;
    }

    public string get_content_str(string __key, string __col_name)
    {
        if (!_data.ContainsKey(__key))
        {
            MyDebug.WriteLine(string.Format("parse failed! key:{0}", __key));
            return string.Empty;
        }

        string ___str = _data[__key].get_content_str(__col_name);

        return ___str;
    }

    public List<tab_file_data.line_data> get_line_data()
    {
        return _line_data;
    }

    public class line_data
    {
        public line_data(string[] __col_name, string[] __line_data)
        {
            for (int i = 0; i < __col_name.Length; i++)
            {
                try
                {
                    _data.Add(__col_name[i], __line_data[i]);
                }
                catch
                {
                    MyDebug.WriteLine("列名重复：" + __col_name[i]);
                }
            }
        }

        public string get_content_str(string __col_name)
        {
            if (!_data.ContainsKey(__col_name))
            {
                MyDebug.WriteLine(string.Format("parse failed! col:{0}", __col_name));
                return string.Empty;
            }

            return _data[__col_name];
        }

        public int get_content_int(string __col_name)
        {
            if (!_data.ContainsKey(__col_name))
            {
                MyDebug.WriteLine(string.Format("parse failed! col:{0}", __col_name));
                return 0;
            }

            int ___res = 0;

            if (!_data[__col_name].TryToInt(out ___res))
            {
                MyDebug.WriteLine(string.Format("not TryToInt! col:{0}", __col_name));
            }

            return ___res;
        }

        public int get_content_int(string __col_name, int defaultValue)
        {
            if (!_data.ContainsKey(__col_name))
            {
                MyDebug.WriteLine(string.Format("parse failed! col:{0}", __col_name));
                return 0;
            }

            int ___res = 0;

            if (!_data[__col_name].TryToInt(out ___res))
            {
                MyDebug.WriteLine(string.Format("not TryToInt! col:{0}", __col_name));
                ___res = defaultValue;
            }

            return ___res;
        }
        public int[] GetIntArr(string colName)
        {
            List<int> tempData = new List<int>();
            var itor = _data.Keys.GetEnumerator();
            while (itor.MoveNext())
            {
                if (itor.Current != null && itor.Current.StartsWith(colName))
                {
                    int value = get_content_int(itor.Current, int.MinValue);
                    if (value != int.MinValue)
                    {
                        tempData.Add(value);
                    }
                }
            }
            itor.Dispose();
            return tempData.ToArray();
        }
        public float get_content_float(string __col_name)
        {
            if (!_data.ContainsKey(__col_name))
            {
                MyDebug.WriteLine(string.Format("parse failed! col:{0}", __col_name));
                return 0.0f;
            }

            double ___res = 0;
            if (!_data[__col_name].TryToDouble(out ___res))
            {
                MyDebug.WriteLine(string.Format("not TryToFloat! col:{0}", __col_name));
            }
            return (float)___res;

        }

        public Dictionary<string, string> _data = new Dictionary<string, string>();
    }

    // -----------------------------------------------------------

    public void add_line(string[] __line_data)
    {
        if (_col_name == null)
            return;

        line_data ___line_data = new line_data(_col_name, __line_data);

        if (_data.ContainsKey(__line_data[0]))
        {
            MyDebug.WriteLine(string.Format("same key!  tab:{0}  name:{1}", _tab_name, __line_data[0]));
        }

        _data.Add(__line_data[0], ___line_data);
        _line_data.Add(___line_data);
    }

    public string _tab_name = "";

    public string[] _col_name = null;

    private Dictionary<string, line_data> _data = new Dictionary<string, line_data>();
    private List<line_data> _line_data = new List<line_data>();
}

public static class tab_file
{
    public static tab_file_data parse(string __txt, int __igron_line = 2, string __tab_name = "")
    {
        tab_file_data ___data = new tab_file_data();

        ___data._tab_name = __tab_name;

        string[] ___str_line = __txt.Split(("\n").ToCharArray());

        string ___line_temp = null;

        ___line_temp = ___str_line[0].TrimEnd("\r".ToCharArray());
        ___data._col_name = ___line_temp.Split(("\t").ToCharArray());

        string[] ___line_split = null;

        for (int i = __igron_line; i < ___str_line.Length; i++)
        {
            ___line_temp = ___str_line[i].TrimEnd("\r".ToCharArray());

            if (___line_temp.Equals(""))
                continue;

            ___line_split = ___line_temp.Split(("\t").ToCharArray());

            if (___data._col_name.Length != ___line_split.Length)
            {
                MyDebug.WriteLine("invalid data! line_data::line_data");
            }
            else
            {
                ___data.add_line(___line_split);
            }
        }
        
        return ___data;
    }
}
