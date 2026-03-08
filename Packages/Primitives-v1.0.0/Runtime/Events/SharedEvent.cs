namespace Koboldgames.Primitives.Events
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedEvent.asset", menuName = "Koboldgames/Primitives/Events/Basic Event", order = 100)]
    public sealed class SharedEvent : SharedEventBase, ISharedEvent
    {
        private event Action inner;

        /// <summary>
        /// Invoke this shared event.
        /// </summary>
        public void Invoke() => inner?.Invoke();

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
        /// Resets this shared event. This will clear all listeners.
        /// </summary>
        public void Reset() => inner = null;

        /// <summary>
        /// Add an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void AddListener(Action action) => inner += action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public void RemoveListener(Action action) => inner -= action;

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        public override void RemoveListener(Delegate action) => RemoveListener((Action)action);

        /// <summary>
        /// Add a wrapped event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        /// <returns>The wrapped delegate that has been added as listener.</returns>
        public override Delegate AddWrappedListener(Action action)
        {
            inner += action;
            return action;
        }

        #region Assignment Overloads

        public static SharedEvent operator +(SharedEvent invoker, Action action)
        {
            invoker.inner += action;
            return invoker;
        }

        public static SharedEvent operator -(SharedEvent invoker, Action action)
        {
            invoker.inner -= action;
            return invoker;
        }

        #endregion
    }
}
