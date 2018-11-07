class CreatePlayerData : SingleClass  <CreatePlayerData>
    {
    private UICreatePlayer createPlayer = null;

    private BytesWriter writer = new BytesWriter();
    private BytesReader reader = null;
    private void Init()
    {
        if (createPlayer == null)
        {
            createPlayer = UIManager.Instance.GetUIBase<UICreatePlayer>(eUIName.UICreatePlayer);
        }
    }

    public void SendCreatePlayer(string name, byte sex, int icon, int occ)
    {
        if (name.Equals("") == false)
        {
            writer.Clear();

            writer.WriteByte((byte)C2SBattleProtocol.C2S_Create);

            writer.WriteString(name, 64);
            writer.WriteByte(sex);
            writer.WriteInt(occ);   //职业;


        }
    }

    public void ReceiveCreatePlayer(byte[] bytes)
    {
       
    }

    public void OpenCreatePlayer(string randNames)
    {
        Init();

        createPlayer.randNames = randNames;

        UIManager.Instance.OpenUI(eUIName.UICreatePlayer);


    }
}
