namespace Koboldgames.Primitives.Events
{
    using System;

    public abstract class SharedEvent<T> : SharedEventBase, ISharedEvent<T>
    {
        [NonSerialized] private T lastArg;

        protected event Action<T> inner;

        /// <summary>
        /// Invoke this shared event.
        /// </summary>
        public void Invoke(T arg)
        {
            lastArg = arg;
            inner?.Invoke(arg);
        }

        /// <summary>
        /// Dynamically invokes (late-bound) the method represented by the current delegate.
        /// </summary>
        /// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.</param>
        /// <returns>The object returned by the method represented by the delegate.</returns>
        public override object DynamicInvoke(params object[] args) => inner?.DynamicInvoke(args);

        /// <summary>
        /// Returns the invocation list of the delegate.
        /// </summary>
        /// <returns>An array of delegates representing the invocation list of the current delegate.</returns>
        public override Delegate[] GetInvocationList() => inner?.GetInvocationList();

        /// <summary>
        /// Get the last event arguments of the last invocation.
        /// </summary>
        /// <returns>A tuple containing all arguments.</returns>
        public T GetLastArgs() => lastArg;

        /// <summary>
        /// Resets this shared event. This will clear all listeners.
        /// </summary>
        public void Reset() => inner = null;

        /// <summary>
        /// Add an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void AddListener(Action<T> action) => inner += action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void RemoveListener(Action<T> action) => inner -= action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public override void RemoveListener(Delegate action) =>
            RemoveListener((Action<T>)action);

        /// <summary>
        /// Add a wrapped event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        /// <returns>The wrapped delegate that has been added as listener.</returns>
        public override Delegate AddWrappedListener(Action action)
        {
            Action<T> wrapper = (arg) => action.Invoke();
            inner += wrapper;
            return wrapper;
        }

        #region Assignment Overloads

        public static SharedEvent<T> operator +(SharedEvent<T> invoker, Action<T> action)
        {
            invoker.inner += action;
            return invoker;
        }

        public static SharedEvent<T> operator -(SharedEvent<T> invoker, Action<T> action)
        {
            invoker.inner -= action;
            return invoker;
        }

        #endregion
    }

    public abstract class SharedEvent<T0, T1> : SharedEventBase, ISharedEvent<T0, T1>
    {
        [NonSerialized] private T0 lastArg0;
        [NonSerialized] private T1 lastArg1;

        protected event Action<T0, T1> inner;

        /// <summary>
        /// Invoke this shared event.
        /// </summary>
        public void Invoke(T0 arg0, T1 arg1)
        {
            lastArg0 = arg0;
            lastArg1 = arg1;
            inner?.Invoke(arg0, arg1);
        }

        /// <summary>
        /// Dynamically invokes (late-bound) the method represented by the current delegate.
        /// </summary>
        /// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.</param>
        /// <returns>The object returned by the method represented by the delegate.</returns>
        public override object DynamicInvoke(params object[] args) => inner?.DynamicInvoke(args);

        /// <summary>
        /// Returns the invocation list of the delegate.
        /// </summary>
        /// <returns>An array of delegates representing the invocation list of the current delegate.</returns>
        public override Delegate[] GetInvocationList() => inner?.GetInvocationList();

        /// <summary>
        /// Get the last event arguments of the last invocation.
        /// </summary>
        /// <returns>A tuple containing all arguments.</returns>
        public ValueTuple<T0, T1> GetLastArgs()
        {
            return new ValueTuple<T0, T1>(
                 lastArg0,
                 lastArg1
            );
        }

        /// <summary>
        /// Resets this shared event. This will clear all listeners.
        /// </summary>
        public void Reset() => inner = null;

        /// <summary>
        /// Add an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void AddListener(Action<T0, T1> action) => inner += action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void RemoveListener(Action<T0, T1> action) => inner -= action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public override void RemoveListener(Delegate action) =>
            RemoveListener((Action<T0, T1>)action);

