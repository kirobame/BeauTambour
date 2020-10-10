namespace BeauTambour.Prototyping
{
    public static class MathBt
    {
        public static double Clamp01(this double value) => Clamp(value, 0d, 1d);
        public static double Clamp(this double value, double min, double max)
        {
            if (value < min) value = min;
            else if (value > max) value = max;

            return value;
        }
    }
}