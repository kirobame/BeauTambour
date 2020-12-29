using BeauTambour;
using Flux;

namespace Deprecated
{
    public class ShapeEventArgs : SingleEventArgs<Shape>
    {
        public ShapeEventArgs(Shape value) : base(value) { }
    }
}