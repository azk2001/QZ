using System.Net;
using System.Net.Sockets;

public class NetServerBase
{
    ~NetServerBase()
    {
        Close();
        CloseClientListen();
    }

    /*
     * 连接战斗管理服
     */
    private Socket _socket = null;  //服务器套接字连接战斗管理服

    public Socket socket
    {
        get
        {
            return _socket;
        }
    }

    public void Close()
    {
        if (_socket != null)
        {
            _socket.Close();
            _socket = null;
        }
    }

    public bool StartConnect()
    {
        if (_socket != null)
        {
            _socket.Close();
        }
        _socket = null;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        return true;
    }

    /*
     * 监听客户端连接
     */
    private Socket _listenClientsocket = null;//服务器套接字

    public Socket listenClientsocket
    {
        get
        {
            return _listenClientsocket;
        }
    }
    public void CloseClientListen()
    {
        if (_listenClientsocket != null)
        {
            _listenClientsocket.Close();
            _listenClientsocket = null;
        }
    }
    ///<summary>
    ///<para>false:可能端口无效,或iListenMax非法</para>
    ///<para> true:成功连接</para>
    ///</summary>
    public bool Start(string ip, int iPortNumber, int iListenMax = 20)
    {
        if (_listenClientsocket != null)
            _listenClientsocket.Close();

        _listenClientsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _listenClientsocket.Blocking = false;//设置未非阻塞
        _listenClientsocket.NoDelay = true;
        try
        {
            IPAddress ipaddress = IPAddress.Parse(ip);
            _listenClientsocket.Bind(new IPEndPoint(ipaddress, iPortNumber));
            _listenClientsocket.Listen(iListenMax);
            return true;
        }
        catch
        {
            _listenClientsocket.Close();
            _listenClientsocket = null;
            return false;
        }
    }
}
