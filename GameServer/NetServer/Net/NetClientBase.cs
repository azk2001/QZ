using System;
using System.Net.Sockets;

namespace GameServer
{
    public class NetClientBase
    {
        public enum eResult
        {
            userHold,       //持续连接中
            userDelay,      //用户延迟
            userResponse,   //用户响应
            userBreak,		//用户断开连接
            userErrorBreak, //用户错误断开连接(可能用户断网了, 没有正确离开)
        };

        protected Socket _userSocket = null;

        /// <summary>  
        /// 连接ID  
        /// </summary>  
        public int uuid { get; set; }

        ushort _packSize = 0;
        ushort _available = 0;

        private eResult _sendResult = eResult.userResponse;
        public eResult sendResult
        {
            set
            {
                _sendResult = value;
            }
            get
            {
                return _sendResult;
            }
        }

        public NetClientBase()
        {
            uuid = 0;
        }

        public NetClientBase(Socket userSocket)
        {
            _userSocket = userSocket;

            System.Timers.Timer aTimerClientConnectUpdate = new System.Timers.Timer();
            aTimerClientConnectUpdate.Elapsed += new System.Timers.ElapsedEventHandler(ClientConnectUpdate);
            aTimerClientConnectUpdate.Interval = 1500;
            aTimerClientConnectUpdate.AutoReset = false;
            aTimerClientConnectUpdate.Enabled = true;
        }

        public void ClientConnectUpdate(object sender, System.Timers.ElapsedEventArgs e)
        {
            BytesWriter write = new BytesWriter();
            write.WriteByte((byte)S2CBattleProtocol.S2C_Connected);
            write.WriteInt(uuid);

            Send(write);
        }
        
        //发送
        public void Send(BytesWriter write)
        {
            if (_userSocket == null)
            {
                sendResult = eResult.userBreak;
                MyDebug.WriteLine("...10033...");
                return;
            }
            try
            {
                int sendSize = write.GetBufferLen();
                int s = _userSocket.Send(write.GetBuffer(), sendSize, 0);

                if (s < sendSize)
                {
                    MyDebug.WriteLine("...10034...");
                    Close();
                    sendResult = eResult.userBreak;
                    return;
                }

                sendResult = eResult.userResponse;
                return;
            }
            catch (SocketException e)
            {
                if (e.ErrorCode.Equals(10035))
                {
                    MyDebug.WriteLine("...10035...");
                    sendResult = eResult.userDelay;
                    return;
                }
                MyDebug.WriteLine("...10036...");
                Close();
                sendResult = eResult.userBreak;
                return;
            }
        }

        //确定用户是否在线
        public bool pollError
        {
            get
            {
                if (_userSocket == null)
                    return true;

                try
                {
                    _userSocket.Send(g_bufferKeep);
                    return false;
                }
                catch (SocketException e)
                {
                    if (e.ErrorCode.Equals(10035))
                        return false;

                    Close();
                    return true;
                }
            }
        }

        //是否有数据到来
        public eResult isRecv
        {
            get
            {
                if (_userSocket == null)
                    return eResult.userBreak;

                _available = (UInt16)_userSocket.Available;

                if (_available == 0)
                    return eResult.userHold;

                if (_packSize > 0 && _packSize <= _available)
                    return eResult.userResponse;

                if (_available < sizeof(UInt16))
                    return eResult.userDelay;

                //第1段数据到来
                _userSocket.Receive(g_bufferByte, sizeof(UInt16), 0);
                _packSize = (UInt16)(g_bufferByte[0] + ((UInt16)g_bufferByte[1] << 8));
                //_available -= sizeof(UInt16);
                _packSize -= sizeof(UInt16);
                return _packSize <= _available ? eResult.userResponse : eResult.userDelay;
            }
        }

        //接收
        public bool Recv(byte[] buffer)
        {
            if (_packSize > _available)
                return false;

            if (buffer.Length < _packSize)
                buffer = new byte[_packSize];

            _userSocket.Receive(buffer,_packSize, 0);

            _packSize = 0;
            return true;
        }

        public void Close()
        {
            if (null != _userSocket)
            {
                try
                {
                    _userSocket.Shutdown(SocketShutdown.Both);//进制接收或发送
                }
                catch
                {

                }
                _userSocket.Close();
                _available = 0;
                _userSocket = null;
            }
        }

        //全局接收byte
        static byte[] g_bufferByte = new byte[sizeof(UInt16)];
        static byte[] g_bufferKeep = new byte[0];
    }
}
