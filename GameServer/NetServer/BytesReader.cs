using System;

public class BytesReader
{
    private byte[] binary;
    private int count;

    public int Count
    {
        get
        {
            return count;
        }
    }

    public BytesReader()
    {

    }

    public BytesReader(byte[] byteBinary)
    {
        this.binary = byteBinary;
        count = 0;
    }

    public void ReBytesReader(byte[] byteBinary)
    {
        Clear();
        this.binary = byteBinary;
        count = 0;
    }

    /// <summary>
    /// 读取short
    /// </summary>
    /// <param name="asc">是否按小端字节序排列</param>
    /// <returns>short</returns>
    public short ReadShort(bool asc = false)
    {
        if (binary == null)
        {
            throw new Exception();
        }
        if (count + sizeof(short) > binary.Length)
        {
            throw new Exception();
        }

        short data = 0;
        if (asc)
        {
            for (int i = 0; i < sizeof(short); i++)
            {
                data <<= 8;
                data |= (short)(binary[count + i] & 0x00ff);
            }
        }
        else
        {
            for (int i = sizeof(short) - 1; i >= 0; i--)
            {
                data <<= 8;
                data |= (short)(binary[count + i] & 0x00ff);
            }
        }

        count += sizeof(short);
        return data;
    }

    /// <summary>
    /// 读取Int
    /// </summary>
    /// <param name="asc">是否按小端字节序排列</param>
    /// <returns>Int</returns>
    public int ReadInt(bool asc = false)
    {
        if (binary == null)
        {
            throw new Exception();
        }

        if ((count + sizeof(int)) > binary.Length)
        {
            throw new Exception();
        }

        int data = 0;
        if (asc)
        {
            for (int i = 0; i < sizeof(int); i++)
            {
                data <<= 8;
                data |= (binary[count + i] & 0x000000ff);
            }
        }
        else
        {
            for (int i = sizeof(int) - 1; i >= 0; i--)
            {
                data <<= 8;
                data |= (binary[count + i] & 0x000000ff);
            }
        }

        count += sizeof(int);
        return data;
    }

    /// <summary>
	/// 读取float
	/// </summary>
	/// <returns>Float</returns>
	public float ReadFloat()
    {
        if (binary == null)
        {
            throw new Exception();
        }

        if ((count + sizeof(float)) > binary.Length)
        {
            throw new Exception();
        }

        float data = BitConverter.ToSingle(binary, count);

        count += sizeof(float);

        return data;
    }

    public uint ReadUint(bool asc = false)
    {

        if (binary == null)
        {
            throw new Exception();
        }

        if ((count + sizeof(int)) > binary.Length)
        {
            throw new Exception();
        }

        uint data = 0;
        if (asc)
        {
            for (int i = 0; i < sizeof(uint); i++)
            {
                data <<= 8;
                data |= ((uint)binary[count + i] & 0x000000ff);
            }
        }
        else
        {
            for (int i = sizeof(uint) - 1; i >= 0; i--)
            {
                data <<= 8;
                data |= ((uint)binary[count + i] & 0x000000ff);
            }
        }

        count += sizeof(int);
        return data;
    }

    /// <summary>
    /// 读取字符串(java-UTF)
    /// </summary>
    /// <param name="asc">是否按小端字节序排列</param>
    /// <returns>short</returns>
    public string ReadUtf(bool asc = false)
    {
        byte high = binary[count++];
        byte low = binary[count++];

        int length = (high << 8) | low;

        if (binary == null)
        {
            throw new Exception();
        }
        if (count + length > binary.Length)
        {
            throw new Exception();
        }

        byte[] buffer = new byte[length];
        if (asc)
        {
            for (int i = 0; i < length; i++)
            {
                buffer[i] = binary[count + i];
            }

        }
        else
        {
            for (int i = length - 1; i >= 0; i--)
            {
                buffer[i] = binary[count + i];
            }
        }

        count += length;

        return System.Text.Encoding.UTF8.GetString(buffer);

    }


    /// <summary>
    /// 读取字符串(C-String)
    /// </summary>
    /// <param name="asc">是否按小端字节序排列</param>
    /// <param name="length">指定读取字符串长度</param>
    /// <returns>short</returns>
    public string ReadString(int length, bool asc = false)
    {

        if (binary == null)
        {
            throw new Exception("buffer is null");
        }
        if (count + length > binary.Length)
        {
            throw new Exception("length invalid");
        }

        byte[] buffer = new byte[length];
        if (asc)
        {
            for (int i = 0; i < length; i++)
            {
                //					Debug.Log(binary[count+i]);
                buffer[i] = binary[count + i];
            }

        }
        else
        {
            for (int i = length - 1; i >= 0; i--)
            {
                buffer[i] = binary[count + i];
            }
        }

        count += length;
        string cString = System.Text.Encoding.UTF8.GetString(buffer);
        return cString;

    }


    /// <summary>
    /// 读取long
    /// </summary>
    /// <param name="asc">是否按小端字节序排列</param>
    /// <returns>long</returns>
    public long ReadLong(bool asc = false)
    {
        if (binary == null)
        {
            throw new Exception();
        }
        if (count + sizeof(long) > binary.Length)
        {
            throw new Exception();
        }

        long data = 0;
        if (asc)
        {
            for (int i = 0; i < sizeof(long); i++)
            {
                data <<= 8;
                data |= (binary[count + i] & 0x00000000000000ff);
            }
        }
        else
        {
            for (int i = sizeof(long) - 1; i >= 0; i--)
            {
                data <<= 8;
                data |= (binary[count + i] & 0x00000000000000ff);
            }
        }

        count += sizeof(long);
        return data;

    }

    /// <summary>
    /// 读取byte
    /// </summary>
    /// <returns>byte</returns>
    public byte ReadByte()
    {
        if (binary == null)
        {
            throw new Exception();
        }
        if (count + sizeof(byte) > binary.Length)
        {
            throw new Exception();
        }

        count += sizeof(byte);
        return binary[count - 1];

    }

    public void SkipByte(int length)
    {
        if (length < 0)
        {
            //Debug.Log("error : skipByte len [" + length + "]");
            return;
        }

        for (int i = 0; i < length; i++)
        {
            count++;
        }
    }

    public void Clear()
    {
        binary = null;
        count = 0;
    }

    public static string RomoveInvalid(string str)
    {
        return str.Substring(0, str.IndexOf('\0'));
    }
}

