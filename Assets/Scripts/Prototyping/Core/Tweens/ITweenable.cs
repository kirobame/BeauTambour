namespace BeauTambour.Prototyping
{
    public interface ITweenable<T>
    {
        T Onset { get; }
        T Outset { get; }

        void Apply(T value);
    }
}