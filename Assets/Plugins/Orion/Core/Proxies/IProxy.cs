using System.Collections;
using System.Collections.Generic;

namespace Orion
{ 
    /// <summary>
    /// Allows to read from a wrapped value which can be remote.
    /// </summary>
    public interface IReadable
    { 
        object Read();
    }
    /// <summary>
    /// Allows to read from a wrapped value of type <code>T</code> which can be remote.
    /// </summary>
    public interface IReadable<out T> : IReadable
    {
        T Read();
    }

    /// <summary>
    /// Allows to write to a wrapped value which can be remote.
    /// </summary>
    public interface IWritable
    {
        void Write(object value);
    }
    /// <summary>
    /// Allows to write to a wrapped value of type <code>T</code> which can be remote.
    /// </summary>
    public interface IWritable<in T> : IWritable
    {
        void Write(T value);
    }
    
    /// <summary>
    /// Allows the manipulation of a wrapped value which can be remote.
    /// </summary>
    public interface IProxy : IReadable, IWritable { }
    /// <summary>
    /// Allows the manipulation of a wrapped value of type <code>T</code> which can be remote.
    /// </summary>
    public interface IProxy<T> : IReadable<T>, IWritable<T> { }
}