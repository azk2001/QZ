using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//自定义类型的扩展方法
//1.需要扩展Table
//2.需要重载FuncCustomParse
public static class FuncCustomParseTableExtensionMethods
{
    /// <summary>
    /// 对一个int扩展配置表解析方法的案例
    /// </summary>
    /// <param name="table">Table.</param>
    /// <param name="rowIndex">Row index.</param>
    /// <param name="colIndex">Col index.</param>
    /// <param name="value">局需要传递的变量类型与变量</param>
    public static void FuncCustomParse(this Table table, int rowIndex, int colIndex, out int value)
    {
        value = 0;
    }
}

//配置表生成, 配置类
public class TableBuilderCfg
{
    public static readonly TableBuilderCfg instance = new TableBuilderCfg();

    //非自动生成配置表, 默认的方法
    public string funcCustomParse = "FuncCustomParse";

    //类型映射
    public Dictionary<string, string> mTypeMap = new Dictionary<string, string>(){

            //扩展类型映射转换
            { "int", "int" },

            { "queueEvent", "int" },
        };

    //方法映射
    public Dictionary<string, string> typeToMethod = new Dictionary<string, string>() { 

            //固定的类型映射
            { "int", "TryGetInt" },
            { "string", "TryGetString" },
            { "float", "TryGetFloat" },
            { "bool", "TryGetBool" },
            { "Vector2", "TryGetVector2" },
            { "Vector3", "TryGetVector3" },
            { "Color", "TryGetColor" },

            { "int[]", "TryGetIntArray" },
            { "string[]", "TryGetStringArray" },
            { "float[]", "TryGetFloatArray" },
            { "bool[]", "TryGetBoolArray" },
            { "Vector2[]", "TryGetVector2Array" },
            { "Vector3[]", "TryGetVector3Array" },
            { "Color[]", "TryGetColorArray" },

            //扩展类型映射
        };
}