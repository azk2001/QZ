using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GameServer
{

    public class SocketManager
    {
        private static int _clientId = 0;
        public static int CLIENTID   //客服端连接ID，唯一标识，就是UUID
        {
            get
            {
                _clientId++;
                return _clientId;
            }
        }
        private static byte[] receiveBytes = new byte[4096];

        private Socket socketListen = null; //服务器监听客户端的连接
        private int clientCount = 0;        //当前连接的客户端数量

        public delegate void OnVoidReceiveData(NetClientBase token, byte[] buff);
        public event OnVoidReceiveData ReceiveClientData;


        private Dictionary<int, NetClientBase> userListClient = new Dictionary<int, NetClientBase>();       //客户端列表;
        private Dictionary<int, NetPlayer> netPlayerList = new Dictionary<int, NetPlayer>();                //客服端角色总表;

        private List<NetClientBase> clients = new List<NetClientBase>();                                    //客户端列表;
        public List<NetClientBase> ClientList { get { return clients; } }

        public SocketManager()
        {
            clientCount = 0;
            clients.Clear();
            userListClient.Clear();
            netPlayerList.Clear();
        }

        public bool Start(IPEndPoint localEndPoint)
        {
            if (socketListen != null)
                socketListen.Close();
            try
            {
                clients.Clear();
                socketListen = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socketListen.Bind(localEndPoint);

                socketListen.Listen(100);

                return true;
            }
            catch (Exception e)
            {
                socketListen.Close();
                socketListen = null;

                return false;
            }
        }

        public void BeginAcceptClientConnect()
        {
            if (socketListen != null)
            {
                socketListen.BeginAccept(AsyncCallback, socketListen);
            }
        }

        private void AsyncCallback(IAsyncResult ar)
        {
            lock (this)
            {
                int clientId = CLIENTID;

                Socket skServer = ar.AsyncState as Socket;
                Socket skClient = skServer.EndAccept(ar);
                skClient.NoDelay = true;

                NetClientBase client = new NetClientBase(skClient);
                client.uuid = clientId;

                clients.Add(client);
                userListClient[clientId] = client;

                Console.WriteLine("新用户登录" + clientId + " " + skClient.RemoteEndPoint.ToString());

                Interlocked.Increment(ref clientCount);

                skServer.BeginAccept(AsyncCallback, skServer);
            }
        }

        public void Update()
        {
            lock (clients)
            {
                for (int i = clients.Count - 1; i >= 0; --i)
                {
                    NetClientBase c = clients[i];

                    switch (c.isRecv)
                    {
                        case NetClientBase.eResult.userDelay:      //用户延迟
                        case NetClientBase.eResult.userHold:       //持续连接中
                            {
                                break;
                            }
                        case NetClientBase.eResult.userResponse:   //用户响应
                            {
                                receiveBytes.Initialize();
                                c.Recv(receiveBytes);

                                //NetEvent.OnRecv(stream);
                                //将数据包交给后台处理,这里你也可以新开个线程来处理.加快速度.  

                                if (ReceiveClientData != null)
                                    ReceiveClientData(c, receiveBytes);

                                i++;//连续接收

                                break;
                            }

                        //接收捕获不了此消息
                        case NetClientBase.eResult.userBreak:      //用户断开连接
                        case NetClientBase.eResult.userErrorBreak: //用户错误断开连接(可能用户断网了, 没有正确离开)
                            {
                                RemoveClient(clients[i]);
                                break;
                            }
                    }
                }

                for (int i = clients.Count - 1; i >= 0; --i)
                {
                    NetClientBase c = clients[i];

                    if (c.pollError)
                    {
                        RemoveClient(clients[i]);
                    }
                    else
                    {
                        switch (c.sendResult)
                        {
                            case NetClientBase.eResult.userDelay:      //用户延迟
                            case NetClientBase.eResult.userHold:       //持续连接中
                                {
                                    break;
                                }
                            case NetClientBase.eResult.userResponse:   //用户响应
                                {
                                    //成功发送
                                    break;
                                }
                            case NetClientBase.eResult.userBreak:      //用户断开连接
                            case NetClientBase.eResult.userErrorBreak: //用户错误断开连接(可能用户断网了, 没有正确离开)
                                {
                                    RemoveClient(clients[i]);
                                    break;
                                }
                        }
                    }
                }
            }
        }

        public void RemoveClient(NetClientBase hn)
        {
            int clientId = hn.uuid;

            userListClient.Remove(clientId);
            clients.Remove(hn);

            Console.WriteLine("用户断开 ID:" + clientId);

            Interlocked.Decrement(ref clientCount);
        }

        public void SendMessage(int clientid, BytesWriter data)
        {
            lock (this)
            {
                if (userListClient.ContainsKey(clientid))
                {
                    NetClientBase cc = userListClient[clientid];
                    cc.Send(data);
                }
            }
        }

        public void CloseClient(int clientid)
        {
            if (userListClient.ContainsKey(clientid))
            {
                NetClientBase cc = userListClient[clientid];
                cc.sendResult = NetClientBase.eResult.userBreak;
            }
        }

        public void updateClientNum()
        {
            Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] BattleServer " + " online client " + clientCount);
        }
    }
}
