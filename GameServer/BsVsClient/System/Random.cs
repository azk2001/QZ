using System;
using System.Collections;

public static class Random
{
	public static float Range(float a, float b)
	{
		if (_random == null)
		{
			_random = new System.Random(1313);
		}

		return ((float)_random.Next((int)(a*_scale), (int)(b*_scale)))/_scale;
	}

	public static int Range(int a, int b)
	{
		if (_random == null)
		{
			_random = new System.Random(1313);
		}
		return _random.Next(a, b);
	}

	private static float _scale = 10000.0f;
	private static System.Random _random = null;
}

