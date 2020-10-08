using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    /// <summary>
    /// Management class allowing access to any value stored through a <code>Token</code> asset.
    /// </summary>
    public static class Repository
    {
        /// <summary>
        /// All <code>object</code> stored with a single <code>Token</code>.
        /// </summary>
        public static IReadOnlyDictionary<Token, object> Objects => objects;
        /// <summary>
        /// All <code>object</code> collections stored with a single <code>Token</code>.
        /// </summary>
        public static IReadOnlyDictionary<Token, List<object>> Stacks => stacks;

        /// <summary>
        /// All <code>Referencer</code> which are currently active & storing their content.
        /// </summary>
        public static IReadOnlyDictionary<Token, Referencer> Referencers => referencers;
        /// <summary>
        /// All <code>StackReferencer</code> which are currently active & storing their content.
        /// </summary>
        public static IReadOnlyDictionary<Token, StackReferencer> StackReferencers => stackReferencers;
        
        private static Dictionary<Token, object> objects = new Dictionary<Token, object>();
        private static Dictionary<Token, List<object>> stacks = new Dictionary<Token, List<object>>();

        private static Dictionary<Token, Referencer> referencers = new Dictionary<Token, Referencer>();
        private static Dictionary<Token, StackReferencer> stackReferencers = new Dictionary<Token, StackReferencer>();

        #region Registration
        
        /// <summary>
        /// Stores the single value of a <code>Referencer</code>.
        /// </summary>
        public static void Register(Referencer referencer)
        {
            try
            {
                referencers.Add(referencer.Token, referencer);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();

                throw;
            }
            Register(referencer.Token, referencer.Value);
        }
        /// <summary>
        /// Stores a single value the given <code>Token</code> key.
        /// </summary>
        public static void Register(Token token, object value)
        {
            try
            {
                objects.Add(token, value);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();

                throw;
            }
        }

        /// <summary>
        /// Stores the single value of a <code>StackReferencer</code> to the collection at its <code>Token</code> key.
        /// </summary>
        public static void RegisterStack(StackReferencer referencer)
        {
            try
            {
                stackReferencers.Add(referencer.Token, referencer);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();

                throw;
            }
            RegisterStack(referencer.Token, referencer.Values);
        }
        /// <summary>
        /// Stores a single value to a collection at the given <code>Token</code> key.
        /// </summary>
        public static void RegisterStack(Token token, object value)
        {
            if (stacks.TryGetValue(token, out var list)) list.Add(value);
            else stacks.Add(token, new List<object>() {value});
        }
        /// <summary>
        /// Stores multiple values to a collection at the given <code>Token</code> key.
        /// </summary>
        public static void RegisterStack(Token token, IEnumerable<object> values)
        {
            if (stacks.TryGetValue(token, out var list)) list.AddRange(values);
            else stacks.Add(token, new List<object>(values));
        }
        #endregion
        
        #region Unregistration
        
        /// <summary>
        /// Removes from the stored content a single value indicated by an <code>IReferencer</code>.
        /// </summary>
        /// <returns>Indicates if the removal was the successful</returns>
        public static bool Unregister(IReferencer referencer)
        {
            return referencers.Remove(referencer.Token) && Unregister(referencer.Token);
        }
        /// <summary>
        /// Removes from the stored content a single value at the given <code>Token</code> key.
        /// </summary>
        /// <param name="token">The key.</param>
        /// <returns>Indicates if the removal was the successful</returns>
        public static bool Unregister(Token token) => objects.Remove(token);
        
        /// <summary>
        /// Deletes entirely the collection at the given <code>Token</code> key.
        /// </summary>
        /// <param name="token">The key.</param>
        /// <returns>Indicates if the deletion was the successful</returns>
        public static bool UnregisterStack(Token token) => stacks.Remove(token);
        
        /// <summary>
        /// Removes all the content of a <code>StackReferencer</code> in the collection at the given <code>Token</code> key.
        /// </summary>
        /// <returns>Indicates if the removal was entirely successful</returns>
        public static bool RemoveFromStack(StackReferencer referencer)
        {
            return referencers.Remove(referencer.Token) && RemoveFromStack(referencer.Token, referencer.Values).All(success => success);
        }
        /// <summary>
        /// Removes a specific value in the collection at the given <code>Token</code> key.
        /// </summary>
        /// <param name="token">The key.</param>
        /// <param name="value">The value to be removed.</param>
        /// <returns>Indicates if the removal was the successful</returns>
        public static bool RemoveFromStack(Token token, object value)
        {
            try
            {
                var removalSuccess = stacks[token].Remove(value);
                if (removalSuccess && stacks[token].Count == 0) stacks.Remove(token);

                return removalSuccess;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();
                
                throw;
            }
        }
        /// <summary>
        /// Removes all specified values in the collection at the given <code>Token</code> key.
        /// </summary>
        /// <param name="token">The key.</param>
        /// <param name="values">The values to be removed.</param>
        /// <returns>Indicates if the removal was the successful</returns>
        public static bool[] RemoveFromStack(Token token, IEnumerable<object> values)
        {
            var successes = new bool[values.Count()];
            List<object> collection;

            try
            {
                collection = stacks[token];
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();
                
                throw;
            }

            var index = 0;
            foreach (var value in values)
            {
                successes[index] = collection.Remove(value);
                index++;
            }

            return successes;
        }
        #endregion

        /// <summary>
        /// Returns the single value at the given <code>Token</code> key casted to the specified <code>T</code> type.
        /// </summary>
        /// <param name="token">The key.</param>
        /// <typeparam name="T">The cast type.</typeparam>
        public static T Get<T>(Token token) => (T)objects[token];
        /// <summary>
        /// Returns the collection at the given <code>Token</code> key with its elements casted to the specified <code>T</code> type.
        /// </summary>
        /// <param name="token">The key.</param>
        /// <typeparam name="T">The cast type/.</typeparam>
        public static IEnumerable<T> GetStack<T>(Token token) => stacks[token].Cast<T>();

        /// <summary>
        /// Tries to return the single value at the given <code>Token</code> key casted to the specified <code>T</code> type.
        /// </summary>
        /// <param name="token">The key.</param>
        /// <param name="value">The casted value.</param>
        /// <typeparam name="T">The cast type.</typeparam>
        /// <returns>Indicates if the operation was successful.</returns>
        public static bool TryGet<T>(Token token, out T value)
        {
            if (objects.TryGetValue(token, out var rawValue))
            {
                value = (T)rawValue;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
        /// <summary>
        /// Tries to return the collection at the given <code>Token</code> key with its elements casted to the specified <code>T</code> type.
        /// </summary>
        /// <param name="token">The key.</param>
        /// <param name="values">The casted collection.</param>
        /// <typeparam name="T">The cast type/.</typeparam>
        /// <returns>Indicates if the operation was successful.</returns>
        public static bool TryGetStack<T>(Token token, out IEnumerable<T> values)
        {
            if (stacks.TryGetValue(token, out var list))
            {
                values = list.Cast<T>();
                return true;
            }
            else
            {
                values = default;
                return false;
            }
        }

        /// <summary>
        /// Sets the value of the reference stored at the given <code>Token</code> key.
        /// </summary>
        public static void Set<T>(Token token, T value)
        {
            objects[token] = value;
            if (referencers.TryGetValue(token, out var referencer)) referencer.SetValue(value);
        }
        /// <summary>
        /// Sets the value of the reference stored in the collection at the given <code>Token</code> key & at the given index.
        /// </summary>
        public static void SetStackAt<T>(Token token, T value, int index)
        {
            stacks[token][index] = value;
            if (stackReferencers.TryGetValue(token, out var referencer)) referencer.SetValue(value, index);
        }
        
        /// <summary>
        /// Tries to set the value of the reference stored at the given <code>Token</code> key.
        /// </summary>
        /// <returns>Indicates if the operation was successful.</returns>
        public static bool TrySet<T>(Token token, T value)
        {
            if (objects.ContainsKey(token))
            {
                if (referencers.TryGetValue(token, out var referencer)) referencer.SetValue(value);
                
                objects[token] = value;
                return true;
            }

            return false;
        }
        /// <summary>
        /// Tries to set the value of the reference stored in the collection at the given <code>Token</code> key & at the given index.
        /// </summary>
        /// <returns>Indicates if the operation was successful.</returns>
        public static bool TrySetStackAt<T>(Token token, T value, int index)
        {
            if (stacks.ContainsKey(token))
            {
                if (stackReferencers.TryGetValue(token, out var referencer)) referencer.SetValue(value, index);
                
                stacks[token][index] = value;
                return true;
            }

            return false;
        }
    }
}