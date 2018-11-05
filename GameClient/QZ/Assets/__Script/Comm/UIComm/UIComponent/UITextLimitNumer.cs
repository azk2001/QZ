using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UITextLimitNumer : MonoBehaviour
{
    public UIText showCurWords;
    public InputField input;
    [Header("汉字字限制")]
    public int CHARACTER_LIMIT = 10;

    private int charLimit;
    public SplitType m_SplitType = SplitType.UTF8;
    public int wordsNum;

    public enum SplitType
    {
        ASCII = 1,
        GB = 2,
        Unicode = 3,
        UTF8 = 4,
    }

    public void Check()
    {
        charLimit = 2 * CHARACTER_LIMIT + ReplaceText(input.text);

        input.text = GetSplitName((int)m_SplitType);
        if (showCurWords != null)
        {
            showCurWords.text = string.Format("{0}/{1}", wordsNum, CHARACTER_LIMIT);
        }
    }

    private int ReplaceText(string inputText)
    {
        int index = 0;
        string result = inputText;

        for (int i = 0; i < 10; i++)
        {
            int startSub = result.IndexOf("<quad");
            int endSub = result.IndexOf("/>");

            if (startSub < 0 || endSub < 0)
            {
                break;
            }
            string s = result.Substring(startSub, endSub);

            result = result.Replace(s, "");

            index += endSub + 2 - startSub;
        }

        return index;  //结果：大家好！我是张三，入职时间：2010-10-7，邮箱：zhangsan@163.com 自我介绍。
    }

    public string GetSplitName(int checkType)
    {
        wordsNum = 0;
        string temp = input.text.Substring(0, (input.text.Length < charLimit + 1) ? input.text.Length : charLimit + 1);
        if (checkType == (int)SplitType.ASCII)
        {
            return SplitNameByASCII(temp);
        }
        else if (checkType == (int)SplitType.GB)
        {
            return SplitNameByGB(temp);
        }
        else if (checkType == (int)SplitType.Unicode)
        {
            return SplitNameByUnicode(temp);
        }
        else if (checkType == (int)SplitType.UTF8)
        {
            return SplitNameByUTF8(temp);
        }

        return "";
    }

    //4、UTF8编码格式（汉字3byte，英文1byte）,//UTF8编码格式,目前是最常用的 
    private string SplitNameByUTF8(string temp)
    {
        string outputStr = "";
        int count = 0;

        for (int i = 0; i < temp.Length; i++)
        {
            string tempStr = temp.Substring(i, 1);
            //byte[] encodedBytes = System.Text.ASCIIEncoding.UTF8.GetBytes(tempStr);//Unicode用两个字节对字符进行编码
            //string output = "[" + temp + "]";
            //for (int byteIndex = 0; byteIndex < encodedBytes.Length; byteIndex++)
            //{
            //    output += Convert.ToString((int)encodedBytes[byteIndex], 2) + "  ";//二进制
            //}
            // Debug.Log(output);

            int byteCount = System.Text.ASCIIEncoding.UTF8.GetByteCount(tempStr);
            Debug.Log("字节数=" + byteCount);

            if (byteCount > 1)
            {
                count += 2;
            }
            else
            {
                count += 1;
            }
            if (count <= charLimit)
            {
                outputStr += tempStr;
            }
            else
            {
                break;
            }
        }
        wordsNum = count / 2 + (count % 2 == 0 ? 0 : 1);
        return outputStr;
    }

    private string SplitNameByUnicode(string temp)
    {
        string outputStr = "";
        int count = 0;

        for (int i = 0; i < temp.Length; i++)
        {
            string tempStr = temp.Substring(i, 1);
            byte[] encodedBytes = System.Text.ASCIIEncoding.Unicode.GetBytes(tempStr);//Unicode用两个字节对字符进行编码
            if (encodedBytes.Length == 2)
            {
                int byteValue = (int)encodedBytes[1];
                if (byteValue == 0)//这里是单个字节
                {
                    count += 1;
                }
                else
                {
                    count += 2;
                }
            }
            if (count <= charLimit)
            {
                outputStr += tempStr;
            }
            else
            {
                break;
            }
        }
        wordsNum = count / 2 + (count % 2 == 0 ? 0 : 1);
        return outputStr;
    }

    private string SplitNameByGB(string temp)
    {
        string outputStr = "";
        int count = 0;

        for (int i = 0; i < temp.Length; i++)
        {
            string tempStr = temp.Substring(i, 1);
            byte[] encodedBytes = System.Text.ASCIIEncoding.Default.GetBytes(tempStr);
            if (encodedBytes.Length == 1)
            {
                //单字节
                count += 1;
            }
            else
            {
                //双字节
                count += 2;
            }

            if (count <= charLimit)
            {
                outputStr += tempStr;
            }
            else
            {
                break;
            }
        }
        wordsNum = count / 2 + (count % 2 == 0 ? 0 : 1);
        return outputStr;
    }

    private string SplitNameByASCII(string temp)
    {
        byte[] encodedBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(temp);

        string outputStr = "";
        int count = 0;

        for (int i = 0; i < temp.Length; i++)
        {
            if ((int)encodedBytes[i] == 63)//双字节
                count += 2;
            else
                count += 1;

            if (count <= charLimit)
                outputStr += temp.Substring(i, 1);
            else if (count > charLimit)
                break;
        }

        if (count <= charLimit)
        {
            outputStr = temp;

        }
        wordsNum = count / 2 + (count % 2 == 0 ? 0 : 1);
        return outputStr;
    }

}
