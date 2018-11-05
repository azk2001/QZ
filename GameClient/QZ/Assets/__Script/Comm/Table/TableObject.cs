
//关联配置表与类
public class TableLoadObject
{
    //路径(配置表所在路径)
    public string tablePath;

    //事件(一般为序列化或反序列化事件)
    public VoidDelegate<Table> onSerialize;

    public TableLoadObject(string tablePath, VoidDelegate<Table> onLoadFinish)
    {
        this.tablePath = tablePath;
        this.onSerialize = onLoadFinish;
    }
}

//关联配置表与类
public class TableWriteObject
{
    //路径(配置表所在路径)
    public string tablePath;

    //事件(一般为序列化或反序列化事件)
    public Delegate<string> onDeserialize;

    public TableWriteObject(string tablePath, Delegate<string> onBeginWrite)
    {
        this.tablePath = tablePath;
        this.onDeserialize = onBeginWrite;
    }
}