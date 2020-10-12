namespace BeauTambour.Prototyping
{
    public interface ITweenable<T>
    {
        T Start { get; }
        T End { get; }

        void Apply(T value);
    }
}