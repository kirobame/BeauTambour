using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour
{
    [IconIndicator(863600071063726587), CreateAssetMenu(fileName = "NewInputSequence", menuName = "Beau Tambour/Input Sequence")]
    public class InputSequence : ScriptableObject, IBindable
    {
        #region Encapsulated Types
        
        [Serializable]
        public class Element
        {
            public InputAction Action => actionReference.action;
            public InputHandler Handler => runtimeHandler;
            
            [SerializeField] private InputActionReference actionReference;
            [SerializeField] private InputHandler handler;

            private InputHandler runtimeHandler;
            private AdvanceSequenceOperation operation;

            public void Initialize(InputSequenceHandler hook, InputSequence sequence, int index)
            {
                runtimeHandler = Instantiate(handler);
                runtimeHandler.Initialize(hook);
                runtimeHandler.Bind(Action);
                
                operation = ScriptableObject.CreateInstance<AdvanceSequenceOperation>();
                operation.Initialize(hook);
                operation.Bind(runtimeHandler);
                
                operation.SetData(sequence, index);
            }
        }
        #endregion
        
        public event Action<EventArgs> onStart;
        public event Action<EventArgs> onUpdate;
        public event Action<EventArgs> onEnd;

        public IReadOnlyList<IContinuousHandler> ContinuousHandlers => continuousHandlers;
        public int Advancement { get; private set; }

        [SerializeField] private Element[] elements;
        [SerializeField] private Operation[] operations;
        
        private IContinuousHandler[] continuousHandlers;
        private Operation[] runtimeOperations;

        public void Initialize(InputSequenceHandler handler)
        {
            runtimeOperations = new Operation[operations.Length];
            for (var i = 0; i < operations.Length; i++)
            {
                var runtimeOperation = Instantiate(operations[i]);
                runtimeOperation.Initialize(handler);
                runtimeOperation.Bind(this);
                
                runtimeOperations[i] = runtimeOperation;
            }

            Advancement = -1;
            
            for (var i = 0; i < elements.Length; i++) elements[i].Initialize(handler, this, i);
            continuousHandlers = elements.Where(element => element.Handler is IContinuousHandler).Select(element => element.Handler).Cast<IContinuousHandler>().ToArray();
        }

        public void Advance(int index)
        {
            if (index == 0) Advancement = 0;
            else if (Advancement + 1 == index) Advancement++;
            
            if (Advancement != elements.Length - 1) return;

            Advancement = -1;
            onStart.Invoke(new EventArgs());
        }
    }
}