using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Orion
{
    [HideReferenceObjectPicker]
    public abstract class OrionEventBase
    {
        [ListDrawerSettings(CustomAddFunction = "AddPersistentCall", ShowItemCount = false)]
        [SerializeField] protected PersistentCallBase[] persistentCalls = new PersistentCallBase[0];

        protected void Invoke(object[] args)
        {
            foreach (var persistentCall in persistentCalls) persistentCall.Invoke(args);
        }

        protected abstract PersistentCallBase AddPersistentCall();
    }
}