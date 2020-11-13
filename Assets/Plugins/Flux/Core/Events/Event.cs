using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Flux
{
    public static class Event
    {
        #region Encapsulated Types

        private class Port
        {
            public Port(string address) => Address = address;

            public bool isBlocked;
            
            public readonly string Address;
        }
        private class VoidPort : Port
        {
            public VoidPort(string address) : base(address) { }

            private event Action callback;

            public void Invoke()
            {
                if (isBlocked) return;
                callback?.Invoke();
            }

            public void Register(Action action) => callback += action;
            public void Unregister(Action action) => callback -= action;
        }
        private class Port<T> : Port
        {
            public Port(string address) : base(address) { }

            private event Action<T> callback;

            public void Invoke(T arg)
            {
                if (isBlocked) return;
                callback?.Invoke(arg);
            }

            public void Register(Action<T> action) => callback += action;
            public void Unregister(Action<T> action) => callback -= action;
        }
        private class Port<T1,T2> : Port
        {
            public Port(string address) : base(address) { }

            private event Action<T1,T2> callback;

            public void Invoke(T1 arg1, T2 arg2)
            {
                if (isBlocked) return;
                callback?.Invoke(arg1, arg2);
            }

            public void Register(Action<T1,T2> action) => callback += action;
            public void Unregister(Action<T1,T2> action) => callback -= action;
        }
        private class Port<T1,T2,T3> : Port
        {
            public Port(string address) : base(address) { }

            private event Action<T1,T2,T3> callback;

            public void Invoke(T1 arg1, T2 arg2, T3 arg3)
            {
                if (isBlocked) return;
                callback?.Invoke(arg1, arg2, arg3);
            }

            public void Register(Action<T1,T2,T3> action) => callback += action;
            public void Unregister(Action<T1,T2,T3> action) => callback -= action;
        }
        private class Port<T1,T2,T3,T4> : Port
        {
            public Port(string address) : base(address) { }

            private event Action<T1,T2,T3,T4> callback;

            public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            {
                if (isBlocked) return;
                callback?.Invoke(arg1, arg2, arg3, arg4);
            }

            public void Register(Action<T1,T2,T3,T4> action) => callback += action;
            public void Unregister(Action<T1,T2,T3,T4> action) => callback -= action;
        }
        #endregion
        
        private static Dictionary<string, Port> globalPorts = new Dictionary<string, Port>();
        private static Dictionary<string, HashSet<object>> queuedGlobalCallbacks = new Dictionary<string, HashSet<object>>();

        private static Dictionary<string, Dictionary<object, Port>> localPorts = new Dictionary<string, Dictionary<object, Port>>();
        private static Dictionary<string, Dictionary<object, HashSet<object>>> queuedLocalCallbacks = new Dictionary<string, Dictionary<object, HashSet<object>>>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Clear()
        {
            globalPorts.Clear();
            queuedGlobalCallbacks.Clear();
            
            queuedLocalCallbacks.Clear();
            localPorts.Clear();
        }

        //--------------------------------------------------------------------------------------------------------------
        
        public static bool Open(Enum address) => Open(address.GetNiceName());
        public static bool Open(object address)
        {
            var stringedAddress = address.ToString();
            if (globalPorts.ContainsKey(stringedAddress)) return false;

            var port = new VoidPort(stringedAddress);
            globalPorts.Add(stringedAddress, port);
            
            if (!queuedGlobalCallbacks.TryGetValue(stringedAddress, out var hashSet)) return true;
            foreach (var callback in hashSet)
            {
                if (callback is Action castedCallback) port.Register(castedCallback);
            }
            queuedGlobalCallbacks.Remove(stringedAddress);

            return true;
        }

        public static bool Open<T>(Enum address) => Open<T>(address.GetNiceName());
        public static bool Open<T>(object address)
        {
            var stringedAddress = address.ToString();
            if (globalPorts.ContainsKey(stringedAddress)) return false;
            
            var port = new Port<T>(stringedAddress);
            globalPorts.Add(stringedAddress, port);
            
            if (!queuedGlobalCallbacks.TryGetValue(stringedAddress, out var hashSet)) return true;
            foreach (var callback in hashSet)
            {
                if (callback is Action<T> castedCallback) port.Register(castedCallback);
            }
            queuedGlobalCallbacks.Remove(stringedAddress);

            return true;
        }
        
        public static bool Open<T1,T2>(Enum address) => Open<T1,T2>(address.GetNiceName());
        public static bool Open<T1,T2>(object address)
        {
            var stringedAddress = address.ToString();
            if (globalPorts.ContainsKey(stringedAddress)) return false;
            
            var port = new Port<T1,T2>(stringedAddress);
            globalPorts.Add(stringedAddress, port);
            
            if (!queuedGlobalCallbacks.TryGetValue(stringedAddress, out var hashSet)) return true;
            foreach (var callback in hashSet)
            {
                if (callback is Action<T1,T2> castedCallback) port.Register(castedCallback);
            }
            queuedGlobalCallbacks.Remove(stringedAddress);

            return true;
        }
        
        public static bool Open<T1,T2,T3>(Enum address) => Open<T1,T2,T3>(address.GetNiceName());
        public static bool Open<T1,T2,T3>(object address)
        {
            var stringedAddress = address.ToString();
            if (globalPorts.ContainsKey(stringedAddress)) return false;

            var port = new Port<T1,T2,T3>(stringedAddress);
            globalPorts.Add(stringedAddress, port);
           
            if (!queuedGlobalCallbacks.TryGetValue(stringedAddress, out var hashSet)) return true;
            foreach (var callback in hashSet)
            {
                if (callback is Action<T1,T2,T3> castedCallback) port.Register(castedCallback);
            }
            queuedGlobalCallbacks.Remove(stringedAddress);

            return true;
        }
        
        public static bool Open<T1,T2,T3,T4>(Enum address) => Open<T1,T2,T3,T4>(address.GetNiceName());
        public static bool Open<T1,T2,T3,T4>(object address)
        {
            var stringedAddress = address.ToString();
            if (globalPorts.ContainsKey(stringedAddress)) return false;

            var port = new Port<T1,T2,T3,T4>(stringedAddress);
            globalPorts.Add(stringedAddress, port);

            if (!queuedGlobalCallbacks.TryGetValue(stringedAddress, out var hashSet)) return true;
            foreach (var callback in hashSet)
            {
                if (callback is Action<T1,T2,T3,T4> castedCallback) port.Register(castedCallback);
            }
            queuedGlobalCallbacks.Remove(stringedAddress);

            return true;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool Open(Enum address, object key) => Open(address.GetNiceName(), key);
        public static bool Open(object address, object key)
        {
            var stringedAddress = address.ToString();
            var port = new VoidPort(stringedAddress);

            if (!localPorts.ContainsKey(stringedAddress)) localPorts.Add(stringedAddress, new Dictionary<object, Port>() { {key, port} });
            else if (localPorts[stringedAddress].ContainsKey(key)) return false;
            else localPorts[stringedAddress].Add(key, port);
            
            if (!queuedLocalCallbacks.ContainsKey(stringedAddress) || !queuedLocalCallbacks[stringedAddress].TryGetValue(key, out var hashSet)) return true;
            
            foreach (var callback in hashSet)
            {
                if (callback is Action castedCallback) port.Register(castedCallback);
            }
            queuedLocalCallbacks[stringedAddress].Remove(stringedAddress);
            
            if (!queuedLocalCallbacks[stringedAddress].Any()) queuedLocalCallbacks.Clear();
            return true;
        }
        
        public static bool Open<T>(Enum address, object key) => Open<T>(address.GetNiceName(), key);
        public static bool Open<T>(object address, object key)
        {
            var stringedAddress = address.ToString();
            var port = new Port<T>(stringedAddress);

            if (!localPorts.ContainsKey(stringedAddress))
            {
                localPorts.Add(stringedAddress, new Dictionary<object, Port>() { {key, port} });
                return true;
            }
            else if (localPorts[stringedAddress].ContainsKey(key)) return false;

            localPorts[stringedAddress].Add(key, port);
            if (!queuedLocalCallbacks.ContainsKey(stringedAddress) || !queuedLocalCallbacks[stringedAddress].TryGetValue(key, out var hashSet)) return true;
            
            foreach (var callback in hashSet)
            {
                if (callback is Action<T> castedCallback) port.Register(castedCallback);
            }
            queuedLocalCallbacks[stringedAddress].Remove(stringedAddress);
            
            if (!queuedLocalCallbacks[stringedAddress].Any()) queuedLocalCallbacks.Clear();
            return true;
        }
        
        public static bool Open<T1,T2>(Enum address, object key) => Open<T1,T2>(address.GetNiceName(), key);
        public static bool Open<T1,T2>(object address, object key)
        {
            var stringedAddress = address.ToString();
            var port = new Port<T1,T2>(stringedAddress);

            if (!localPorts.ContainsKey(stringedAddress))
            {
                localPorts.Add(stringedAddress, new Dictionary<object, Port>() { {key, port} });
                return true;
            }
            else if (localPorts[stringedAddress].ContainsKey(key)) return false;

            localPorts[stringedAddress].Add(key, port);
            if (!queuedLocalCallbacks.ContainsKey(stringedAddress) || !queuedLocalCallbacks[stringedAddress].TryGetValue(key, out var hashSet)) return true;
            
            foreach (var callback in hashSet)
            {
                if (callback is Action<T1,T2> castedCallback) port.Register(castedCallback);
            }
            queuedLocalCallbacks[stringedAddress].Remove(stringedAddress);
            
            if (!queuedLocalCallbacks[stringedAddress].Any()) queuedLocalCallbacks.Clear();
            return true;
        }
        
        public static bool Open<T1,T2,T3>(Enum address, object key) => Open<T1,T2,T3>(address.GetNiceName(), key);
        public static bool Open<T1,T2,T3>(object address, object key)
        {
            var stringedAddress = address.ToString();
            var port = new Port<T1,T2,T3>(stringedAddress);

            if (!localPorts.ContainsKey(stringedAddress))
            {
                localPorts.Add(stringedAddress, new Dictionary<object, Port>() { {key, port} });
                return true;
            }
            else if (localPorts[stringedAddress].ContainsKey(key)) return false;

            localPorts[stringedAddress].Add(key, port);
            if (!queuedLocalCallbacks.ContainsKey(stringedAddress) || !queuedLocalCallbacks[stringedAddress].TryGetValue(key, out var hashSet)) return true;
            
            foreach (var callback in hashSet)
            {
                if (callback is Action<T1,T2,T3> castedCallback) port.Register(castedCallback);
            }
            queuedLocalCallbacks[stringedAddress].Remove(stringedAddress);
            
            if (!queuedLocalCallbacks[stringedAddress].Any()) queuedLocalCallbacks.Clear();
            return true;
        }
        
        public static bool Open<T1,T2,T3,T4>(Enum address, object key) => Open<T1,T2,T3,T4>(address.GetNiceName(), key);
        public static bool Open<T1,T2,T3,T4>(object address, object key)
        {
            var stringedAddress = address.ToString();
            var port = new Port<T1,T2,T3,T4>(stringedAddress);

            if (!localPorts.ContainsKey(stringedAddress))
            {
                localPorts.Add(stringedAddress, new Dictionary<object, Port>() { {key, port} });
                return true;
            }
            else if (localPorts[stringedAddress].ContainsKey(key)) return false;

            localPorts[stringedAddress].Add(key, port);
            if (!queuedLocalCallbacks.ContainsKey(stringedAddress) || !queuedLocalCallbacks[stringedAddress].TryGetValue(key, out var hashSet)) return true;
            
            foreach (var callback in hashSet)
            {
                if (callback is Action<T1,T2,T3,T4> castedCallback) port.Register(castedCallback);
            }
            queuedLocalCallbacks[stringedAddress].Remove(stringedAddress);
            
            if (!queuedLocalCallbacks[stringedAddress].Any()) queuedLocalCallbacks.Clear();
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------
        
        public static bool Close(Enum address) => Close(address.GetNiceName());
        public static bool Close(object address) => globalPorts.Remove(address.ToString());
        
        public static void Block(Enum address) => Block(address.GetNiceName());
        public static void Block(object address) => globalPorts[address.ToString()].isBlocked = true;
        
        public static void Unblock(Enum address) => Unblock(address.GetNiceName());
        public static void Unblock(object address) => globalPorts[address.ToString()].isBlocked = false;

        //--------------------------------------------------------------------------------------------------------------

        public static bool Close(Enum address, object key) => Close(address.GetNiceName(), key);
        public static bool Close(object address, object key) => localPorts[address.ToString()].Remove(key);
        
        public static void Block(Enum address, object key) => Block(address.GetNiceName(), key);
        public static void Block(object address, object key) => localPorts[address.ToString()][key].isBlocked = true;
        
        public static void Unblock(Enum address, object key) => Unblock(address.GetNiceName(), key);
        public static void Unblock(object address, object key) => localPorts[address.ToString()][key].isBlocked = false;
        
        //--------------------------------------------------------------------------------------------------------------
        
        public static void Call(Enum address) => Call(address.GetNiceName());
        public static void Call(object address) => ((VoidPort)globalPorts[address.ToString()]).Invoke();

        public static void Call<T>(Enum address, T arg) => Call(address.GetNiceName(), arg);
        public static void Call<T>(object address, T arg) => ((Port<T>)globalPorts[address.ToString()]).Invoke(arg);

        public static void Call<T1, T2>(Enum address, T1 arg1, T2 arg2) => Call(address.GetNiceName(), arg1, arg2);
        public static void Call<T1,T2>(object address, T1 arg1, T2 arg2) => ((Port<T1,T2>)globalPorts[address.ToString()]).Invoke(arg1, arg2);

        public static void Call<T1, T2, T3>(Enum address, T1 arg1, T2 arg2, T3 arg3) => Call(address.GetNiceName(), arg1, arg2, arg3);
        public static void Call<T1,T2,T3>(object address, T1 arg1, T2 arg2, T3 arg3) => ((Port<T1,T2,T3>)globalPorts[address.ToString()]).Invoke(arg1, arg2, arg3);

        public static void Call<T1, T2, T3, T4>(Enum address, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => Call(address.GetNiceName(), arg1, arg2, arg3, arg4);
        public static void Call<T1,T2,T3,T4>(object address, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => ((Port<T1,T2,T3,T4>)globalPorts[address.ToString()]).Invoke(arg1, arg2, arg3, arg4);

        //--------------------------------------------------------------------------------------------------------------
        
        public static void CallLocal(Enum address, object key) => CallLocal(address.GetNiceName(), key);
        public static void CallLocal(object address, object key) => ((VoidPort)localPorts[address.ToString()][key]).Invoke();
        
        public static void CallLocal<T>(Enum address, object key, T arg) => CallLocal(address.GetNiceName(), key, arg);
        public static void CallLocal<T>(object address, object key, T arg) => ((Port<T>)localPorts[address.ToString()][key]).Invoke(arg);

        public static void CallLocal<T1, T2>(Enum address, object key, T1 arg1, T2 arg2) => CallLocal(address.GetNiceName(), key, arg1, arg2);
        public static void CallLocal<T1,T2>(object address, object key, T1 arg1, T2 arg2) => ((Port<T1,T2>)localPorts[address.ToString()][key]).Invoke(arg1, arg2);

        public static void CallLocal<T1, T2, T3>(Enum address, object key, T1 arg1, T2 arg2, T3 arg3) => CallLocal(address.GetNiceName(), key, arg1, arg2, arg3);
        public static void CallLocal<T1,T2,T3>(object address, object key, T1 arg1, T2 arg2, T3 arg3) => ((Port<T1,T2,T3>)localPorts[address.ToString()][key]).Invoke(arg1, arg2, arg3);

        public static void CallLocal<T1, T2, T3, T4>(Enum address, object key, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => CallLocal(address.GetNiceName(), key, arg1, arg2, arg3, arg4);
        public static void CallLocal<T1,T2,T3,T4>(object address, object key, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => ((Port<T1,T2,T3,T4>)localPorts[address.ToString()][key]).Invoke(arg1, arg2, arg3, arg4);
        
        //--------------------------------------------------------------------------------------------------------------
        
        public static bool Register(Enum address, Action callback) => Register(address.GetNiceName(), callback);
        public static bool Register(object address, Action callback)
        {
            var stringedAddress = address.ToString();

            if (globalPorts.ContainsKey(stringedAddress))
            {
                ((VoidPort)globalPorts[stringedAddress]).Register(callback);
                return true;
            }
            else
            {
                QueueGlobalCallback(address, callback);
                return false;
            }
        }
        public static bool Register<T>(Enum address, Action<T> callback) => Register(address.GetNiceName(), callback);
        public static bool Register<T>(object address, Action<T> callback)
        {
            var stringedAddress = address.ToString();

            if (globalPorts.ContainsKey(stringedAddress))
            {
                ((Port<T>)globalPorts[stringedAddress]).Register(callback);
                return true;
            }
            else
            {
                QueueGlobalCallback(address, callback);
                return false;
            }
        }
        public static bool Register<T1,T2>(Enum address, Action<T1,T2> callback) => Register(address.GetNiceName(), callback);
        public static bool Register<T1,T2>(object address, Action<T1,T2> callback)
        {
            var stringedAddress = address.ToString();

            if (globalPorts.ContainsKey(stringedAddress))
            {
                ((Port<T1,T2>)globalPorts[stringedAddress]).Register(callback);
                return true;
            }
            else
            {
                QueueGlobalCallback(address, callback);
                return false;
            }
        }
        public static bool Register<T1,T2,T3>(Enum address, Action<T1,T2,T3> callback) => Register(address.GetNiceName(), callback);
        public static bool Register<T1,T2,T3>(object address, Action<T1,T2,T3> callback)
        {
            var stringedAddress = address.ToString();

            if (globalPorts.ContainsKey(stringedAddress))
            {
                ((Port<T1,T2,T3>)globalPorts[stringedAddress]).Register(callback);
                return true;
            }
            else
            {
                QueueGlobalCallback(address, callback);
                return false;
            }
        }
        public static bool Register<T1,T2,T3,T4>(Enum address, Action<T1,T2,T3,T4> callback) => Register(address.GetNiceName(), callback);
        public static bool Register<T1,T2,T3,T4>(object address, Action<T1,T2,T3,T4> callback)
        {
            var stringedAddress = address.ToString();

            if (globalPorts.ContainsKey(stringedAddress))
            {
                ((Port<T1,T2,T3,T4>)globalPorts[stringedAddress]).Register(callback);
                return true;
            }
            else
            {
                QueueGlobalCallback(address, callback);
                return false;
            }
        }
        private static void QueueGlobalCallback(object address, object callback)
        {
            var stringedAddress = address.ToString();
            
            if (queuedGlobalCallbacks.TryGetValue(stringedAddress, out var hashSet)) hashSet.Add(callback);
            else queuedGlobalCallbacks.Add(stringedAddress, new HashSet<object>() {callback});
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        public static bool Register(Enum address, object key, Action callback) => Register(address.GetNiceName(), callback);
        public static bool Register(object address, object key, Action callback)
        {
            var stringedAddress = address.ToString();

            if (localPorts.ContainsKey(stringedAddress) && localPorts[stringedAddress].ContainsKey(key))
            {
                ((VoidPort)localPorts[stringedAddress][key]).Register(callback);
                return true;
            }
            else
            {
                QueueLocalCallback(address, key, callback);
                return false;
            }
        }
        public static bool Register<T>(Enum address, object key, Action<T> callback) => Register(address.GetNiceName(), key, callback);
        public static bool Register<T>(object address, object key, Action<T> callback)
        {
            var stringedAddress = address.ToString();

            if (localPorts.ContainsKey(stringedAddress) && localPorts[stringedAddress].ContainsKey(key))
            {
                ((Port<T>)localPorts[stringedAddress][key]).Register(callback);
                return true;
            }
            else
            {
                QueueLocalCallback(address, key, callback);
                return false;
            }
        }
        public static bool Register<T1,T2>(Enum address, object key, Action<T1,T2> callback) => Register(address.GetNiceName(), key, callback);
        public static bool Register<T1,T2>(object address, object key, Action<T1,T2> callback)
        {
            var stringedAddress = address.ToString();

            if (localPorts.ContainsKey(stringedAddress) && localPorts[stringedAddress].ContainsKey(key))
            {
                ((Port<T1,T2>)localPorts[stringedAddress][key]).Register(callback);
                return true;
            }
            else
            {
                QueueLocalCallback(address, key, callback);
                return false;
            }
        }
        public static bool Register<T1,T2,T3>(Enum address, object key, Action<T1,T2,T3> callback) => Register(address.GetNiceName(), key, callback);
        public static bool Register<T1,T2,T3>(object address, object key, Action<T1,T2,T3> callback)
        {
            var stringedAddress = address.ToString();

            if (localPorts.ContainsKey(stringedAddress) && localPorts[stringedAddress].ContainsKey(key))
            {
                ((Port<T1,T2,T3>)localPorts[stringedAddress][key]).Register(callback);
                return true;
            }
            else
            {
                QueueLocalCallback(address, key, callback);
                return false;
            }
        }
        public static bool Register<T1,T2,T3,T4>(Enum address, object key, Action<T1,T2,T3,T4> callback) => Register(address.GetNiceName(), key, callback);
        public static bool Register<T1,T2,T3,T4>(object address, object key, Action<T1,T2,T3,T4> callback)
        {
            var stringedAddress = address.ToString();

            if (localPorts.ContainsKey(stringedAddress) && localPorts[stringedAddress].ContainsKey(key))
            {
                ((Port<T1,T2,T3,T4>)localPorts[stringedAddress][key]).Register(callback);
                return true;
            }
            else
            {
                QueueLocalCallback(address, key, callback);
                return false;
            }
        }
        private static void QueueLocalCallback(object address, object key, object callback)
        {
            var stringedAddress = address.ToString();

            if (queuedLocalCallbacks.TryGetValue(stringedAddress, out var subDictionary))
            {
                if (subDictionary.TryGetValue(key, out var hashSet)) hashSet.Add(callback);
                else subDictionary.Add(key, new HashSet<object>() {callback});
            }
            else
            {
                var hashset = new HashSet<object>() {callback};
                queuedLocalCallbacks.Add(stringedAddress, new Dictionary<object, HashSet<object>>(){ {key, hashset} });
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------

        public static void Unregister(Enum address, Action callback) => Unregister(address.GetNiceName(), callback);
        public static void Unregister(object address, Action callback)
        {
            var stringedAddress = address.ToString();
            
            if (!globalPorts.ContainsKey(stringedAddress)) return;
            ((VoidPort)globalPorts[stringedAddress]).Unregister(callback);
        }
        public static void Unregister<T>(Enum address, Action<T> callback) => Unregister(address.GetNiceName(), callback);
        public static void Unregister<T>(object address, Action<T> callback)
        {
            var stringedAddress = address.ToString();
            
            if (!globalPorts.ContainsKey(stringedAddress)) return;
            ((Port<T>)globalPorts[stringedAddress]).Unregister(callback);
        }
        public static void Unregister<T1,T2>(Enum address, Action<T1,T2> callback) => Unregister(address.GetNiceName(), callback);
        public static void Unregister<T1,T2>(object address, Action<T1,T2> callback)
        {
            var stringedAddress = address.ToString();
            
            if (!globalPorts.ContainsKey(stringedAddress)) return;
            ((Port<T1, T2>)globalPorts[stringedAddress]).Unregister(callback);
        }
        public static void Unregister<T1,T2,T3>(Enum address, Action<T1,T2,T3> callback) => Unregister(address.GetNiceName(), callback);
        public static void Unregister<T1,T2,T3>(object address, Action<T1,T2,T3> callback)
        {
            var stringedAddress = address.ToString();
            
            if (!globalPorts.ContainsKey(stringedAddress)) return;
            ((Port<T1, T2, T3>)globalPorts[stringedAddress]).Unregister(callback);
        }
        public static void Unregister<T1,T2,T3,T4>(Enum address, Action<T1,T2,T3,T4> callback) => Unregister(address.GetNiceName(), callback);
        public static void Unregister<T1,T2,T3,T4>(object address, Action<T1,T2,T3,T4> callback)
        {
            var stringedAddress = address.ToString();
            
            if (!globalPorts.ContainsKey(stringedAddress)) return;
            ((Port<T1, T2, T3, T4>)globalPorts[stringedAddress]).Unregister(callback);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        public static void Unregister(Enum address, object key, Action callback) => Unregister(address.GetNiceName(), key, callback);
        public static void Unregister(object address, object key, Action callback)
        {
            var stringedAddress = address.ToString();
            
            if (!localPorts.ContainsKey(stringedAddress)) return;
            if (!localPorts[stringedAddress].ContainsKey(key)) return;
            
            ((VoidPort)localPorts[stringedAddress][key]).Unregister(callback);
        }
        public static void Unregister<T>(Enum address, object key, Action<T> callback) => Unregister(address.GetNiceName(), key, callback);
        public static void Unregister<T>(object address, object key, Action<T> callback)
        {
            var stringedAddress = address.ToString();
            
            if (!localPorts.ContainsKey(stringedAddress)) return;
            if (!localPorts[stringedAddress].ContainsKey(key)) return;
            
            ((Port<T>)localPorts[stringedAddress][key]).Unregister(callback);
        }
        public static void Unregister<T1,T2>(Enum address, object key, Action<T1,T2> callback) => Unregister(address.GetNiceName(), key, callback);
        public static void Unregister<T1,T2>(object address, object key, Action<T1,T2> callback)
        {
            var stringedAddress = address.ToString();
            
            if (!localPorts.ContainsKey(stringedAddress)) return;
            if (!localPorts[stringedAddress].ContainsKey(key)) return;

            ((Port<T1,T2>)localPorts[stringedAddress][key]).Unregister(callback);
        }
        public static void Unregister<T1,T2,T3>(Enum address, object key, Action<T1,T2,T3> callback) => Unregister(address.GetNiceName(), key, callback);
        public static void Unregister<T1,T2,T3>(object address, object key, Action<T1,T2,T3> callback)
        {
            var stringedAddress = address.ToString();
            
            if (!localPorts.ContainsKey(stringedAddress)) return;
            if (!localPorts[stringedAddress].ContainsKey(key)) return;

            ((Port<T1,T2,T3>)localPorts[stringedAddress][key]).Unregister(callback);
        }
        public static void Unregister<T1,T2,T3,T4>(Enum address, object key, Action<T1,T2,T3,T4> callback) => Unregister(address.GetNiceName(), key, callback);
        public static void Unregister<T1,T2,T3,T4>(object address, object key, Action<T1,T2,T3,T4> callback)
        {
            var stringedAddress = address.ToString();
            
            if (!localPorts.ContainsKey(stringedAddress)) return;
            if (!localPorts[stringedAddress].ContainsKey(key)) return;

            ((Port<T1,T2,T3,T4>)localPorts[stringedAddress][key]).Unregister(callback);
        }
    }
}

