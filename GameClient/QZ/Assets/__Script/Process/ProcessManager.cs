using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum ProcessType
{
    processstart,//开始流程
}

class ProcessManager : SingleClass<ProcessManager>
{
    private Dictionary<ProcessType, ProcessBase> procDic = new Dictionary<ProcessType, ProcessBase>();

    public ProcessBase curProcess = null; //当前所在的流程;

    public void Init()
    {
        ProcessStart processStart = new ProcessStart();
        AddProcess(ProcessType.processstart, processStart);
    }

    public void AddProcess(ProcessType type, ProcessBase pb)
    {
        if (procDic.ContainsKey(type) == false)
        {
            procDic[type] = pb;
        }
    }

    public void RemoveProcess(ProcessType type)
    {
        if (procDic.ContainsKey(type))
        {
            procDic.Remove(type);
        }
    }
    public ProcessBase GetProcess(ProcessType type)
    {
        if (procDic.ContainsKey(type))
        {
            return procDic[type];
        }

        return null;
    }

    //开始一个流程
    public void Begin(ProcessType type)
    {
        //结束当前流程;
        if(curProcess != null)
        {
            curProcess.OnEnd();
        }

        //运行下一个流程;
        curProcess = GetProcess(type);
        curProcess.OnBegin();
    }
}