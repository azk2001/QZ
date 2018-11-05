using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEditor;

public static class TableBuilder
{
    /// <summary>
    /// 参考一个txt文件...生成一个cs文件到指定路径下
    /// </summary>
    /// <param name="filePath">参考的txt文件的全路径</param>
    /// <param name="savePath">保存的路径(不包含文件名)</param>
    /// <param name="isAuto">是否自动生成</param>
    /// <param name="isDic">是否生成字典配置表类</param>
    /// <param name="isDeserialize">是否支持反序列化</param>
    /// <param name="namespaceName">使用的空间名</param>
    public static void BuilderFileUTF8(string filePath, string savePath, bool isAuto, bool isDic, bool isDeserialize, string namespaceName)
    {
        if (!File.Exists(filePath))
        {
            Debuger.LogError("filepath not exists : " + filePath);
            return;
        }

        string jsonText = File.ReadAllText(filePath, Encoding.UTF8);
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string text = Builder(jsonText, fileName, isAuto, isDic, isDeserialize, namespaceName);
        File.WriteAllText(savePath + "/" + fileName + ".cs", text, Encoding.UTF8);
    }

    /// <summary>
    /// 生成配置表类的json字符串
    /// 生成的配置表若要添加方法, 建议添加扩展, 以方便之后更新
    /// </summary>
    /// <param name="jsonText">配置表字符串</param>
    /// <param name="fileName">配置表名</param>
    /// <param name="isDic">字典或列表</param>
    /// <param name="isDeserialize">是否反序列化配置表字符串方法(建议基本类型可以使用, 否则消耗极大)</param>
    /// <returns></returns>
    public static string Builder(string jsonText, string fileName, bool isAuto, bool isDic, bool isDeserialize, string namespaceName)
    {
        if (string.IsNullOrEmpty(jsonText))
            return string.Empty;

        //对，每种类型进行解析的方法
        //此处添加成员需要注意以下2点
        //1.Table类中添加具体的方法
        //2.本类的ParseObject方法中添加对应的解析方案
        Dictionary<string, string> typeToMethod = TableBuilderCfg.instance.typeToMethod;

        //空间名
        string[][] data = Table.GetArrar(jsonText);

        StringBuilder cfgInfo = new StringBuilder();

        cfgInfo.AppendLine("using System;");
        cfgInfo.AppendLine("using System.Collections.Generic;");
        cfgInfo.AppendLine("using MXEngine;");
        cfgInfo.AppendLine("using UnityEngine;\r\n");

        cfgInfo.AppendLine("namespace " + namespaceName);
        cfgInfo.AppendLine("{");

        cfgInfo.AppendLine("    public class " + fileName);
        cfgInfo.AppendLine("    {");

        string dataName = isDic ? "gInfoDic" : "gInfoList";
        string[] colNames = data[0];
        string[] colNameTypes = isAuto ? GetColNameTypes(data) : (data[1]);

        //大小写约定
        //RefreshColNames(colNames);
        //RefreshColNameTypes(colNameTypes);

        int colNameMaxCount = GetColTypeMaxCount(colNameTypes);

        //写入方法
        string[] keys = GetKeys(data, colNameTypes, isAuto);
        bool isIntKey = IsIntKeyType(colNameTypes, keys.Length);
        string keyType = isIntKey ? "int" : "string";               //组合类型必须是int或string, 无法生成
        int endIndex;

        //写入成员----------------------------------------------------------------
        for (int i = 0, max = colNames.Length; i < max; ++i)
        {
            string colNameType = colNameTypes[i];
            cfgInfo.Append("        public ");
            cfgInfo.Append(colNameType);
            cfgInfo.Append(' ', colNameMaxCount - colNameType.Length + 1);
            cfgInfo.Append(colNames[i]);

            //是否写入私有, 取决于key(此处尚未完成)
            if (isDic && keys.Length > i)
                cfgInfo.AppendLine(" { get; private set; }");
            else
                cfgInfo.AppendLine(";");
        }
        cfgInfo.AppendLine("");


        //静态方法
        if (isDic)
        {
            endIndex = keys.Length - 1;
            string keyName = colNames[0];

            //复杂数据类型
            string keysData = "";
            if (keys.Length > 1)
            {
                keyName = "_dicKey";
                keysData += "            " + keyType + " _dicKey = Table.GetDicKey(";
                for (int i = 0; i < endIndex; ++i)
                    keysData += colNames[i] + ", ";
                keysData += colNames[endIndex];
                keysData += ");";
            }

            //参数
            string parsName = "";
            for (int i = 0; i < endIndex; ++i)
                parsName += colNameTypes[i] + " " + colNames[i] + ", ";
            parsName += colNameTypes[endIndex] + " " + colNames[endIndex];

            //1.写入ContainsThis方法
            cfgInfo.AppendLine("        public static bool ContainsThis(" + parsName + ")");
            cfgInfo.AppendLine("        {");
            if (keys.Length > 1)
                cfgInfo.AppendLine(keysData);
            cfgInfo.AppendLine("            return " + dataName + ".ContainsKey(" + keyName + ");");
            cfgInfo.AppendLine("        }\r\n");

            //2.写入GetThis方法
            cfgInfo.AppendLine("        public static " + fileName + " GetThis(" + parsName + ")");
            cfgInfo.AppendLine("        {");
            if (keys.Length > 1)
                cfgInfo.AppendLine(keysData);
            cfgInfo.AppendLine("            " + fileName + " cfg = " + dataName + ".TryGetValue(" + keyName + ");");
            cfgInfo.AppendLine("            if (cfg == null)");
            cfgInfo.AppendLine("                Debuger.LogError(\"" + fileName + " table found no " + keyName + ": \" + " + keyName + ");");
            cfgInfo.AppendLine("            return cfg;");
            cfgInfo.AppendLine("        }\r\n");

            if (isDeserialize)
            {
                //3.写入TryAddThis方法
                cfgInfo.AppendLine("        public static bool AddThis(" + parsName + ", " + fileName + " p)");
                cfgInfo.AppendLine("        {");
                if (keys.Length > 1)
                    cfgInfo.AppendLine(keysData);
                cfgInfo.AppendLine("            if (" + dataName + ".ContainsKey(" + keyName + "))");
                cfgInfo.AppendLine("            {");
                cfgInfo.AppendLine("                Debuger.LogError(\"" + fileName + " table key repeat, " + keyName + ": \" + " + keyName + ");");
                cfgInfo.AppendLine("                return false;");
                cfgInfo.AppendLine("            }");
                //cfgInfo.AppendLine("            " + dataName + ".Add(p.key = key, p);");

                for (int i = 0, max = keys.Length; i < max; ++i)
                    cfgInfo.AppendLine("            p." + colNames[i] + " = " + colNames[i] + ";");

                //写入参数
                cfgInfo.AppendLine("            " + dataName + ".Add(" + keyName + ", p);");
                cfgInfo.AppendLine("            return true;");
                cfgInfo.AppendLine("        }\r\n");

                //4.写入TryRemoveThis方法
                cfgInfo.AppendLine("        public static bool RemoveThis(" + parsName + ")");
                cfgInfo.AppendLine("        {");
                if (keys.Length > 1)
                    cfgInfo.AppendLine(keysData);
                cfgInfo.AppendLine("            if (!" + dataName + ".ContainsKey(" + keyName + "))");
                cfgInfo.AppendLine("            {");
                cfgInfo.AppendLine("                Debuger.LogError(\"" + fileName + " table found no key, " + keyName + ": \" + " + keyName + ");");
                cfgInfo.AppendLine("                return false;");
                cfgInfo.AppendLine("            }");
                cfgInfo.AppendLine("            " + dataName + ".Remove(" + keyName + ");");
                cfgInfo.AppendLine("            return true;");
                cfgInfo.AppendLine("        }\r\n");
            }
        }
        else
        {
            //List不需要写入GetThis; 此处无视
            cfgInfo.AppendLine("        public static bool ContainsThis(int uIndex)");
            cfgInfo.AppendLine("        {");
            cfgInfo.AppendLine("            return uIndex < " + dataName + ".Count;");
            cfgInfo.AppendLine("        }\r\n");
            cfgInfo.AppendLine("        public static " + fileName + " GetThis(int uIndex)");
            cfgInfo.AppendLine("        {");
            cfgInfo.AppendLine("            if (uIndex < " + dataName + ".Count)");
            cfgInfo.AppendLine("                return " + dataName + "[uIndex];");
            cfgInfo.AppendLine("            Debuger.LogError(\"" + fileName + " table found no key, uIndex: \" + uIndex);");
            cfgInfo.AppendLine("            return null;");
            cfgInfo.AppendLine("        }\r\n");
        }

        //静态成员
        if (isDic)
        {
            cfgInfo.AppendLine("        public static readonly Dictionary<" + keyType + ", " + fileName + "> " + dataName + " = new Dictionary<" + keyType + ", " + fileName + ">();\r\n");
        }
        else
        {
            cfgInfo.AppendLine("        public static readonly List<" + fileName + "> " + dataName + " = new List<" + fileName + ">();\r\n");
        }

        //序列化
        cfgInfo.AppendLine("        public static void Serialize(Table table)");
        cfgInfo.AppendLine("        {");

        //写入解析列索引
        for (int i = 0, max = colNames.Length; i < max; ++i)
            cfgInfo.AppendLine("            int " + colNames[i] + "Index = table.TryGetColIndex(\"" + colNames[i] + "\");");

        string beginIndex = isAuto ? "1" : "2";
        cfgInfo.AppendLine("            for (int i = " + beginIndex + ", rowHeight = table.rowHeight; i < rowHeight; ++i)");
        cfgInfo.AppendLine("            {");
        cfgInfo.AppendLine("                " + fileName + " p = new " + fileName + "();");

        //typeToMethod
        for (int i = 0, max = colNames.Length; i < max; ++i)
        {
            if (typeToMethod.ContainsKey(colNameTypes[i]))
            {
                string methodString = typeToMethod[colNameTypes[i]];
                cfgInfo.AppendLine("                p." + colNames[i] + " = table." + methodString + "(i, " + colNames[i] + "Index);");
            }
            else if (!isAuto)
            {
                string methodString = TableBuilderCfg.instance.funcCustomParse;
                //cfgInfo.AppendLine("                p." + colNames[i] + " = table." + methodString + "(i, " + colNames[i] + "Index) as " + colNameTypes[i] + ";");
                cfgInfo.AppendLine("                table." + methodString + "(i, " + colNames[i] + "Index, out p." + colNames[i] + ");");
            }
            else
            {
                Debuger.LogError("没有找到对应的类型: " + colNameTypes[i]);
                cfgInfo.AppendLine("                p." + colNames[i] + " = table." + "未能识别" + "(i, " + colNames[i] + "Index);");
            }
        }

        if (isDic)
        {
            string keyName;
            if (keys.Length > 1)
            {
                endIndex = keys.Length - 1;
                keyName = "_dicKey";
                cfgInfo.Append("                " + keyType + " _dicKey = Table.GetDicKey(");
                for (int i = 0; i < endIndex; ++i)
                    cfgInfo.Append("p." + colNames[i] + ", ");
                cfgInfo.Append("p." + colNames[endIndex]);
                cfgInfo.AppendLine(");");
            }
            else
                keyName = "p." + colNames[0];

            cfgInfo.AppendLine("                if (" + dataName + ".ContainsKey(" + keyName + "))");
            cfgInfo.AppendLine("                    Debuger.LogError(\"" + fileName + " table key repeat, " + keyName + ": \" + " + keyName + ");");
            cfgInfo.AppendLine("                else");
            cfgInfo.AppendLine("                    " + dataName + ".Add(" + keyName + ", p);");
        }
        else
        {
            cfgInfo.AppendLine("                " + dataName + ".Add(p);");
        }
        cfgInfo.AppendLine("            }");
        cfgInfo.AppendLine("        }\r\n");

        if (isDeserialize)
        {
            cfgInfo.AppendLine("        public override string ToString()");
            cfgInfo.Append("        { return string.Concat(");
            endIndex = colNames.Length - 1;
            for (int i = 0; i < endIndex; ++i)
            {
                cfgInfo.Append("Table.Parse(" + colNames[i] + "), '\\t', ");
            }
            cfgInfo.AppendLine("Table.Parse(" + colNames[endIndex] + ")); }\r\n");

            cfgInfo.AppendLine("        public static string Deserialize()");
            cfgInfo.Append("        { return Table.Deserialize(" + dataName + ", \"");
            cfgInfo.Append(string.Join("\\t", colNames));
            cfgInfo.Append("\"");
            if (!isAuto)
            {
                cfgInfo.Append(", \"");
                cfgInfo.Append(string.Join("\\t", colNameTypes));
                cfgInfo.Append("\"");
            }
            cfgInfo.Append("); }\r\n");


            //                endIndex = colNames.Length - 1;
            //                for (int i = 0; i < endIndex; ++i)
            //                    cfgInfo.Append(colNames[i] + "\\t");
            //                cfgInfo.Append(colNames[endIndex] + "\"");
            //
            //                if (!isAuto)
            //                {
            //                    cfgInfo.Append(string.Join("\t", colNameTypes));
            //                    cfgInfo.Append(", ");
            //                }
            //                cfgInfo.Append(dataName + "); }\r\n");
            //cfgInfo.Append(colNames[endIndex] + "\", " + dataName + "); }\r\n");
        }

        //反序列化


        cfgInfo.AppendLine("    }");
        cfgInfo.AppendLine("}");

        return cfgInfo.ToString();
    }


