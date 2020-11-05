using System;

namespace Flux
{
    public class SingleEventArgs<T> : EventArgs
    {
        public SingleEventArgs(T value) => Value = value;
        public T Value;
    }
}