        /// <summary>
        /// Add a wrapped event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        /// <returns>The wrapped delegate that has been added as listener.</returns>
        public override Delegate AddWrappedListener(Action action)
        {
            Action<T0, T1> wrapper = (arg0, arg1) => action.Invoke();
            inner += wrapper;
            return wrapper;
        }

        #region Assignment Overloads

        public static SharedEvent<T0, T1> operator +(SharedEvent<T0, T1> invoker, Action<T0, T1> action)
        {
            invoker.inner += action;
            return invoker;
        }

        public static SharedEvent<T0, T1> operator -(SharedEvent<T0, T1> invoker, Action<T0, T1> action)
        {
            invoker.inner -= action;
            return invoker;
        }

        #endregion
    }

    public abstract class SharedEvent<T0, T1, T2> : SharedEventBase, ISharedEvent<T0, T1, T2>
    {
        [NonSerialized] private T0 lastArg0;
        [NonSerialized] private T1 lastArg1;
        [NonSerialized] private T2 lastArg2;

        protected event Action<T0, T1, T2> inner;

        /// <summary>
        /// Invoke this shared event.
        /// </summary>
        public void Invoke(T0 arg0, T1 arg1, T2 arg2)
        {
            lastArg0 = arg0;
            lastArg1 = arg1;
            lastArg2 = arg2;
            inner?.Invoke(arg0, arg1, arg2);
        }

        /// <summary>
        /// Dynamically invokes (late-bound) the method represented by the current delegate.
        /// </summary>
        /// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.</param>
        /// <returns>The object returned by the method represented by the delegate.</returns>
        public override object DynamicInvoke(params object[] args) => inner?.DynamicInvoke(args);

        /// <summary>
        /// Returns the invocation list of the delegate.
        /// </summary>
        /// <returns>An array of delegates representing the invocation list of the current delegate.</returns>
        public override Delegate[] GetInvocationList() => inner?.GetInvocationList();

        /// <summary>
        /// Get the last event arguments of the last invocation.
        /// </summary>
        /// <returns>A tuple containing all arguments.</returns>
        public ValueTuple<T0, T1, T2> GetLastArgs()
        {
            return new ValueTuple<T0, T1, T2>(
                 lastArg0,
                 lastArg1,
                 lastArg2
            );
        }

        /// <summary>
        /// Resets this shared event. This will clear all listeners.
        /// </summary>
        public void Reset() => inner = null;

        /// <summary>
        /// Add an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void AddListener(Action<T0, T1, T2> action) => inner += action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void RemoveListener(Action<T0, T1, T2> action) => inner -= action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public override void RemoveListener(Delegate action) =>
            RemoveListener((Action<T0, T1, T2>)action);

        /// <summary>
        /// Add a wrapped event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        /// <returns>The wrapped delegate that has been added as listener.</returns>
        public override Delegate AddWrappedListener(Action action)
        {
            Action<T0, T1, T2> wrapper = (arg0, arg1, arg2) => action.Invoke();
            inner += wrapper;
            return wrapper;
        }

        #region Assignment Overloads

        public static SharedEvent<T0, T1, T2> operator +(SharedEvent<T0, T1, T2> invoker, Action<T0, T1, T2> action)
        {
            invoker.inner += action;
            return invoker;
        }

        public static SharedEvent<T0, T1, T2> operator -(SharedEvent<T0, T1, T2> invoker, Action<T0, T1, T2> action)
        {
            invoker.inner -= action;
            return invoker;
        }

        #endregion
    }

    public abstract class SharedEvent<T0, T1, T2, T3> : SharedEventBase, ISharedEvent<T0, T1, T2, T3>
    {
        [NonSerialized] private T0 lastArg0;
        [NonSerialized] private T1 lastArg1;
        [NonSerialized] private T2 lastArg2;
        [NonSerialized] private T3 lastArg3;

        protected event Action<T0, T1, T2, T3> inner;

        /// <summary>
        /// Invoke this shared event.
        /// </summary>
        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            lastArg0 = arg0;
            lastArg1 = arg1;
            lastArg2 = arg2;
            lastArg3 = arg3;
            inner?.Invoke(arg0, arg1, arg2, arg3);
        }

