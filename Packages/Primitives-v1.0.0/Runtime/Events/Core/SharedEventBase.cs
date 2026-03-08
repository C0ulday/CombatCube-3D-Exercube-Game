namespace Koboldgames.Primitives.Events
{
    using System;
    using UnityEngine;

    [Serializable]
    public abstract class SharedEventBase : ScriptableObject
    {
        [SerializeField, Multiline] protected string description;

        /// <summary>
        /// Gets the description of this event.
        /// </summary>
        /// <value>The description of this event.</value>
        public string Description => description;

        /// <summary>
        /// Dynamically invokes (late-bound) the method represented by the current delegate.
        /// </summary>
        /// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.</param>
        /// <returns>The object returned by the method represented by the delegate.</returns>
        public abstract object DynamicInvoke(params object[] args);

        /// <summary>
        /// Returns the invocation list of the delegate.
        /// </summary>
        /// <returns>An array of delegates representing the invocation list of the current delegate.</returns>
        public abstract Delegate[] GetInvocationList();

        /// <summary>
        /// Remove an event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        /// <returns>The wrapped delegate that has been added as listener.</returns>
        public abstract void RemoveListener(Delegate action);

        /// <summary>
        /// Add a wrapped event listener delegate.
        /// </summary>
        /// <param name="action">The event listener delegate.</param>
        /// <returns>The wrapped delegate that has been added as listener.</returns>
        public abstract Delegate AddWrappedListener(Action action);
    }
}
