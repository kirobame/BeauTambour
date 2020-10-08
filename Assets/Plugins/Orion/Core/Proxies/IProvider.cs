using Sirenix.OdinInspector;

namespace Orion
{
    /// <summary>
    /// An interface which gives the guarantee of obtaining a fresh new instance.
    /// </summary>
    [HideReferenceObjectPicker]
    public interface IProvider
    {
        object GetInstance();
    }
    
    /// <summary>
    /// An interface which gives the guarantee of obtaining a fresh new instance of type <code>T</code>.
    /// </summary>
    [HideReferenceObjectPicker]
    public interface IProvider<out T> : IProvider
    {
        T GetInstance();
    }
}