        /// <summary>
        /// Dynamically invokes (late-bound) the method represented by the current delegate.
        /// </summary>
        /// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.</param>
        /// <returns>The object returned by the method represented by the delegate.</returns>
        public override object DynamicInvoke(params object[] args) => inner?.DynamicInvoke(args);

        /// <summary>
        /// Returns the invocation list of the delegate.
        /// </summary>
        /// <returns>An array of delegates representing the invocation list of the current delegate.</returns>
        public override Delegate[] GetInvocationList() => inner?.GetInvocationList();

        /// <summary>
        /// Get the last event arguments of the last invocation.
        /// </summary>
        /// <returns>A tuple containing all arguments.</returns>
        public ValueTuple<T0, T1, T2, T3> GetLastArgs()
        {
            return new ValueTuple<T0, T1, T2, T3>(
                 lastArg0,
                 lastArg1,
                 lastArg2,
                 lastArg3
            );
        }

        /// <summary>
        /// Resets this shared event. This will clear all listeners.
        /// </summary>
        public void Reset() => inner = null;

        /// <summary>
        /// Add an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void AddListener(Action<T0, T1, T2, T3> action) => inner += action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void RemoveListener(Action<T0, T1, T2, T3> action) => inner -= action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public override void RemoveListener(Delegate action) =>
            RemoveListener((Action<T0, T1, T2, T3>)action);

        /// <summary>
        /// Add a wrapped event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        /// <returns>The wrapped delegate that has been added as listener.</returns>
        public override Delegate AddWrappedListener(Action action)
        {
            Action<T0, T1, T2, T3> wrapper = (arg0, arg1, arg2, arg3) => action.Invoke();
            inner += wrapper;
            return wrapper;
        }

        #region Assignment Overloads

        public static SharedEvent<T0, T1, T2, T3> operator +(SharedEvent<T0, T1, T2, T3> invoker, Action<T0, T1, T2, T3> action)
        {
            invoker.inner += action;
            return invoker;
        }

        public static SharedEvent<T0, T1, T2, T3> operator -(SharedEvent<T0, T1, T2, T3> invoker, Action<T0, T1, T2, T3> action)
        {
            invoker.inner -= action;
            return invoker;
        }

