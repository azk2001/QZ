using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine;

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
    public static void BuilderFileUTF8(string filePath, string savePath, string namespaceName)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("filepath not exists : " + filePath);
            return;
        }

        string jsonText = File.ReadAllText(filePath, Encoding.UTF8);
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string text = Builder(jsonText, fileName, namespaceName);
        File.WriteAllText(savePath + "/" + fileName + ".cs", text, Encoding.UTF8);
    }

    /// <summary>
    /// 生成配置表类的json字符串
    /// 生成的配置表若要添加方法, 建议添加扩展, 以方便之后更新
    /// </summary>
    /// <param name="jsonText">配置表字符串</param>
    /// <param name="fileName">配置表名</param>
    /// <returns></returns>
    public static string Builder(string jsonText, string fileName, string namespaceName)
    {
        if (string.IsNullOrEmpty(jsonText))
            return string.Empty;

        string[] txtList = jsonText.Replace("\r","").Split(("\n").ToCharArray());

        if (txtList.Length < 2)
        {
            Debug.LogError("文件夹里面必须添加测试数据");
            return "";
        }

        string[] keysList = txtList[0].Split('\t');
        string[] valueList = txtList[1].Split('\t');

        StringBuilder cfgInfo = new StringBuilder();

        cfgInfo.AppendLine("using System;");
        cfgInfo.AppendLine("using System.Collections.Generic;");
        cfgInfo.AppendLine("using UnityEngine;\r\n");

        cfgInfo.AppendLine("namespace " + namespaceName);
        cfgInfo.AppendLine("{");

        cfgInfo.AppendLine("    public class " + fileName);
        cfgInfo.AppendLine("    {");

        string keyType = GetVariableType(valueList[0]);

        cfgInfo.AppendLine("        private static readonly Dictionary<" + keyType + ", " + fileName + "> gInfoDic = new Dictionary<"+ keyType + ", " + fileName + ">();");
        cfgInfo.AppendLine("        private static readonly List<" + fileName + "> gInfoList = new List<" + fileName + ">();\r\n");

        //添加变量;
        for (int i = 0, max = keysList.Length; i < max; ++i)
        {
            cfgInfo.AppendLine("        public readonly " + GetVariableType(valueList[i]) + " " + keysList[i] + ";");
        }

        cfgInfo.AppendLine("\r\n");

        //添加构造方法;
        cfgInfo.AppendLine("        private " + fileName + "( tab_file_data.line_data file)");
        cfgInfo.AppendLine("        {");
        for (int i = 0, max = keysList.Length; i < max; ++i)
        {
            string str = GetVariableType(valueList[i]);
            switch (str)
            {
                case "int":
                    cfgInfo.AppendLine("            " + keysList[i] + "= file.get_content_int(\"" + keysList[i] + "\");");
                    break;
                case "float":
                    cfgInfo.AppendLine("            " + keysList[i] + "= file.get_content_float(\"" + keysList[i] + "\");");
                    break;
                case "string":
                    cfgInfo.AppendLine("            " + keysList[i] + "= file.get_content_str(\"" + keysList[i] + "\");");
                    break;
            }

        }
        cfgInfo.AppendLine("        }");

        cfgInfo.AppendLine("\r\n");

        //添加获取方法;
        cfgInfo.AppendLine("        public static " + fileName + " Get("+ keyType + " " + keysList[0] + ")");
        cfgInfo.AppendLine("        {");
        cfgInfo.AppendLine("            if (gInfoDic.ContainsKey(" + keysList[0] + "))");
        cfgInfo.AppendLine("                return gInfoDic[" + keysList[0] + "];");
        cfgInfo.AppendLine("            MyDebug.LogError(\"" + fileName + " 未能找到id: \" +" + keysList[0] + ".ToString());");
        cfgInfo.AppendLine("            return null;");
        cfgInfo.AppendLine("        }");

        cfgInfo.AppendLine("\r\n");

        //添加获取方法;
        cfgInfo.AppendLine("        public static List<"+fileName+"> GetList()");
        cfgInfo.AppendLine("        {");
        cfgInfo.AppendLine("            return gInfoList;");
        cfgInfo.AppendLine("        }");

        cfgInfo.AppendLine("\r\n");

        //添加获取方法;
        cfgInfo.AppendLine("        public static void LoadTxt(tab_file_data file)");
        cfgInfo.AppendLine("        {");
        cfgInfo.AppendLine("            List<tab_file_data.line_data> list = file.get_line_data();");
        cfgInfo.AppendLine("            for (int i = 0, max = list.Count; i < max; i++)");
        cfgInfo.AppendLine("            {");
        cfgInfo.AppendLine("                tab_file_data.line_data data = list[i];");

        switch (keyType)
        {
            case "int":
                cfgInfo.AppendLine("                " + keyType + " id = data.get_content_int(\"" + keysList[0] + "\");");
                break;
            case "float":
                cfgInfo.AppendLine("                " + keyType + " id = data.get_content_float(\"" + keysList[0] + "\");");
                break;
            case "string":
                cfgInfo.AppendLine("                " + keyType + " id = data.get_content_str(\"" + keysList[0] + "\");");
                break;
        }
       
        cfgInfo.AppendLine("                if (gInfoDic.ContainsKey(id))");
        cfgInfo.AppendLine("                {");
        cfgInfo.AppendLine("                     MyDebug.LogError(\""+fileName+ "配置表id重复:\"+id);");
        cfgInfo.AppendLine("                }");
        cfgInfo.AppendLine("                else");
        cfgInfo.AppendLine("                {");
        cfgInfo.AppendLine("                    " + fileName + " info = new " + fileName + "(data);");
        cfgInfo.AppendLine("                    gInfoDic.Add(id,info);");
        cfgInfo.AppendLine("                    gInfoList.Add(info);");
        cfgInfo.AppendLine("                }");
        cfgInfo.AppendLine("            }");
        cfgInfo.AppendLine("        }"); 
        cfgInfo.AppendLine("    }");
        cfgInfo.AppendLine("}");

        return cfgInfo.ToString(); 
    }

    private static string GetVariableType(string variable)
    {

        int mInt = 0;
        float mFloat = 0;

        if (int.TryParse(variable, out mInt))
        {
            return "int";
        }

        if (float.TryParse(variable, out mFloat))
        {
            return "float";
        }

        return "string";
    }
}
