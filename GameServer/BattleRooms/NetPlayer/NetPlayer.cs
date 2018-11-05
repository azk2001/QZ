using System;
using System.Collections.Generic;

namespace BattleServer
{
    public class NetPlayer
    {
        public BattleUnitData battleUnitData = new BattleUnitData();            //角色战斗属性;
        public PlayerBasicsData basicsData = new PlayerBasicsData();            //角色基础信息;
        public GameUnit gameUnit;                                               //初始化的游戏单位;

        private ePlayerRoomState _roomState = ePlayerRoomState.inRoom;  //角色游戏单位状态;
        public ePlayerRoomState roomState
        {
            get
            {
                return _roomState;
            }
        }

        private int _roomIndex;   //房间ID
        public int roomIndex
        {
            get
            {
                return _roomIndex;
            }
            set
            {
                _roomIndex = value;
            }
        }

        private int _uuid;
        public int uuid
        {
            get
            {
                return basicsData.roleData.uuid;
            }
            set
            {
                _uuid = value;
                basicsData.roleData.uuid = value;
            }
        }

        //第一步初始化玩家数据
        public void InitData(BytesReader reader)
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

        //设置角色当前状态
        public void SetRoomState(ePlayerRoomState state)
        {
            //如果是离线数据玩家都跳过其他的检查
            _roomState = state;
        }
    }
}
