using System.Collections;

public static class FloatExtensionMethods
{
    public static bool IsZero(this float that)
    {
        return that > -0.0001f && that < 0.0001f;
    }
    public static float ToAbs(this float that)
    {
        return that > 0f ? that : -that;
    }
    public static float Clamp(this float that, float min, float max)
    {
        return that < min ? min : (that > max ? max : that);
    }
    public static float Clamp01(this float that)
    {
        return that < 0f ? 0f : (that > 1f ? 1f : that);
    }
}
