using System;
using System.Collections.Generic;
using System.Text;

public class Compress
{
	public Compress()
	{
	    mCodeValue = new UInt16[HashTableSize];
	    mPrefixCode = new UInt16[HashTableSize];
	    mFreq = new UInt16[HashTableSize];
	    mAppendChar = new byte[HashTableSize];
	    mDecodeStack = new byte[DecodeStackSize];

	    reset();
	}

	public Int32 compress(byte[] src, byte[] dst)
	{
	    byte b = 0;
	    UInt16 stringCode = 0;
	    UInt32 index = 0;
	    byte[] srcTemp = src;
	    byte[] dstTemp = dst;
	    UInt32 remainSize = (UInt32)dst.Length;
	    UInt32 srcLength = (UInt32)src.Length;
	    Int32 srcIndex = 0;
	    Int32 dstIndex = 0;

	    stringCode = srcTemp[srcIndex];
	    srcIndex++;

	    mBitBuffer = 0;
	    mBitCount = 0;

	    while (--srcLength > 0)
	    {
	        b = srcTemp[srcIndex];
	        srcIndex++;
	        index = (UInt32)findHashIndex(stringCode, b);
	        if (mCodeValue[index] != InvalidWord)
	        {
	            stringCode = mCodeValue[index];
	            mFreq[stringCode]++;
	            continue;
	        }
	        if (outputCode(stringCode, dstTemp, ref dstIndex, ref remainSize) < 0)
	        {
	            return -1;
	        }
	        if (mNextCode > MaxCode)
	        {
	            rebuildDictTable();
	            stringCode = b;
	            continue;
	        }

	        mCodeValue[index] = mNextCode;
	        mPrefixCode[mNextCode] = stringCode;
	        mAppendChar[mNextCode] = b;
	        ++mNextCode;
	        stringCode = b;
	    }

	    if (outputCode(stringCode, dstTemp, ref dstIndex, ref remainSize) < 0)
	    {
	        return -1;
	    }
	    if (outputCode(MaxValue, dstTemp, ref dstIndex, ref remainSize) < 0)
	    {
	        return -1;
	    }
	    if (outputCode(0, dstTemp, ref dstIndex, ref remainSize) < 0)
	    {
	        return -1;
	    }

	    if (mNextCode > MaxCode)
	    {
	        rebuildDictTable();
	    }
	    return (Int32)dst.Length - (Int32)remainSize;
	}

	public Int32 decompress(byte[] src, Int32 srcLength, byte[] dst)
	{
	    UInt16 newCode = InvalidWord;
	    UInt16 oldCode = InvalidWord;
	    byte b = 0;
	    byte[] str = mDecodeStack;
	    byte[] srcTemp = src;
	    byte[] dstTemp = dst;
	    Int32 remainSize = dst.Length;
	    Int32 srcIndex = 0;
	    Int32 dstIndex = 0;

	    mBitCount = 0;
	    mBitBuffer = 0;

	    while ((newCode = inputCode(srcTemp, ref srcIndex, ref srcLength)) != MaxValue)
	    {
	        if (newCode == InvalidWord)
	        {
	            return -1;
	        }

	        int i = 0;
	        str = mDecodeStack;

	        if (newCode >= mNextCode)
	        {
	            if (newCode >= MinCode && oldCode != InvalidWord)
	            {
	                ++mFreq[newCode];
	            }

	            str[i] = b;
	            i++;
	            if (!decodeString(str, ref i, oldCode))
	            {
	                return -1;
	            }
	        }
	        else
	        {
	            if (!decodeString(str, ref i, newCode))
	            {
	                return -1;
	            }
	        }
	        b = str[i];
	        while (i >= 0)
	        {
	            if (remainSize-- <= 0)
	            {
	                return -1;
	            }
	            dstTemp[dstIndex] = str[i];
	            dstIndex++;
	            i--;
	        }

	        if (oldCode != InvalidWord)
	        {
	            UInt16 value = (UInt16)findHashIndex(oldCode, b);
	            if (mCodeValue[value] == InvalidWord)
	            {
	                mCodeValue[value] = mNextCode;
	            }

	            mPrefixCode[mNextCode] = oldCode;
	            mAppendChar[mNextCode] = b;
	            ++mNextCode;

	            if (mNextCode > MaxCode)
	            {
	                oldCode = InvalidWord;
	                rebuildDictTable();
	                continue;
	            }
	        }
	        oldCode = newCode;
	    }

	    if (mNextCode > MaxCode)
	    {
	        rebuildDictTable();
	    }
	    return dst.Length - remainSize;
	}

	public void reset()
	{
	    mBitBuffer = 0;
	    mBitCount = 0;
	    mNextCode = MinCode;
	    for (UInt32 i = 0; i < HashTableSize; i++)
	    {
	        mCodeValue[i] = InvalidWord;
	        mPrefixCode[i] = InvalidWord;
	        mAppendChar[i] = InvalidByte;
	        prefixCode[i] = InvalidWord;
	        appendChar[i] = InvalidByte;
	        mMap[i] = (UInt16)i;
	        mFreq[i] = 0;
	    }

	    for (UInt32 i = 0; i < DecodeStackSize; i++)
	    {
	        mDecodeStack[i] = 0;
	    }
	}

