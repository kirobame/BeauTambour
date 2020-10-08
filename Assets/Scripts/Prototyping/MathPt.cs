namespace BeauTambour.Prototyping
{
    public static class MathPt
    {
        public static double Clamp01(this double value)
        {
            if (value < 0) value = 0d;
            else if (value > 1) value = 1d;

            return value;
        }
    }
}