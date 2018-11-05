//与System.Action 一样
//如果超出4个参数, 应该考虑定义为类

//返回值不能为void
public delegate BACK Delegate<BACK>();

public delegate BACK Delegate<BACK, T0>(T0 val0);

public delegate BACK Delegate<BACK, T0, T1>(T0 val0, T1 val1);

public delegate BACK Delegate<BACK, T0, T1, T2>(T0 val0, T1 val1, T2 val2);

public delegate BACK Delegate<BACK, T0, T1, T2, T3, T4>(T0 val0, T1 val1, T2 val2, T3 val3, T4 val4);

public delegate BACK Delegate<BACK, T0, T1, T2, T3, T4, T5>(T0 val0, T1 val1, T2 val2, T3 val3, T4 val4, T5 val5);


//返回值void
public delegate void VoidDelegate();

public delegate void VoidDelegate<T0>(T0 val0);

public delegate void VoidDelegate<T0, T1>(T0 val0, T1 val1);

public delegate void VoidDelegate<T0, T1, T2>(T0 val0, T1 val1, T2 val2);