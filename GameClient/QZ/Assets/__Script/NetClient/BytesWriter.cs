using System;

public class BytesWriter
{
    private const int BUFF_SIZE = 819200;
    private byte[] _binary = new byte[BUFF_SIZE]; //读取数据总大小不能超过4k
    private int _count = 0;
    private bool _ignore_header = false;
    private bool _big_endian = false;

    public BytesWriter(bool big_endian = false, bool ignore_header = false)
    {
        _ignore_header = ignore_header;

        if (!_ignore_header)
        {
            _count = sizeof(ushort);
        }

        _big_endian = big_endian;
    }

    public int GetBufferLen()
    {
        return _count;
    }

    public byte[] GetBuffer()
    {
        byte[] buffer = new byte[_count];

        if (!_ignore_header)
        {
            ushort len = (ushort)(_count);

            WriteHeader(len);
        }

        //Buffer.BlockCopy (_binary, 0, buffer, 0, _count);

        return _binary;
    }

    private void WriteHeader(ushort data)
    {
        if (_big_endian)
        {
            for (int i = sizeof(ushort) - 1; i >= 0; i--)
            {
                _binary[i] = (byte)(data & 0x00ff);
                data >>= 8;
            }
        }
        else
        {
            for (int i = 0; i < sizeof(ushort); i++)
            {
                _binary[i] = (byte)(data & 0x00ff);
                data >>= 8;
            }
        }
    }

    public void WriteInt(int data)
    {
        if (_count + sizeof(int) > BUFF_SIZE - 1)
        {
            throw new Exception("buffer is full");
        }

        if (_big_endian)
        {
            for (int i = sizeof(int) - 1; i >= 0; i--)
            {
                _binary[i + _count] = (byte)(data & 0x000000ff);
                data >>= 8;
            }

        }
        else
        {
            for (int i = 0; i < sizeof(int); i++)
            {
                _binary[_count + i] = (byte)(data & 0x000000ff);
                data >>= 8;
            }

        }

        this._count += sizeof(int);
    }
    public void WriteUint(uint data)
    {
        if (_count + sizeof(uint) > BUFF_SIZE - 1)
        {
            throw new Exception("buffer is full");
        }

        if (_big_endian)
        {
            for (uint i = sizeof(uint) - 1; i >= 0; i--)
            {
                _binary[i + _count] = (byte)(data & 0x000000ff);
                data >>= 8;
            }

        }
        else
        {
            for (uint i = 0; i < sizeof(uint); i++)
            {
                _binary[_count + i] = (byte)(data & 0x000000ff);
                data >>= 8;
            }

        }

        this._count += sizeof(uint);
    }

    public void WriteUtf(string data)
    {
        if (data.Length > ushort.MaxValue)
        {
            throw new Exception("string is too long");
        }

        if (_count + data.Length > BUFF_SIZE - 1)
        {
            throw new Exception("buffer is full");
        }

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(data);
        int utflen = buffer.Length;
        byte a = (byte)((utflen >> 8) & 0xFF);
        byte b = (byte)((utflen >> 0) & 0xFF);
        byte[] sendUTF = new byte[utflen + 2];
        sendUTF[0] = a;
        sendUTF[1] = b;
        Array.Copy(buffer, 0, sendUTF, 2, utflen);

        if (_big_endian)
        {
            for (int i = 0; i < sendUTF.Length; i++)
            {
                _binary[_count + i] = sendUTF[i];
            }

        }
        else
        {

            for (int i = sendUTF.Length - 1; i >= 0; i--)
            {
                _binary[_count + i] = sendUTF[i];
            }
        }

        this._count += sendUTF.Length;
    }

    /// <summary>
    /// 写入string
    /// </summary>
    /// <param name="data">写入String</param>
    /// <param name="big_endian">是否按小端字节序排列</param>
    /// <returns>void</returns>
    public void WriteString(string data, int length)
    {

        if (data.Length > length)
        {
            throw new Exception("length is invalid");
        }

        if (length > ushort.MaxValue || data.Length > ushort.MaxValue)
        {
            throw new Exception("string is too long");
        }

        if (_count + length > BUFF_SIZE - 1)
        {
            throw new Exception("buffer is full");
        }

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(data);

        if (_big_endian)
        {
            for (int i = 0; i < length; i++)
            {
                if (i > (buffer.Length - 1))
                {
                    _binary[_count + i] = 0;
                }
                else
                {
                    _binary[_count + i] = buffer[i];
                }
            }

        }
        else
        {

            for (int i = length - 1; i >= 0; i--)
            {
                if (i > (buffer.Length - 1))
                {
                    _binary[_count + i] = 0;
                }
                else
                {
                    _binary[_count + i] = buffer[i];
                }
            }
        }

        this._count += length;

    }

    /// <summary>
    /// 写入short
    /// </summary>
    /// <param name="data">写入short</param>
    /// <param name="big_endian">是否按小端字节序排列</param>
    /// <returns>void</returns>
    public void WriteShort(short data)
    {
        if (_count + sizeof(short) > BUFF_SIZE - 1)
        {
            throw new Exception("buffer is full");
        }

        if (_big_endian)
        {
            for (int i = sizeof(short) - 1; i >= 0; i--)
            {
                _binary[_count + i] = (byte)(data & 0x00ff);
                data >>= 8;
            }
        }
        else
        {
            for (int i = 0; i < sizeof(short); i++)
            {
                _binary[_count + i] = (byte)(data & 0x00ff);
                data >>= 8;
            }

        }

        this._count += sizeof(short);
    }

    /// <summary>
    /// 写入long
    /// </summary>
    /// <param name="data">写入long</param>
    /// <param name="big_endian">是否按小端字节序排列</param>
    /// <returns>void</returns>
    public void WriteLong(long data)
    {
        if (_count + sizeof(long) > BUFF_SIZE - 1)
        {
            throw new Exception("buffer is full");
        }

        if (_big_endian)
        {
            for (int i = sizeof(long) - 1; i >= 0; i--)
            {
                _binary[_count + i] = (byte)(data & 0x00000000000000ff);
                data >>= 8;
            }
        }
        else
        {
            for (int i = 0; i < sizeof(long); i++)
            {
                _binary[_count + i] = (byte)(data & 0x00000000000000ff);
                data >>= 8;
            }
        }

        this._count += sizeof(long);
    }

    /// <summary>
    /// 写入byte
    /// </summary>
    /// <param name="data">写入byte</param>
    /// <returns>void</returns>
    public void WriteByte(byte data)
    {
        _binary[_count++] = data;
    }

    public void WriteBytes(byte[] data)
    {
        Buffer.BlockCopy(data, 0, _binary, _count, data.Length);
        _count += data.Length;
    }

    public void Clear()
    {
        //		for(int i = 0; i < BUFF_SIZE; i++)
        //		{
        //			binary[i] = 0;
        //		}
        _count = _ignore_header ? 0 : sizeof(short);
    }
}

