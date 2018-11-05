using System;
using System.Collections;

public static class Mathf
{
	public static double PI = Math.PI;

	public static float Cos(double d)
	{
		return (float)Math.Cos(d);
	}

	public static float Sin(double d)
	{
		return (float)Math.Sin(d);
	}

	public static float Abs(double d)
	{
		return (float)Math.Abs(d);
	}

	public static float Sqrt(double d)
	{
		return (float)Math.Sqrt(d);
	}

    public static float Min(float a, float b)
    {
        return Math.Min(a, b);
    }

    public static long Min(long a, long b)
    {
        return Math.Min(a, b);
    }
}

