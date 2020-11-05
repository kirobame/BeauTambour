using Flux;

namespace BeauTambour
{
    public class ShapeEventArgs : SingleEventArgs<Shape>
    {
        public ShapeEventArgs(Shape value) : base(value) { }
    }
}