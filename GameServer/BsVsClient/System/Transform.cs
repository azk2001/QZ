using System.Collections;
using System;

public class Transform:ICloneable
{
	public Vector3 localEulerAngles
	{
		get
		{
			return new Vector3(0.0f, localEulerAngles_Y, 0.0f);
		}
	}

	public float localEulerAngles_Y
	{
		get
		{
			float val = Vector3.Angle(Vector3.forward, forward);

			if (forward.x < 0.0f)
			{
				val = 360.0f-val;
			}
			return val;
		}
	}

	public Vector3 right
	{
		get
		{
			Vector3 v3 = Vector3.zero;
			if (forward.x >= 0.0f && forward.z >= 0.0f)
			{
				v3.x = forward.z;
				v3.z = -forward.x;
			}
			else if (forward.x <= 0.0f && forward.z >= 0.0f)
			{
				v3.x = forward.z;
				v3.z = -forward.x;
			}
			else if (forward.x <= 0.0f && forward.z <= 0.0f)
			{
				v3.x = -forward.z;
				v3.z = forward.x;
			}
			else if (forward.x >= 0.0f && forward.z <= 0.0f)
			{
				v3.x = -forward.z;
				v3.z = forward.x;
			}
			return v3;
		}
	}

	public Vector3 forward = Vector3.forward;
	public Vector3 localPosition = Vector3.zero;

    public Vector3 position;    //当前位置;
    public Vector3 eulerAngles; //旋转值;
    public Vector3 localScale;   //大小;

    public object Clone()
    {
        return MemberwiseClone();
    }
}