	void rebuildDictTable()
	{
	    for (UInt32 i = 0; i < HashTableSize; i++)
	    {
	        prefixCode[i] = mPrefixCode[i];
	        appendChar[i] = mAppendChar[i];
	        mMap[i] = (UInt16)i;
	        mCodeValue[i] = InvalidWord;
	    }
	    mNextCode = MinCode;

	    for (Int32 i = 0; i <= MaxCode; i++)
	    {
	        UInt16 pc = prefixCode[i];
	        if (mFreq[i] > 0)
	        {
	            Int32 value;
	            byte ac = appendChar[i];
	            pc = mMap[pc];
	            value = (ac << HashingShift) ^ pc;
	            while (mCodeValue[value] != InvalidWord)
	            {
	                if (value == 0)
	                {
	                    value = HashTableSize - 1;
	                }
	                else
	                {
	                    value -= HashTableSize - value;
	                    if (value < 0)
	                    {
	                        value += HashTableSize;
	                    }
	                }
	            }
	            mCodeValue[value] = mNextCode;
	            mMap[i] = mNextCode;
	            mPrefixCode[mNextCode] = pc;
	            mAppendChar[mNextCode] = ac;
	            ++mNextCode;
	        }
	    }

	    for (UInt32 i = 0; i < HashTableSize; i++)
	    {
	        mFreq[i] = 0;
	    }
	}
	Int32 findHashIndex(UInt16 hashPrefix, byte hashChar)
	{
	    Int32 index;
	    UInt16 code;
	    Int32 offset;

	    index = (hashChar << HashingShift) ^ hashPrefix;
	    if (index == 0)
	    {
	        offset = 1;
	    }
	    else
	    {
	        offset = HashTableSize - index;
	    }

	    do
	    {
	        if ((code = mCodeValue[index]) == InvalidWord)
	        {
	            return index;
	        }
	        if (mPrefixCode[code] == hashPrefix &&
	            mAppendChar[code] == hashChar)
	        {
	            return index;
	        }
	        index -= offset;
	        if (index < 0)
	        {
	            index += HashTableSize;
	        }
	    } while (true);
	}
	Int32 outputCode(UInt16 code, byte[] dst, ref Int32 index, ref UInt32 dstSize)
	{
	    mBitBuffer |= (UInt32)code << (32 - CodeBits - mBitCount);
	    mBitCount += CodeBits;
	    while (mBitCount >= 8)
	    {
	        if (dstSize-- <= 0)
	        {
	            return -1;
	        }
	        dst[index] = (byte)(mBitBuffer >> 24);
	        index++;
	        mBitBuffer <<= 8;
	        mBitCount -= 8;
	    }
	    return 0;
	}
	UInt16 inputCode(byte[] src, ref Int32 index, ref Int32 srcSize)
	{
	    UInt16 result;
	    while (mBitCount < CodeBits)
	    {
	        if (srcSize-- <= 0)
	        {
	            return InvalidWord;
	        }
	        UInt32 tmp = (UInt32)(src[index]);
	        mBitBuffer |= (tmp << (24 - mBitCount));
	        mBitCount += 8;
	        index++;
	    }
	    result = (UInt16)(mBitBuffer >> (32 - CodeBits));
	    mBitBuffer <<= CodeBits;
	    mBitCount -= CodeBits;
	    return result;
	}
	bool decodeString(byte[] buffer, ref Int32 i, UInt16 code)
	{
	    while (code >= MinCode)
	    {
	        if (code >= HashTableSize)
	        {
	            return false;
	        }

	        buffer[i] = mAppendChar[code];
	        mFreq[code]++;

	        code = mPrefixCode[code];
	        if (i++ >= DecodeStackSize)
	        {
	            return false;
	        }
	    }
	    buffer[i] = (byte)code;
	    return true;
	}
	/*
	const Int32 InvalidWord = 0xffff;
	const Int32 InvalidByte = 0xff;
	const Int32 CodeBits = 13;
	const Int32 HashingShift = CodeBits - 13;
	const Int32 MaxValue = ((1 << CodeBits) - 1);
	const Int32 MinCode = 257;
	const Int32 MaxCode = (MaxValue - 1);
	const Int32 HashTableSize = 9029;
	const Int32 DecodeStackSize = 819200;
	*/

	const Int32 InvalidWord = 0xffff;
	const Int32 InvalidByte = 0xff;
	const Int32 CodeBits = 12;
	const Int32 HashingShift = CodeBits - 8;
	const Int32 MaxValue = ((1 << CodeBits) - 1);
	const Int32 MinCode = 257;
	const Int32 MaxCode = (MaxValue - 1);
	const Int32 HashTableSize = 5021;
	const Int32 DecodeStackSize = 819200;

	UInt16[] mCodeValue;
	UInt16[] mPrefixCode;
	UInt16[] mFreq;
	byte[] mAppendChar;
	byte mBitCount;
	UInt32 mBitBuffer;
	UInt16 mNextCode;
	byte[] mDecodeStack;

	UInt16[] mMap = new UInt16[HashTableSize];

	UInt16[] prefixCode = new UInt16[HashTableSize];
	byte[] appendChar = new byte[HashTableSize];
}
