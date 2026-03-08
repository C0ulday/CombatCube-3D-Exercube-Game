namespace Koboldgames.Primitives.Events
{
    using System;

    public interface ISharedEvent
    {
        void Invoke();
        object DynamicInvoke(params object[] args);
        Delegate[] GetInvocationList();
        void AddListener(Action action);
        void RemoveListener(Action action);
        void RemoveListener(Delegate action);
        Delegate AddWrappedListener(Action action);
        void Reset();
    }

    public interface ISharedEvent<T>
    {
        void Invoke(T arg);
        object DynamicInvoke(params object[] args);
        Delegate[] GetInvocationList();
        T GetLastArgs();
        void AddListener(Action<T> action);
        void RemoveListener(Action<T> action);
        void RemoveListener(Delegate action);
        Delegate AddWrappedListener(Action action);
        void Reset();
    }

    public interface ISharedEvent<T0, T1>
    {
        void Invoke(T0 arg0, T1 arg1);
        object DynamicInvoke(params object[] args);
        Delegate[] GetInvocationList();
        ValueTuple<T0, T1> GetLastArgs();
        void AddListener(Action<T0, T1> action);
        void RemoveListener(Action<T0, T1> action);
        void RemoveListener(Delegate action);
        Delegate AddWrappedListener(Action action);
        void Reset();
    }

    public interface ISharedEvent<T0, T1, T2>
    {
        void Invoke(T0 arg0, T1 arg1, T2 arg2);
        object DynamicInvoke(params object[] args);
        Delegate[] GetInvocationList();
        ValueTuple<T0, T1, T2> GetLastArgs();
        void AddListener(Action<T0, T1, T2> action);
        void RemoveListener(Action<T0, T1, T2> action);
        void RemoveListener(Delegate action);
        Delegate AddWrappedListener(Action action);
        void Reset();
    }

    public interface ISharedEvent<T0, T1, T2, T3>
    {
        void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
        object DynamicInvoke(params object[] args);
        Delegate[] GetInvocationList();
        ValueTuple<T0, T1, T2, T3> GetLastArgs();
        void AddListener(Action<T0, T1, T2, T3> action);
        void RemoveListener(Action<T0, T1, T2, T3> action);
        void RemoveListener(Delegate action);
        Delegate AddWrappedListener(Action action);
        void Reset();
    }

    public interface ISharedEvent<T0, T1, T2, T3, T4>
    {
        void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
        object DynamicInvoke(params object[] args);
        Delegate[] GetInvocationList();
        ValueTuple<T0, T1, T2, T3, T4> GetLastArgs();
        void AddListener(Action<T0, T1, T2, T3, T4> action);
        void RemoveListener(Action<T0, T1, T2, T3, T4> action);
        void RemoveListener(Delegate action);
        Delegate AddWrappedListener(Action action);
        void Reset();
    }

    public interface ISharedEvent<T0, T1, T2, T3, T4, T5>
    {
        void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
        object DynamicInvoke(params object[] args);
        Delegate[] GetInvocationList();
        ValueTuple<T0, T1, T2, T3, T4, T5> GetLastArgs();
        void AddListener(Action<T0, T1, T2, T3, T4, T5> action);
        void RemoveListener(Action<T0, T1, T2, T3, T4, T5> action);
        void RemoveListener(Delegate action);
        Delegate AddWrappedListener(Action action);
        void Reset();
    }

    public interface ISharedEvent<T0, T1, T2, T3, T4, T5, T6>
    {
        void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
        object DynamicInvoke(params object[] args);
        Delegate[] GetInvocationList();
        ValueTuple<T0, T1, T2, T3, T4, T5, T6> GetLastArgs();
        void AddListener(Action<T0, T1, T2, T3, T4, T5, T6> action);
        void RemoveListener(Action<T0, T1, T2, T3, T4, T5, T6> action);
        void RemoveListener(Delegate action);
        Delegate AddWrappedListener(Action action);
        void Reset();
    }

    public interface ISharedEvent<T0, T1, T2, T3, T4, T5, T6, T7>
    {
        void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
        object DynamicInvoke(params object[] args);
        Delegate[] GetInvocationList();
        ValueTuple<T0, T1, T2, T3, T4, T5, T6, ValueTuple<T7>> GetLastArgs();
        void AddListener(Action<T0, T1, T2, T3, T4, T5, T6, T7> action);
        void RemoveListener(Action<T0, T1, T2, T3, T4, T5, T6, T7> action);
        void RemoveListener(Delegate action);
        Delegate AddWrappedListener(Action action);
        void Reset();
    }
}
