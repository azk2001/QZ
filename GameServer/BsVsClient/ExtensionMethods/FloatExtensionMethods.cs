using System.Collections;

namespace MXEngine
{
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

        public static bool TryParse(this float that, out string value)
        {
            value = that.ToString();
            return true;
        }

        public static bool TryParse(this double that, out string value)
        {
            value = that.ToString();
            return true;
        }
    }
}
