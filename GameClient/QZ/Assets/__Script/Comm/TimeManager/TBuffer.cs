using System;
using System.Collections;

public class TBuffer<T>
{
	public delegate T Instantiation();

	public TBuffer()
	{
		onInstantiation = OnInstantiation;
	}

	public TBuffer(Instantiation onCall)
	{
		onInstantiation = onCall;
	}

	//设置缓存不足实例化函数
	public void SetInstantiation(Instantiation onCall)
	{
		onInstantiation = onCall;
	}

	//得到一个缓存对象
	public T Get()
	{
		return mLen == 0 ? onInstantiation() : mBuffers[--mLen];
	}
	
	//释放一个缓存对象
	public void   Release(T obj)
	{
		if (mLen == mBuffers.Length)
			AddRAM();
		
		mBuffers[mLen++] = obj;
	}

	//清空
	public void   Clear()
	{
		Array.Clear(mBuffers, mLen = 0, mBuffers.Length);
	}

	//缓存个数
	public int    Count
	{ get{ return mLen;} }
	
	
	#region 不用关注
	private T[] mBuffers = new T[8];
	private int mLen = 0;
	private Instantiation onInstantiation;
	private void AddRAM()
	{
		T[] p = new T[mLen << 1];
		Array.Copy(mBuffers, p, mLen);
		mBuffers = p;
	}

	//默认的分配
	private static T OnInstantiation()
	{
		return default(T);
	}
	#endregion
}
