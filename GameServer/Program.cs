using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace GameServer
{
    class Program
    {
        public static byte mServerType = 0; //服务器类型，0普通战斗服务器，1跨服战战斗服务器
        public static SocketManager socket = null;

        private static List<RoomBase> roomList = new List<RoomBase>();
        private static float deltaTime = 0.5f;

        static void Main(string[] args)
        {

            string sysform = Environment.OSVersion.Platform.ToString();

            MyDebug.WriteLine("...TableManager.Instance.Init....");
            TableManager.Instance.Init();
            MyDebug.WriteLine("...TableManager.Instance.Init over....");

            socket = new SocketManager();
            BattleProtocol.InitProtocol();

            RoomManager.Init();

            //创建socket去监听客户端的消息
            string ip = string.Empty;
            if (sysform.IndexOf("Win") >= 0)
            {
                foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                    {
                        ip = _IPAddress.ToString();
                    }
                }
            }


            Console.WriteLine("服务器启动成功：IP" + ip);

            if (socket.Start(new IPEndPoint(IPAddress.Parse(ip), 8000)))
            {
                if (mServerType == 1)
                    MyDebug.WriteLine("...开启跨服战斗服务器成功....");
                else
                    MyDebug.WriteLine("...开启战斗服务器成功....");

                socket.BeginAcceptClientConnect();
            }
            else
            {
                if (mServerType == 1)
                    MyDebug.WriteLine("...开启跨服战斗服务器失败....");
                else
                    MyDebug.WriteLine("...开启战斗服务器失败....");
            }

            System.Timers.Timer gameUpdateTime = new System.Timers.Timer();
            gameUpdateTime.Elapsed += new System.Timers.ElapsedEventHandler(GameUpdate);
            gameUpdateTime.Interval = deltaTime*1000;
            gameUpdateTime.AutoReset = true;
            gameUpdateTime.Enabled = true;
            gameUpdateTime.Start();

            //BattleRoom roomBase = new BattleRoom();
            //RoomManager.AddRoom(roomBase);
            //roomBase.sceneId = 10001;
            //roomBase.StartScene();
            //roomBase.StartBattle();


            ClientUpdate();
            
        }

        public static void ClientUpdate()
        {
            while(true)
            {
                socket.Update();

                Thread.Sleep(10);
            }
        }

        public static void GameUpdate(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dictionary<int, RoomBase> roomBaseDic = RoomManager.GetAllRoomList();
            roomList = new List<RoomBase>(roomBaseDic.Values);

            lock(roomList)
            {
                for (int i = roomList.Count - 1; i >= 0; i--)
                {
                    roomList[i].Update(deltaTime);
                }
            }
        }

        /*
         * 发送消息到客户端
         */
        public static bool SendMsgToClient(int clientid, BytesWriter bytes)
        {
            socket.SendMessage(clientid, bytes);
            return true;
        }

        /*
         * 关闭客户端
         */
        public static void CloseClient(Int32 clientid)
        {
            socket.CloseClient(clientid);
        }
    }
}
