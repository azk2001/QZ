using System.Collections;
using System.Collections.Generic;


public class ClientBattle
{
    private static ClientBattle instance = null;
    public static ClientBattle Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ClientBattle();
            }
            return instance;
        }
    }

    public string identifyCode = "";
    private bool isSendConnect = false;
    private VoidBoolDelegate onConnect = null;

    private SocketInst _socketInst = new SocketInst();

    public SocketInst socketInst
    {
        get
        {
            return _socketInst;
        }
    }

    public void NetConnect(string serverIp, int serverPort, VoidBoolDelegate onConnect)
    {
        this.onConnect = onConnect;
        isSendConnect = true;
        _socketInst.isCompress = false;  //不启用数据压缩;
        _socketInst.Close();

        if (!_socketInst.isWorking)
        {
            _socketInst.Connect(serverIp, serverPort);
        }
    }

    public void Send(byte[] __bytes, int __length)
    {
        socketInst.Send(__bytes, __length);
    }

    public void OnClose()
    {
        _socketInst.Close();
    }

    public void OnDestroy()
    {
        _socketInst.Close();
    }

    public void Update()
    {
        _socketInst.dispitcher.Update();

        if (_socketInst != null)
        {
            if (_socketInst.isConnected == true && isSendConnect == true && onConnect != null)
            {
                this.onConnect(true);

                isSendConnect = false;
            }
        }
    }

}
