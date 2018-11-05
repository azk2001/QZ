using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Remoting.Messaging;

public delegate void OnReceiveSocketData(byte[] bytes);

//unity 和 socket 消息通信接口
public class SocketDataDispitcher
{
    public OnReceiveSocketData OnReceive = null;

    private List<SocketData> listData = new List<SocketData>();

    public void Update()
    {
        while (listData.Count > 0)
        {
            SocketData data = listData[0];
            listData.RemoveAt(0);

            if (OnReceive != null)
            {
                OnReceive(data.receiveBytes);
            }
        }
    }

    public void Push(SocketData data)
    {
        listData.Add(data);
    }

}

public class SocketData
{
    public void Build(byte[] bytes, short totalLen, SocketInst inst, bool isCompress = true)
    {
        this.totalLen = (short)(totalLen - sizeof(ushort));

        Buffer.BlockCopy(bytes, sizeof(ushort), receiveBytes, 0, this.totalLen);

        if (isCompress == false)
        {
            inst.tmpReceiveUncompressBuff = receiveBytes;

            // Buffer.BlockCopy(inst._tmp_receive_uncompress_buff, 0, _bytes, 0, _total_len);
        }
        else
        {

            int receiveUnncompressLen = inst.mCompress.decompress(receiveBytes, this.totalLen, inst.tmpReceiveUncompressBuff);

            this.totalLen = (short)receiveUnncompressLen;

            if (receiveUnncompressLen == -1)
            {
                Console.WriteLine("uncompress error!");
                return;
            }

            Buffer.BlockCopy(inst.tmpReceiveUncompressBuff, 0, receiveBytes, 0, receiveUnncompressLen);
        }

    }

    public void DebugLog()
    {
        string str = BitConverter.ToString(receiveBytes, sizeof(short), (totalLen - sizeof(short)));
        Console.WriteLine("msg : " + str);
    }

    public short totalLen = 0;

    public byte[] receiveBytes = new byte[819200];
}

public class SocketDataBuffer
{
    private int offset = 0;
    private byte[] _bytes = new byte[0xffff];
    private List<SocketData> lstData = new List<SocketData>();	// List是线程安全的

    public void AddBuffer(byte[] buff, int len, SocketInst inst)
    {
        Buffer.BlockCopy(buff, 0, _bytes, offset, len);
        offset += len;

        if (offset < sizeof(ushort))
            return;

        short totalLen = BitConverter.ToInt16(_bytes, 0);

        // 提取完整数据块
        while (offset >= totalLen)
        {
            // 后续优化：socket_data变固定大小，加缓存，降GC
            SocketData data = new SocketData();
            data.Build(_bytes, totalLen, inst, inst.isCompress);

            Buffer.BlockCopy(_bytes, totalLen, _bytes, 0, offset - totalLen);

            offset = offset - totalLen;

            lstData.Add(data);

            if (offset > sizeof(ushort))
            {
                totalLen = BitConverter.ToInt16(_bytes, 0);
            }
            else
            {
                break;
            }
        }
    }

    public SocketData pickData()
    {
        SocketData data = null;
        if (lstData.Count > 0)
        {
            data = lstData[0];
            lstData.RemoveAt(0);
        }
        return data;
    }

    public void Clear()
    {
        offset = 0;
        lstData.Clear();
    }
}

public class SocketInst
{

    public void Connect(string _ip, int _port)
    {
        ipAddress = _ip;
        port = _port;

        _isStopReceive = false;

        sendCallback = new AsyncCallback(OnSendSucc);


        if (threadConnect == null)
        {
            threadConnect = new Thread(new ThreadStart(OnConnect));
        }
        threadConnect.Start();
    }

    public void Send(byte[] _bytes, int _length)
    {
        try
        {
            if (socket == null || !socket.Connected)
            {
                _isConnected = false;
                Console.WriteLine("connection is invalid!" + socket + "   " + socket.Connected);
                return;
            }

            socket.BeginSend(_bytes, 0, _length, SocketFlags.None, sendCallback, socket);
        }
        catch (Exception e)
        {
            _isConnected = false;
            Console.WriteLine(e);
        }
    }

