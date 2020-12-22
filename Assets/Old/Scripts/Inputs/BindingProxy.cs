using System;
 using System.Linq;
 using Flux;

namespace Deprecated
 {
     public class BindingProxy : IBindable
     {
         public event Action<EventArgs> onStart;
         public event Action<EventArgs> onUpdate;
         public event Action<EventArgs> onEnd;
 
         public void Start(EventArgs args) => onStart?.Invoke(args);
         public void Update(EventArgs args) => onUpdate?.Invoke(args);
         public void End(EventArgs args) => onEnd?.Invoke(args);
     }
 }