using System;

namespace Flux
{
    public interface IBindable
    {
        event Action<EventArgs> onStart;
        event Action<EventArgs> onUpdate;
        event Action<EventArgs> onEnd;
    }
}