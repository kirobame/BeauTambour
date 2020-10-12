using System;

namespace Orion
{
    public interface IBinder
    {
        object Input { get; }

        event Action<object> onActionStarted;
        event Action<object> onActionEnded;
    }
    
    public interface IBinder<T> : IBinder
    {
        bool IsCompatibleWith(Type type);
    }
}