    public void Close()
    {
        if (!_isConnected || _isConnecting)
            return;

        _isConnected = false;
        _isConnecting = false;
        _isStopReceive = true;

        if (receiveThread != null)
        {
            receiveThread.Abort();
            receiveThread = null;
        }

        if (threadConnect != null)
        {
            threadConnect.Abort();
            threadConnect = null;
        }

        if (socket != null)
        {
            if (socket.Connected)
            {
                //Console.WriteLine("_socket.Close ();1");
                socket.Close();
            }
            socket = null;
        }

        _data_buffer.Clear();

        mCompress = new Compress();
    }

    // ----------------------------------------

    private void OnConnect()
    {
        try
        {
            threadConnect = null;

            if (socket != null)
            {
                Close();
            }

            _isConnecting = true;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //原始代码如下，由于做了花生壳域名映射！不能再做解析！
            /*IPAddress ___ip_address = IPAddress.Parse(_ip_address);
            IPEndPoint ___ip_end_point = new IPEndPoint(___ip_address, port);

            IAsyncResult __result = _socket.BeginConnect(___ip_end_point, new AsyncCallback(on_connect_succ), _socket);
            */

            //花生壳导致的新的解析
            IAsyncResult __result = socket.BeginConnect(ipAddress, port, new AsyncCallback(OnConnectSucc), socket);

            bool succ = __result.AsyncWaitHandle.WaitOne(5000, true);

            if (!succ)
            {
                OnConnectOutTime();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            OnConnectFailed();
        }
    }

    private void OnReceive()
    {
        while (!isStopReceive)
        {
            if (socket == null)
                continue;

            if (!socket.Connected && !isConnecting)
            {
                _isConnected = false;
                OnReconnect();
                break;
            }

            try
            {
                int receiveLength = socket.Receive(tmpReceiveCompressBuff);

                if (receiveLength > 0)
                {
                   // Console.WriteLine("recv: " + __receive_length);

                    _data_buffer.AddBuffer(tmpReceiveCompressBuff, receiveLength, this);
                    SocketData data = null;
                    do
                    {
                        data = _data_buffer.pickData();

                        if (data != null)
                        {
                            // 投递消息;
                            // ...
                            dispitcher.Push(data);
                        }

                    } while (data != null);
                }
            }
            catch (Exception e)
            {
                break;
            }
        }
    }

    private void OnConnectSucc(IAsyncResult result)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)result.AsyncState;

            _isConnecting = false;

            // Complete the connection.  
            client.EndConnect(result);

            Console.WriteLine("connect succ!");

            if (receiveThread == null)
            {
                receiveThread = new Thread(new ThreadStart(OnReceive));
                receiveThread.Start();
            }

            _isConnected = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void OnSendSucc(IAsyncResult result)
    {
        Socket client = (Socket)result.AsyncState;
        client.EndSend(result);
    }

    private void OnConnectOutTime()
    {
        _isConnected = false;
        _isConnecting = false;
    }

    private void OnConnectFailed()
    {
        _isConnected = false;
        _isConnecting = false;
    }

    public void OnReconnect()
    {
        Close();

        Console.WriteLine(string.Format("reconnect.  {0}", System.DateTime.Now));

        if (threadConnect == null)
        {
            threadConnect = new Thread(new ThreadStart(OnConnect));
        }
        threadConnect.Start();

    }

    public bool isWorking
    {
        get
        {
            return _isConnected || isConnecting;
        }
    }

    public SocketDataDispitcher dispitcher
    {
        get
        {
            return _dispitcher;
        }
    }

    public bool isConnected
    {
        get
        {
            return _isConnected;
        }
    }

    public bool isConnecting
    {
        get
        {
            return _isConnecting;
        }
    }

    public bool isStopReceive
    {
        get
        {
            return _isStopReceive;
        }
    }
    


    // ---------------------------------------

    public Action<short, byte[]> _on_receive_msg;

    public Compress mCompress = new Compress();

    public bool isCompress = true;  //是否启动数据压缩;

    private AsyncCallback sendCallback = null;

    private bool _isStopReceive = false;
    private bool _isConnecting = false;
    private bool _isConnected = false;    //是否连接成功;
    private Socket socket = null;
    private int port = 8080;
    private string ipAddress = "";
    private byte[] tmpReceiveCompressBuff = new byte[0xffff];
    public byte[] tmpReceiveUncompressBuff = new byte[0xffff];

    private Thread threadConnect = null;
    private Thread receiveThread = null;

    private SocketDataDispitcher _dispitcher = new SocketDataDispitcher();
    private SocketDataBuffer _data_buffer = new SocketDataBuffer();
}
