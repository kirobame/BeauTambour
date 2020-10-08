using System;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    [HideReferenceObjectPicker]
    public abstract class PersistentCallBase
    {
        public Object Target => target;
        public string Info => info;

        [SerializeField, HideInInspector] protected string info = string.Empty;
        
        [SerializeField, HideLabel] protected Object target;
        [SerializeField, HideLabel] protected string method = string.Empty;

        protected int hasBeenInitialized = 0;

        public virtual void Set(Object target, string method, string info)
        {
            this.target = target;
            this.method = method;
            
            this.info = info;
        }
        public MethodInfo GetMethod()
        {
            if (target == null) return null;
            
            var split = method.Split('/');

            var parameters = new Type[split.Length - 1];
            for (var i = 1; i < split.Length; i++) parameters[i - 1] = Type.GetType(split[i]);

            var source = target.GetType().GetMethod(split[0], parameters);
            return source;
        }
        
        public virtual void Invoke(object[] args)
        {
            if (hasBeenInitialized == -1) return;
            else if (hasBeenInitialized == 0)
            {
                var source = GetMethod();
                if (source == null)
                {
                    hasBeenInitialized = -1;
                    return;
                }
                
                Initialize(source);
                hasBeenInitialized = 1;
            }
        }

        protected abstract void Initialize(MethodInfo source);

        protected object[] GetValues(object[] parameters, object[] args, bool[,] linkage)
        {
            var values = new object[parameters.Length];

            for (var x = 0; x < parameters.Length; x++)
            {
                var useArg = false;
                object arg = null;
                
                for (var y = 0; y < args.Length; y++)
                {
                    if (!linkage[x, y]) continue;

                    useArg = true;
                    arg = args[y];
                }

                if (useArg) values[x] = arg;
                else values[x] = parameters[x];
            }

            return values;
        }
    }
}