    static void RefreshColNames(string[] colNames)
    {
        for (int i = colNames.Length - 1; i >= 0; --i)
        {
            string value = colNames[i];
            colNames[i] = Char.ToLower(value[0]) + value.Substring(1);
        }
    }

    static void RefreshColNameTypes(string[] colNameTypes)
    {
        for (int i = colNameTypes.Length - 1; i >= 0; --i)
        {
            string value = colNameTypes[i];

            //不在范围内的尝试转
            if (!TableBuilderCfg.instance.typeToMethod.ContainsKey(value))
                colNameTypes[i] = Char.ToUpper(value[0]) + value.Substring(1);
        }
    }
    //		string[] colNames = data[0];
    //		string[] colNameTypes = isAuto ? GetColNameTypes(data) : (data[1]);

    static bool IsIntKeyType(string[] types, int len)
    {
        for (int i = 0; i < len; ++i)
        {
            if (!string.Equals(types[i], "int"))
                return false;
        }
        return true;
    }

    //通过一个类型可以获取方法
    //通过一个类型可以获取
    static string ParseKey(string[] keys, string[] types, int len)
    {
        //如果全是int 否则视为string
        if (IsIntKeyType(types, len))
            return ParseKeyInt(keys, len);
        else
            return ParseKeyString(keys, len);
    }

