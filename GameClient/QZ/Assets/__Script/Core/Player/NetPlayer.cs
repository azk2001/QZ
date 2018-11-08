﻿using System;
using System.Collections.Generic;

public class NetPlayer
{
    public BattleUnitData battleUnitData = new BattleUnitData();            //角色战斗属性;
    public PlayerBasicsData basicsData = new PlayerBasicsData();            //角色基础信息;
   
    public int uuid
    {
        get
        {
            return basicsData.roleData.uuid;
        }
        set
        {
            basicsData.roleData.uuid = value;
        }
    }

    //第一步初始化玩家数据
    public void SetBytes(BytesReader reader)
    {
        basicsData.SetBytes(reader);                //网络战斗数据初始化数据;
        battleUnitData.SetBytes(reader);            //网络基础数据初始化数据;
    }

    public BytesWriter GetBytes(BytesWriter writer)
    {
        writer = basicsData.GetBytes(writer);
        writer = battleUnitData.GetBytes(writer);

        return writer;
    }
}