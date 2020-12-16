﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Flux
{
    public abstract class Pool : MonoBehaviour { }
    public abstract class Pool<T> : Pool
    {
        public abstract void Stock(Poolable<T> poolable);
    }
    public abstract class Pool<T, TPoolable> : Pool<T> where TPoolable : Poolable<T>
    {
        protected abstract Provider<T,TPoolable>[] Providers { get; }

        private Dictionary<TPoolable, Queue<TPoolable>> availableInstances = new Dictionary<TPoolable, Queue<TPoolable>>();
        private HashSet<TPoolable> usedInstances = new HashSet<TPoolable>();

        void Awake()
        {
            foreach (var provider in Providers)
            {
                var queue = new Queue<TPoolable>(provider.Instances);
                availableInstances.Add(provider.Prefab, queue);
            }
        }

        public T RequestSingle() => RequestSinglePoolable().Value;
        public T RequestSingle(TPoolable key) => RequestSinglePoolable(key).Value;
        
        public TPoolable RequestSinglePoolable() => RequestPoolable(Providers[0].Prefab, 1).First();
        public TPoolable RequestSinglePoolable(TPoolable key) => RequestPoolable(key, 1).First();

        public T[] Request(int count) => RequestPoolable(count).Select(poolable => poolable.Value).ToArray();
        public T[] Request(TPoolable key, int count) => RequestPoolable(key, count).Select(poolable => poolable.Value).ToArray();
        
        public TPoolable[] RequestPoolable(int count) => RequestPoolable(Providers[0].Prefab, count);
        public TPoolable[] RequestPoolable(TPoolable key, int count)
        {
            var request = new TPoolable[count];
            if (availableInstances.TryGetValue(key, out var queue))
            {
                int index;
                for (index = 0; index < count - queue.Count; index++)
                {
                    var instance = Instantiate(key, transform);
                    Claim(instance, key);
                    
                    request[index] = instance;
                    index++;
                }
                
                for (var i = index; i < count; i++)
                {
                    var instance = queue.Dequeue();
                
                    Claim(instance, key);
                    request[i] = instance;
                }
            }
            else
            {
                availableInstances.Add(key, new Queue<TPoolable>());
                for (var i = 0; i < count; i++)
                {
                    var instance = Instantiate(key);
                    Claim(instance, key);
                    
                    request[i] = instance;
                }
            }

            return request;
        }
        
        public override void Stock(Poolable<T> poolable) => Stock(poolable as TPoolable);
        public void Stock(TPoolable poolable)
        {
            if (this == null || !gameObject.activeInHierarchy) return;

            poolable.Reboot();
            poolable.gameObject.SetActive(false);
            
            StartCoroutine(ParentingRoutine(poolable));

            availableInstances[(TPoolable)poolable.Key].Enqueue(poolable);
            usedInstances.Remove(poolable);
        }
        private IEnumerator ParentingRoutine(TPoolable poolable)
        {
            yield return new WaitForEndOfFrame();
            poolable.transform.SetParent(transform);
        }

        private void Claim(TPoolable poolable, TPoolable key)
        {
            poolable.SetOrigin(this, key);
            poolable.Prepare();
            
            poolable.gameObject.SetActive(true);
            poolable.transform.SetParent(null);
            
            usedInstances.Add(poolable);
        }
    }
}