    //int 最多3位 超出容易出错
    public static string ParseKeyInt(string[] keys, int len)
    {
        if (len > 3)
            Debuger.LogError("int key 最多3位 超出容易出错");

        switch (len)
        {
            case 1:
                return keys[0];
            case 2:
                return Table.GetDicKey(int.Parse(keys[0]), int.Parse(keys[1])).ToString();
            case 3:
                return Table.GetDicKey(int.Parse(keys[0]), int.Parse(keys[1]), int.Parse(keys[2])).ToString();
            default:
                {
                    return ParseKeyString(keys, len);
                }
        }
    }

    public static string ParseKeyString(string[] keys, int len)
    {
        StringBuilder sb = new StringBuilder();
        int endIndex = len - 1;
        for (int i = 0; i < len; ++i)
        {
            sb.Append(keys[i]);
            if (i < endIndex)
                sb.Append(Table.splitStringKey);
        }
        return sb.ToString();
    }

    //获取key列表(本身数据, 数据类型)
    static string[] GetKeys(string[][] data, string[] types, bool isAuto)
    {
        string[] colNames = data[0];
        string[] colData = new string[data.Length];

        //映射类型
        types = types.Clone() as string[];
        for (int i = 0, max = types.Length; i < max; ++i)
            types[i] = TableBuilderCfg.instance.mTypeMap.TryGetValue(types[i], types[i]);

        //递归查找是否重复
        for (int x = 0, max = colNames.Length; x < max; ++x)
        {
            switch (types[x])
            {
                //key只允许为int或string
                //需要添加新的类型, 需要在这里添加
                case "int":
                case "string":
                    {
                        int endValue = isAuto ? 0 : 1;
                        for (int y = colData.Length - 1; y > endValue; --y)
                        {
                            if (string.IsNullOrEmpty(colData[x]))
                                colData[y] = data[y][x];
                            else
                                colData[y] = ParseKey(data[y], types, x + 1);
                        }

                        if (!IsRepeat(colData, isAuto))
                        {
                            string[] keys = new string[x + 1];
                            while (x >= 0)
                            {
                                keys[x] = colNames[x];
                                --x;
                            }
                            return keys;
                        }
                        break;
                    }
                default:
                    {
                        Debuger.LogError("没办法解析key的类型: " + types[x]);

                        if (x > 0)
                        {
                            string[] keys = new string[x--];
                            while (x >= 0)
                            {
                                keys[x] = colNames[x];
                                --x;
                            }
                            return keys;
                        }
                        break;
                    }

            }
        }

        //数据异常
        Debuger.LogError("配置表key类型解析异常");
        string[] keys1 = new string[colNames.Length];
        for (int i = 0, max = keys1.Length; i < max; ++i)
            keys1[i] = colNames[i];
        return keys1;
        //return new string[] { data[0][0] };
    }