        #endregion
    }

    public abstract class SharedEvent<T0, T1, T2, T3, T4> : SharedEventBase, ISharedEvent<T0, T1, T2, T3, T4>
    {
        [NonSerialized] private T0 lastArg0;
        [NonSerialized] private T1 lastArg1;
        [NonSerialized] private T2 lastArg2;
        [NonSerialized] private T3 lastArg3;
        [NonSerialized] private T4 lastArg4;

        protected event Action<T0, T1, T2, T3, T4> inner;

        /// <summary>
        /// Invoke this shared event.
        /// </summary>
        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            lastArg0 = arg0;
            lastArg1 = arg1;
            lastArg2 = arg2;
            lastArg3 = arg3;
            lastArg4 = arg4;
            inner?.Invoke(arg0, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Dynamically invokes (late-bound) the method represented by the current delegate.
        /// </summary>
        /// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.</param>
        /// <returns>The object returned by the method represented by the delegate.</returns>
        public override object DynamicInvoke(params object[] args) => inner?.DynamicInvoke(args);

        /// <summary>
        /// Returns the invocation list of the delegate.
        /// </summary>
        /// <returns>An array of delegates representing the invocation list of the current delegate.</returns>
        public override Delegate[] GetInvocationList() => inner?.GetInvocationList();

        /// <summary>
        /// Get the last event arguments of the last invocation.
        /// </summary>
        /// <returns>A tuple containing all arguments.</returns>
        public ValueTuple<T0, T1, T2, T3, T4> GetLastArgs()
        {
            return new ValueTuple<T0, T1, T2, T3, T4>(
                 lastArg0,
                 lastArg1,
                 lastArg2,
                 lastArg3,
                 lastArg4
            );
        }

        /// <summary>
        /// Resets this shared event. This will clear all listeners.
        /// </summary>
        public void Reset() => inner = null;

        /// <summary>
        /// Add an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void AddListener(Action<T0, T1, T2, T3, T4> action) => inner += action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void RemoveListener(Action<T0, T1, T2, T3, T4> action) => inner -= action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public override void RemoveListener(Delegate action) =>
            RemoveListener((Action<T0, T1, T2, T3, T4>)action);

        /// <summary>
        /// Add a wrapped event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        /// <returns>The wrapped delegate that has been added as listener.</returns>
        public override Delegate AddWrappedListener(Action action)
        {
            Action<T0, T1, T2, T3, T4> wrapper = (arg0, arg1, arg2, arg3, arg4) => action.Invoke();
            inner += wrapper;
            return wrapper;
        }

        #region Assignment Overloads

        public static SharedEvent<T0, T1, T2, T3, T4> operator +(SharedEvent<T0, T1, T2, T3, T4> invoker, Action<T0, T1, T2, T3, T4> action)
        {
            invoker.inner += action;
            return invoker;
        }

        public static SharedEvent<T0, T1, T2, T3, T4> operator -(SharedEvent<T0, T1, T2, T3, T4> invoker, Action<T0, T1, T2, T3, T4> action)
        {
            invoker.inner -= action;
            return invoker;
        }

        #endregion
    }

    public abstract class SharedEvent<T0, T1, T2, T3, T4, T5> : SharedEventBase, ISharedEvent<T0, T1, T2, T3, T4, T5>
    {
        [NonSerialized] private T0 lastArg0;
        [NonSerialized] private T1 lastArg1;
        [NonSerialized] private T2 lastArg2;
        [NonSerialized] private T3 lastArg3;
        [NonSerialized] private T4 lastArg4;
        [NonSerialized] private T5 lastArg5;

        protected event Action<T0, T1, T2, T3, T4, T5> inner;

        /// <summary>
        /// Invoke this shared event.
        /// </summary>
        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            lastArg0 = arg0;
            lastArg1 = arg1;
            lastArg2 = arg2;
            lastArg3 = arg3;
            lastArg4 = arg4;
            lastArg5 = arg5;
            inner?.Invoke(arg0, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Dynamically invokes (late-bound) the method represented by the current delegate.
        /// </summary>
        /// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.</param>
        /// <returns>The object returned by the method represented by the delegate.</returns>
        public override object DynamicInvoke(params object[] args) => inner?.DynamicInvoke(args);

        /// <summary>
        /// Returns the invocation list of the delegate.
        /// </summary>
        /// <returns>An array of delegates representing the invocation list of the current delegate.</returns>
        public override Delegate[] GetInvocationList() => inner?.GetInvocationList();

        /// <summary>
        /// Get the last event arguments of the last invocation.
        /// </summary>
        /// <returns>A tuple containing all arguments.</returns>
        public ValueTuple<T0, T1, T2, T3, T4, T5> GetLastArgs()
        {
            return new ValueTuple<T0, T1, T2, T3, T4, T5>(
                 lastArg0,
                 lastArg1,
                 lastArg2,
                 lastArg3,
                 lastArg4,
                 lastArg5
            );
        }

        /// <summary>
        /// Resets this shared event. This will clear all listeners.
        /// </summary>
        public void Reset() => inner = null;

        /// <summary>
        /// Add an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void AddListener(Action<T0, T1, T2, T3, T4, T5> action) => inner += action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void RemoveListener(Action<T0, T1, T2, T3, T4, T5> action) => inner -= action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public override void RemoveListener(Delegate action) =>
            RemoveListener((Action<T0, T1, T2, T3, T4, T5>)action);

        /// <summary>
        /// Add a wrapped event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        /// <returns>The wrapped delegate that has been added as listener.</returns>
        public override Delegate AddWrappedListener(Action action)
        {
            Action<T0, T1, T2, T3, T4, T5> wrapper = (arg0, arg1, arg2, arg3, arg4, arg5) => action.Invoke();
            inner += wrapper;
            return wrapper;
        }

        #region Assignment Overloads

        public static SharedEvent<T0, T1, T2, T3, T4, T5> operator +(SharedEvent<T0, T1, T2, T3, T4, T5> invoker, Action<T0, T1, T2, T3, T4, T5> action)
        {
            invoker.inner += action;
            return invoker;
        }

        public static SharedEvent<T0, T1, T2, T3, T4, T5> operator -(SharedEvent<T0, T1, T2, T3, T4, T5> invoker, Action<T0, T1, T2, T3, T4, T5> action)
        {
            invoker.inner -= action;
            return invoker;
        }

        #endregion
    }

    public abstract class SharedEvent<T0, T1, T2, T3, T4, T5, T6> : SharedEventBase, ISharedEvent<T0, T1, T2, T3, T4, T5, T6>
    {
        [NonSerialized] private T0 lastArg0;
        [NonSerialized] private T1 lastArg1;
        [NonSerialized] private T2 lastArg2;
        [NonSerialized] private T3 lastArg3;
        [NonSerialized] private T4 lastArg4;
        [NonSerialized] private T5 lastArg5;
        [NonSerialized] private T6 lastArg6;

        protected event Action<T0, T1, T2, T3, T4, T5, T6> inner;

        /// <summary>
        /// Invoke this shared event.
        /// </summary>
        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            lastArg0 = arg0;
            lastArg1 = arg1;
            lastArg2 = arg2;
            lastArg3 = arg3;
            lastArg4 = arg4;
            lastArg5 = arg5;
            lastArg6 = arg6;
            inner?.Invoke(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Dynamically invokes (late-bound) the method represented by the current delegate.
        /// </summary>
        /// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.</param>
        /// <returns>The object returned by the method represented by the delegate.</returns>
        public override object DynamicInvoke(params object[] args) => inner?.DynamicInvoke(args);

        /// <summary>
        /// Returns the invocation list of the delegate.
        /// </summary>
        /// <returns>An array of delegates representing the invocation list of the current delegate.</returns>
        public override Delegate[] GetInvocationList() => inner?.GetInvocationList();

        /// <summary>
        /// Get the last event arguments of the last invocation.
        /// </summary>
        /// <returns>A tuple containing all arguments.</returns>
        public ValueTuple<T0, T1, T2, T3, T4, T5, T6> GetLastArgs()
        {
            return new ValueTuple<T0, T1, T2, T3, T4, T5, T6>(
                 lastArg0,
                 lastArg1,
                 lastArg2,
                 lastArg3,
                 lastArg4,
                 lastArg5,
                 lastArg6
            );
        }

        /// <summary>
        /// Resets this shared event. This will clear all listeners.
        /// </summary>
        public void Reset() => inner = null;

        /// <summary>
        /// Add an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void AddListener(Action<T0, T1, T2, T3, T4, T5, T6> action) => inner += action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void RemoveListener(Action<T0, T1, T2, T3, T4, T5, T6> action) => inner -= action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public override void RemoveListener(Delegate action) =>
            RemoveListener((Action<T0, T1, T2, T3, T4, T5, T6>)action);

        /// <summary>
        /// Add a wrapped event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        /// <returns>The wrapped delegate that has been added as listener.</returns>
        public override Delegate AddWrappedListener(Action action)
        {
            Action<T0, T1, T2, T3, T4, T5, T6> wrapper = (arg0, arg1, arg2, arg3, arg4, arg5, arg6) => action.Invoke();
            inner += wrapper;
            return wrapper;
        }

        #region Assignment Overloads

        public static SharedEvent<T0, T1, T2, T3, T4, T5, T6> operator +(SharedEvent<T0, T1, T2, T3, T4, T5, T6> invoker, Action<T0, T1, T2, T3, T4, T5, T6> action)
        {
            invoker.inner += action;
            return invoker;
        }

        public static SharedEvent<T0, T1, T2, T3, T4, T5, T6> operator -(SharedEvent<T0, T1, T2, T3, T4, T5, T6> invoker, Action<T0, T1, T2, T3, T4, T5, T6> action)
        {
            invoker.inner -= action;
            return invoker;
        }

        #endregion
    }

    public abstract class SharedEvent<T0, T1, T2, T3, T4, T5, T6, T7> : SharedEventBase, ISharedEvent<T0, T1, T2, T3, T4, T5, T6, T7>
    {
        [NonSerialized] private T0 lastArg0;
        [NonSerialized] private T1 lastArg1;
        [NonSerialized] private T2 lastArg2;
        [NonSerialized] private T3 lastArg3;
        [NonSerialized] private T4 lastArg4;
        [NonSerialized] private T5 lastArg5;
        [NonSerialized] private T6 lastArg6;
        [NonSerialized] private T7 lastArg7;

        protected event Action<T0, T1, T2, T3, T4, T5, T6, T7> inner;

        /// <summary>
        /// Invoke this shared event.
        /// </summary>
        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            lastArg0 = arg0;
            lastArg1 = arg1;
            lastArg2 = arg2;
            lastArg3 = arg3;
            lastArg4 = arg4;
            lastArg5 = arg5;
            lastArg6 = arg6;
            lastArg7 = arg7;
            inner?.Invoke(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Dynamically invokes (late-bound) the method represented by the current delegate.
        /// </summary>
        /// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.</param>
        /// <returns>The object returned by the method represented by the delegate.</returns>
        public override object DynamicInvoke(params object[] args) => inner?.DynamicInvoke(args);

        /// <summary>
        /// Returns the invocation list of the delegate.
        /// </summary>
        /// <returns>An array of delegates representing the invocation list of the current delegate.</returns>
        public override Delegate[] GetInvocationList() => inner?.GetInvocationList();

        /// <summary>
        /// Get the last event arguments of the last invocation.
        /// </summary>
        /// <returns>A tuple containing all arguments.</returns>
        public ValueTuple<T0, T1, T2, T3, T4, T5, T6, ValueTuple<T7>> GetLastArgs()
        {
            return new ValueTuple<T0, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>(
                 lastArg0,
                 lastArg1,
                 lastArg2,
                 lastArg3,
                 lastArg4,
                 lastArg5,
                 lastArg6,
                 new ValueTuple<T7>(lastArg7)
            );
        }

        /// <summary>
        /// Resets this shared event. This will clear all listeners.
        /// </summary>
        public void Reset() => inner = null;

        /// <summary>
        /// Add an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void AddListener(Action<T0, T1, T2, T3, T4, T5, T6, T7> action) => inner += action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void RemoveListener(Action<T0, T1, T2, T3, T4, T5, T6, T7> action) => inner -= action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public override void RemoveListener(Delegate action) =>
            RemoveListener((Action<T0, T1, T2, T3, T4, T5, T6, T7>)action);

        /// <summary>
        /// Add a wrapped event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        /// <returns>The wrapped delegate that has been added as listener.</returns>
        public override Delegate AddWrappedListener(Action action)
        {
            Action<T0, T1, T2, T3, T4, T5, T6, T7> wrapper = (arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7) => action.Invoke();
            inner += wrapper;
            return wrapper;
        }

        #region Assignment Overloads

        public static SharedEvent<T0, T1, T2, T3, T4, T5, T6, T7> operator +(SharedEvent<T0, T1, T2, T3, T4, T5, T6, T7> invoker, Action<T0, T1, T2, T3, T4, T5, T6, T7> action)
        {
            invoker.inner += action;
            return invoker;
        }

        public static SharedEvent<T0, T1, T2, T3, T4, T5, T6, T7> operator -(SharedEvent<T0, T1, T2, T3, T4, T5, T6, T7> invoker, Action<T0, T1, T2, T3, T4, T5, T6, T7> action)
        {
            invoker.inner -= action;
            return invoker;
        }

        #endregion
    }
}
