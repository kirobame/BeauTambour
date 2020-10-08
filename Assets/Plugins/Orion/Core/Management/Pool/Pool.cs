using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    public abstract class Pool<T> : SerializedMonoBehaviour
    {
        /// <summary>
        /// Stock the passed <code>Poolable</code> for it to have its lifetime & handling managed by the <code>Pool</code>.
        /// </summary>
        /// <param name="poolable">The <code>Poolable</code> to be stocked.</param>
        public abstract void Stock(Poolable<T> poolable);
    }
    
    /// <summary>
    /// <code>Utility</code> component responsible for the lifetime & handling of a collection of <code>Poolable</code>.
    /// </summary>
    /// <typeparam name="T">The true type to handle.</typeparam>
    /// <typeparam name="TPoolable">The wrapper <code>Poolable</code> type which handles a true type instance.</typeparam>
    public abstract class Pool<T,TPoolable> : Pool<T>, IProvider<T> where TPoolable : Poolable<T>
    {
        [SerializeField] private TPoolable prefab;
        [ShowInInspector, ReadOnly] protected List<TPoolable> instances = new List<TPoolable>();
    
        private Queue<TPoolable> availableInstances = new Queue<TPoolable>();
        private HashSet<TPoolable> usedInstances = new HashSet<TPoolable>();

        void Awake()
        {
            instances.AddRange(GetComponentsInChildren<TPoolable>(true));
            foreach (var instance in instances) availableInstances.Enqueue(instance);
        }

        /// <summary>
        /// Asks for only one <code>T</code> instance. Provision is guaranteed.
        /// </summary>
        /// <returns>The <code>T</code> instance to be returned.</returns>
        public T RequestSingle() => Request(1).First();
        /// <summary>
        /// Asks for any number of <code>T</code> instances. Provision is guaranteed.
        /// </summary>
        /// <param name="count">The number of <code>T</code> instances to be returned.</param>
        /// <returns>The <code>T</code> instance collection to be returned.</returns>
        public T[] Request(int count)
        {
            for (var i = 0; i < count - availableInstances.Count; i++)
            {
                var instance = Instantiate(prefab, transform);

                instances.Add(instance);
                availableInstances.Enqueue(instance);
            }
        
            var request = new T[count];
            for (var i = 0; i < count; i++)
            {
                var instance = availableInstances.Dequeue();
                
                Claim(instance);
                request[i] = instance.Value;
            }

            return request;
        }

        /// <summary>
        /// Asks for a specific number of <code>T</code> instances matching a condition. Provision is not guaranteed.
        /// </summary>
        /// <param name="count">The number of <code>T</code> instances to be returned.</param>
        /// <param name="predicate">The condition allowing the determine if an instance is a match or not.</param>
        /// <returns>The <code>T</code> instance collection to be returned.</returns>
        public T[] RequestSpecific(int count, Func<TPoolable, bool> predicate) => RequestSpecific(prefab, count, predicate);
        /// <summary>
        /// Asks for a specific number of <code>T</code> instances matching a condition. Provision is guaranteed as long as the passed prefab also matches the condition.
        /// </summary>
        /// <param name="prefab">The model which will be instantiated if there are not enough matching instances in the <code>Pool</code>.</param>
        /// <param name="count">The number of <code>T</code> instances to be returned.</param>
        /// <param name="predicate">The condition allowing the determine if an instance is a match or not.</param>
        /// <returns>The <code>T</code> instance collection to be returned.</returns>
        public T[] RequestSpecific(TPoolable prefab, int count, Func<TPoolable, bool> predicate)
        {
            var request = new T[count];
            var index = 0;

            var toRequeue = new List<TPoolable>();
            while (availableInstances.Count > 0 && index < count)
            {
                var instance = availableInstances.Dequeue();
                
                if (predicate(instance) == true)
                {
                    Claim(instance);
                    request[index] = instance.Value;
                    
                    index++;
                }
                else toRequeue.Add(instance);
            }

            foreach (var instance in toRequeue) availableInstances.Enqueue(instance);

            for (var i = 0; i < count - index; i++)
            {
                var instance = Instantiate(prefab);
                
                instance.gameObject.SetActive(true);
                instance.SetOrigin(this);
                
                instances.Add(instance);
                usedInstances.Add(instance);

                request[index + i] = instance.Value;
            }

            return request;
        }

        public override void Stock(Poolable<T> poolable) => Stock(poolable as TPoolable);
        public void Stock(TPoolable poolable)
        {
            if (this == null || !gameObject.activeInHierarchy) return;

            poolable.Reboot();
            poolable.gameObject.SetActive(false);
            
            var routine = new WaitForEndOfFrame().ToRoutine();
            routine.Append(new ActionRoutine() {action = () => poolable.transform.SetParent(transform)});

            StartCoroutine(routine.Call);

            availableInstances.Enqueue(poolable);
            if (!usedInstances.Remove(poolable)) instances.Add(poolable);
        }

        private void Claim(TPoolable poolable)
        {
            poolable.SetOrigin(this);
            
            poolable.gameObject.SetActive(true);
            poolable.transform.SetParent(null);
            
            usedInstances.Add(poolable);
        }
        
        object IProvider.GetInstance() => RequestSingle();
        T IProvider<T>.GetInstance() => RequestSingle();
    }
}