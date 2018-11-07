using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ProcessManager:SingleClass<ProcessManager>
{
    private Dictionary<string, ProcessBase> procDic = new Dictionary<string, ProcessBase>();

    public void Init()
    {

    }

    public ProcessBase GetProcess(string str)
    {
        if (procDic.ContainsKey(str))
        {
            return procDic[str];
        }

        return null;
    }



}