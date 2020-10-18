using UnityEngine.InputSystem;

namespace BeauTambour.Tooling
{
    public interface IBindable
    {
        void Bind(InputAction action);
        void Unbind(InputAction action);
    }
}