using System;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    public class PersistentCall : PersistentCallBase
    {
        private Action action;
        
        protected override void Initialize(MethodInfo source) => action = source.CreateDelegate(typeof(Action), target) as Action;

        public override void Invoke(object[] args)
        {
            base.Invoke(args);
            action();
        }
    }

    public class PersistentCall<T> : PersistentCallBase, ILinkage
    {
        public bool[,] Linkage => linkage;
        
        [SerializeField, HideInInspector] private bool[,] linkage;

        [SerializeField, EnableIf("IsParameterOneDynamic")] private IReadable<T> parameterOne;
        
        private Action<T> action;
        
        protected override void Initialize(MethodInfo source) => action = source.CreateDelegate(typeof(Action<T>), target) as Action<T>;

        public override void Invoke(object[] args)
        {
            base.Invoke(args);

            var parameters = new object[] {parameterOne != null ? parameterOne.Read() : (object)null};
            var values = GetValues(parameters, args, linkage);

            action((T)values[0]);
        }

        public bool IsParameterOneDynamic() => IsParameterDynamic(0);
        private bool IsParameterDynamic(int index)
        {
            if (info == string.Empty) return true;
            
            var argLength = linkage.GetLength(1);
            for (var i = 0; i < argLength; i++)
            {
                if (!linkage[index, i]) continue;
                return false;
            }

            return true;
        }

        public override void Set(Object target, string method, string info)
        {
            base.Set(target, method, info);
            if (info != string.Empty) linkage = new bool[1, info.Split('/').Length];
        }
    }
    
    public class PersistentCall<T1,T2> : PersistentCallBase, ILinkage
    {
        public bool[,] Linkage => linkage;
        
        [SerializeField, HideInInspector] private bool[,] linkage;

        [SerializeField, EnableIf("IsParameterOneDynamic")] private IReadable<T1> parameterOne;
        [SerializeField, EnableIf("IsParameterTwoDynamic")] private IReadable<T2> parameterTwo;
        
        private Action<T1,T2> action;
        
        protected override void Initialize(MethodInfo source) => action = source.CreateDelegate(typeof(Action<T1,T2>), target) as Action<T1,T2>;

        public override void Invoke(object[] args)
        {
            base.Invoke(args);

            var parameters = new object[]
            {
                parameterOne != null ? parameterOne.Read() : (object)null,
                parameterTwo != null ? parameterTwo.Read() : (object)null,
            };
            var values = GetValues(parameters, args, linkage);

            action((T1)values[0], (T2)values[1]);
        }

        public bool IsParameterOneDynamic() => IsParameterDynamic(0);
        public bool IsParameterTwoDynamic() => IsParameterDynamic(1);
        private bool IsParameterDynamic(int index)
        {
            if (info == string.Empty) return true;
            
            var argLength = linkage.GetLength(1);
            for (var i = 0; i < argLength; i++)
            {
                if (!linkage[index, i]) continue;
                return false;
            }

            return true;
        }

        public override void Set(Object target, string method, string info)
        {
            base.Set(target, method, info);
            if (info != string.Empty) linkage = new bool[2, info.Split('/').Length];
        }
    }
    
    public class PersistentCall<T1,T2,T3> : PersistentCallBase, ILinkage
    {
        public bool[,] Linkage => linkage;
        
        [SerializeField, HideInInspector] private bool[,] linkage;

        [SerializeField, EnableIf("IsParameterOneDynamic")] private IReadable<T1> parameterOne;
        [SerializeField, EnableIf("IsParameterTwoDynamic")] private IReadable<T2> parameterTwo;
        [SerializeField, EnableIf("IsParameterThreeDynamic")] private IReadable<T3> parameterThree;
        
        private Action<T1,T2,T3> action;

        protected override void Initialize(MethodInfo source) => action = source.CreateDelegate(typeof(Action<T1, T2, T3>), target) as Action<T1, T2, T3>;

        public override void Invoke(object[] args)
        {
            base.Invoke(args);

            var parameters = new object[]
            {
                parameterOne != null ? parameterOne.Read() : (object)null,
                parameterTwo != null ? parameterTwo.Read() : (object)null,
                parameterThree != null ? parameterThree.Read() : (object)null,
            };
            var values = GetValues(parameters, args, linkage);

            action((T1)values[0], (T2)values[1], (T3)values[2]);
        }

        public bool IsParameterOneDynamic() => IsParameterDynamic(0);
        public bool IsParameterTwoDynamic() => IsParameterDynamic(1);
        public bool IsParameterThreeDynamic() => IsParameterDynamic(2);
        private bool IsParameterDynamic(int index)
        {
            if (info == string.Empty) return true;
            
            var argLength = linkage.GetLength(1);
            for (var i = 0; i < argLength; i++)
            {
                if (!linkage[index, i]) continue;
                return false;
            }

            return true;
        }

        public override void Set(Object target, string method, string info)
        {
            base.Set(target, method, info);
            if (info != string.Empty) linkage = new bool[3, info.Split('/').Length];
        }
    }
    
    public class PersistentCall<T1,T2,T3,T4> : PersistentCallBase, ILinkage
    {
        public bool[,] Linkage => linkage;
        
        [SerializeField, HideInInspector] private bool[,] linkage;

        [SerializeField, EnableIf("IsParameterOneDynamic")] private IReadable<T1> parameterOne;
        [SerializeField, EnableIf("IsParameterTwoDynamic")] private IReadable<T2> parameterTwo;
        [SerializeField, EnableIf("IsParameterThreeDynamic")] private IReadable<T3> parameterThree;
        [SerializeField, EnableIf("IsParameterFourDynamic")] private IReadable<T4> parameterFour;
        
        private Action<T1,T2,T3,T4> action;
        
        protected override void Initialize(MethodInfo source) => action = source.CreateDelegate(typeof(Action<T1,T2,T3,T4>), target) as Action<T1,T2,T3,T4>;

        public override void Invoke(object[] args)
        {
            base.Invoke(args);

            var parameters = new object[]
            {
                parameterOne != null ? parameterOne.Read() : (object)null,
                parameterTwo != null ? parameterTwo.Read() : (object)null,
                parameterThree != null ? parameterThree.Read() : (object)null,
                parameterFour != null ? parameterFour.Read() : (object)null,
            };
            var values = GetValues(parameters, args, linkage);
            
            action((T1)values[0], (T2)values[1], (T3)values[2], (T4)values[3]);
        }

        public bool IsParameterOneDynamic() => IsParameterDynamic(0);
        public bool IsParameterTwoDynamic() => IsParameterDynamic(1);
        public bool IsParameterThreeDynamic() => IsParameterDynamic(2);
        public bool IsParameterFourDynamic() => IsParameterDynamic(3);
        private bool IsParameterDynamic(int index)
        {
            if (info == string.Empty) return true;
            
            var argLength = linkage.GetLength(1);
            for (var i = 0; i < argLength; i++)
            {
                if (!linkage[index, i]) continue;
                return false;
            }

            return true;
        }

        public override void Set(Object target, string method, string info)
        {
            base.Set(target, method, info);
            if (info != string.Empty) linkage = new bool[4, info.Split('/').Length];
        }
    }
}