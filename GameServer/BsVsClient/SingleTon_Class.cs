
public class SingleTon_Class<T> where T : SingleTon_Class<T>, new()
{
    private static T instance;

    protected SingleTon_Class()
    {

    }
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}
