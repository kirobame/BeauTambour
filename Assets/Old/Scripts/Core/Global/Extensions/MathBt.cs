namespace Deprecated
{
    public static class MathBt
    {
        public static double Clamp(double value, double min, double max)
        {
            if (value < min)
            {
                value = min;
                return value;
            }

            if (value > max) value = max;
            return value;
        }
    }
}