    static bool IsRepeat(string[] keys, bool isAuto)
    {
        int endValue = isAuto ? 0 : 1;
        //递归查找是否重复
        for (int i = keys.Length - 1; i > endValue; --i)
        {
            string value = keys[i];
            for (int j = i - 1; j >= 0; --j)
            {
                if (string.Equals(value, keys[j]))
                    return true;
            }
        }
        return false;
    }


    //获取最大长度的字符串长度
    static int GetColTypeMaxCount(string[] values)
    {
        int count = 0;

        for (int i = values.Length - 1; i >= 0; --i)
        {
            if (values[i].Length > count)
                count = values[i].Length;
        }
        return count;
    }

    //获取配置表列 所有列的类型
    static string[] GetColNameTypes(string[][] data)
    {
        string[] colNameTypes = new string[data[0].Length];
        string colNameType;
        //数组
        for (int x = data[0].Length - 1; x >= 0; --x)
        {
            colNameType = null;
            for (int y = data.Length - 1; y > 0; --y)
            {
                string value = data[y][x];
                colNameType = GetTypeString(value);

                if (!string.IsNullOrEmpty(colNameType))
                {
                    //类型验证
                    for (int i = y - 1; i > 0; --i)
                    {
                        value = data[i][x];
                        string temp = GetTypeString(value);

                        if (!string.IsNullOrEmpty(temp))
                        {
                            //数据类型不匹配
                            if (!string.Equals(temp, colNameType))
                            {
                                //如果是数组类型的转换
                                if (temp.Contains(colNameType))
                                    colNameType = temp;
                                else if (colNameType.Contains(temp))
                                {
                                }
                                else
                                {
                                    //如果是int或float之间的转换优先float然后 计算通过
                                    string intOrFloatDesc = IntToFloatDesc(temp, colNameType);

                                    if (string.IsNullOrEmpty(intOrFloatDesc))
                                    {
                                        Debuger.LogError("数据类型不匹配temp: " + temp + ", colNameType: " + colNameType);
                                        colNameType = "string";
                                        i = 0;
                                    }
                                    else
                                        colNameType = intOrFloatDesc;
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(colNameType))
                {
                    if (!colNameType.Contains("[]"))
                    {
                        //如果不是数组, 检查该列是否有数组
                        for (int i = y - 1; i > 0; --i)
                        {
                            value = data[i][x];
                            string[] values = value.Split(Table.splitArray);
                            if (values.Length > 1)
                            {
                                colNameType += "[]";
                                break;
                            }
                        }
                    }
                    break;
                }
            }

            //默认字符串类型
            if (string.IsNullOrEmpty(colNameType))
                colNameTypes[x] = "string";
            else
                colNameTypes[x] = colNameType;
        }
        return colNameTypes;
    }

    //int和float之间的描述换算
    //返回空表示 换算失败
    static string IntToFloatDesc(string par1, string par2)
    {
        if (par1.Contains("int"))
        {
            if (par2.Contains("int"))
                return par2;
            else if (par2.Contains("float"))
                return par2;
        }
        else if (par1.Contains("float"))
        {
            if (par2.Contains("int"))
                return par1;
            else if (par2.Contains("float"))
                return par1;
        }

        return string.Empty;
    }

    //获取 某个数据的类型
    static string GetTypeString(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        string[] valueArray = value.Split(Table.splitArray);

        if (valueArray.Length > 1)
            value = valueArray[0];

        string[] valueObject = value.Split(Table.splitObject);

        value = valueObject.Length > 1 ? ParseObject(valueObject) :
            ParseObject(value);

        if (valueArray.Length > 1)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            value = value + "[]";
        }

        return value;
    }

    //基本类型解析
    static string ParseObject(string value)
    {
        if (value.IndexOf('.') > 0)
        {
            float f;
            if (float.TryParse(value, out f))
                return "float";
        }
        else
        {
            int i;
            if (int.TryParse(value, out i))
                return "int";
            else
            {
                bool b;
                if (bool.TryParse(value, out b))
                    return "bool";
            }
        }

        //默认类型
        return "string";
    }

    //复杂类型解析
    static string ParseObject(string[] value)
    {
        int valueLenght = value.Length;

        if (valueLenght < 2 || valueLenght > 4)
            return "string";

        for (int i = 0, max = value.Length; i < max; ++i)
        {
            float f;
            if (!float.TryParse(value[i], out f))
                return "string";
        }

        switch (value.Length)
        {
            case 2: return "Vector2";
            case 3: return "Vector3";
            case 4: return "Color";
        }

        //默认类型
        return "string";
    }
}
