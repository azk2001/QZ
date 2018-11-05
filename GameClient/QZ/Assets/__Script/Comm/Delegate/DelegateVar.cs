/// <summary>
/// 委托变量
/// </summary>
public struct DelegateVar<T>
{
    public VoidDelegate<T> onValueChange;

    public T value
    {
        set
        {
            mValue = value;
            if (onValueChange != null)
                onValueChange(mValue);
        }
        get
        {
            return mValue;
        }
    }

    private T mValue;

    public DelegateVar(T value)
    {
        onValueChange = null;
        mValue = value;
    }

    //一下暂时屏蔽, 委托处理回调存在问题			
    //		//DelegateVar<bool> d;
    //		//return d ? 1 : 0;
    //		public static implicit operator T(DelegateVar<T> value)
    //        {
    //            return value.mValue;
    //        }
    ////
    //		//DelegateVar<bool> d = false;
    //		//d = true;//这里是不会触发委托, 
    //		public static implicit operator DelegateVar<T>(T value)
    //        {
    //			DelegateVar<T> d = new DelegateVar<T>();
    //			return d;
    //        }
}