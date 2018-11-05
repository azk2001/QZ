using System;
using System.Collections;

public struct Vector3
{
	public static Vector3 zero
	{
		get
		{
			return new Vector3(0.0f, 0.0f, 0.0f);
		}
	}

	public static Vector3 one
	{
		get
		{
			return new Vector3(1.0f, 1.0f, 1.0f);
		}
	}

	public static float Distance(Vector3 a, Vector3 b)
	{
		Vector3 vector = new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
		return (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
	}

	public static float Magnitude(Vector3 a)
	{
		return (float)Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
	}

	public static float Dot(Vector3 lhs, Vector3 rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
	}

	public static float Angle(Vector3 from, Vector3 to)
	{
		return (float)Math.Acos(Clamp(Vector3.Dot(from.normalized, to.normalized), -1f, 1f)) * 57.29578f;
	}

	public static float Clamp(float value, float min, float max)
	{
		if (value < min)
		{
			value = min;
		}
		else
		{
			if (value > max)
			{
				value = max;
			}
		}
		return value;
	}

	public static Vector3 Normalize(Vector3 value)
	{
		float num = Vector3.Magnitude(value);
		if (num > 1E-05f)
		{
			return value / num;
		}
		return Vector3.zero;
	}

	public static float SqrMagnitude(Vector3 a)
	{
		return a.x * a.x + a.y * a.y + a.z * a.z;
	}

	public static Vector3 forward
	{
		get
		{
			return new Vector3(0f, 0f, 1f);
		}
	}

	//
	// Operators
	//
	public static Vector3 operator +(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
	}
	
	public static Vector3 operator /(Vector3 a, float d)
	{
		return new Vector3(a.x / d, a.y / d, a.z / d);
	}
	
	public static bool operator ==(Vector3 lhs, Vector3 rhs)
	{
		return Vector3.SqrMagnitude(lhs - rhs) < 9.99999944E-11f;
	}
	
	public static bool operator !=(Vector3 lhs, Vector3 rhs)
	{
		return Vector3.SqrMagnitude(lhs - rhs) >= 9.99999944E-11f;
	}
	
	public static Vector3 operator *(Vector3 a, float d)
	{
		return new Vector3(a.x * d, a.y * d, a.z * d);
	}
	
	public static Vector3 operator *(float d, Vector3 a)
	{
		return new Vector3(a.x * d, a.y * d, a.z * d);
	}
	
	public static Vector3 operator -(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
	}
	
	public static Vector3 operator -(Vector3 a)
	{
		return new Vector3(-a.x, -a.y, -a.z);
	}

    public float sqrMagnitude
    {
        get
        {
            return this.x * this.x + this.y * this.y + this.z * this.z;
        }
    }

	public float magnitude
	{
		get { return (float)Math.Sqrt((double)(x * x + y * y + z * z)); }
	}

	public Vector3 normalized
	{
		get
		{
			return Vector3.Normalize(this);
		}
	}

	public void Normalize()
	{
		float num = Vector3.Magnitude(this);
		if (num > 1E-05f)
		{
			x /= num;
			y /= num;
			z /= num;
		}
		else
		{
			x = 0.0f;
			y = 0.0f;
			z = 0.0f;
		}
	}

//	public Vector3()
//	{
//		x = 0.0f;
//		y = 0.0f;
//		z = 0.0f;
//	}

	public Vector3(float xPos, float yPos, float zPos)
	{
		x = xPos;
		y = yPos;
		z = zPos;
	}

	public float x;
	public float y;
	public float z;

    public override string ToString()
    {
        return string.Format("{0}&{1}&{2}", x, y, z);
    }
}

