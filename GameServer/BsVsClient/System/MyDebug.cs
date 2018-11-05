using System.Collections;
using System;

public class MyDebug
{

    public static bool isDebug = true;


    public static void WriteLine(object obj)
    {
        if (isDebug)
        {
            Console.WriteLine(obj);
        }
    }
    public static void WriteLine(string str, params object[] obj)
    {
        if (isDebug)
        {
            Console.WriteLine(str, obj);
        }
    }
}
