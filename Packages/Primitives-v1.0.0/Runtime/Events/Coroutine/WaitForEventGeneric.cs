namespace Koboldgames.Primitives.Events
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Wait for event coroutine yield instruction.
    /// </summary>
    /// <typeparam name="T">Event parameter.</typeparam>
    public class WaitForEvent<T> : CustomYieldInstruction
    {
        protected bool eventFired;
        protected Delegate reset;
        protected float startTime;
        protected float timeout;

        public override bool keepWaiting => !eventFired && (Time.time - startTime <= timeout);

        /// <summary>
        /// Wait for a specific event to fire.
        /// </summary>
        /// <param name="invoker">The event to be listened on.</param>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        public WaitForEvent(UnityEvent<T> invoker, float timeout = Single.MaxValue)
        {
            if(invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            UnityAction<T> action = (arg) => {
                this.eventFired = true;
                invoker.RemoveListener((UnityAction<T>)this.reset);
            };

            this.eventFired = false;
            this.reset = action;
            this.startTime = Time.time;
            this.timeout = timeout;
            invoker.AddListener(action);
        }

        /// <summary>
        /// Wait for a specific event to fire.
        /// </summary>
        /// <param name="invoker">The event to be listened on.</param>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        public WaitForEvent(SharedEvent<T> invoker, float timeout = Single.MaxValue)
        {
            if(invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            Action<T> action = (arg) => {
                this.eventFired = true;
                invoker.RemoveListener((Action<T>)this.reset);
            };

            this.eventFired = false;
            this.reset = action;
            this.startTime = Time.time;
            this.timeout = timeout;
            invoker.AddListener(action);
        }
    }

    /// <summary>
    /// Wait for event coroutine yield instruction.
    /// </summary>
    /// <typeparam name="T0">Event parameter.</typeparam>
    /// <typeparam name="T1">Event parameter.</typeparam>
    public class WaitForEvent<T0, T1> : CustomYieldInstruction
    {
        protected bool eventFired;
        protected Delegate reset;
        protected float startTime;
        protected float timeout;

        public override bool keepWaiting => !eventFired && (Time.time - startTime <= timeout);

        /// <summary>
        /// Wait for a specific event to fire.
        /// </summary>
        /// <param name="invoker">The event to be listened on.</param>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        public WaitForEvent(UnityEvent<T0, T1> invoker, float timeout = Single.MaxValue)
        {
            if(invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            UnityAction<T0, T1> action = (arg0, arg1) => {
                this.eventFired = true;
                invoker.RemoveListener((UnityAction<T0, T1>)this.reset);
            };

            this.eventFired = false;
            this.reset = action;
            this.startTime = Time.time;
            this.timeout = timeout;
            invoker.AddListener(action);
        }

        /// <summary>
        /// Wait for a specific event to fire.
        /// </summary>
        /// <param name="invoker">The event to be listened on.</param>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        public WaitForEvent(SharedEvent<T0, T1> invoker, float timeout = Single.MaxValue)
        {
            if(invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            Action<T0, T1> action = (arg0, arg1) => {
                this.eventFired = true;
                invoker.RemoveListener((Action<T0, T1>)this.reset);
            };

            this.eventFired = false;
            this.reset = action;
            this.startTime = Time.time;
            this.timeout = timeout;
            invoker.AddListener(action);
        }
    }

    /// <summary>
    /// Wait for event coroutine yield instruction.
    /// </summary>
    /// <typeparam name="T0">Event parameter.</typeparam>
    /// <typeparam name="T1">Event parameter.</typeparam>
    /// <typeparam name="T2">Event parameter.</typeparam>
    public class WaitForEvent<T0, T1, T2> : CustomYieldInstruction
    {
        protected bool eventFired;
        protected Delegate reset;
        protected float startTime;
        protected float timeout;

        public override bool keepWaiting => !eventFired && (Time.time - startTime <= timeout);

        /// <summary>
        /// Wait for a specific event to fire.
        /// </summary>
        /// <param name="invoker">The event to be listened on.</param>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        public WaitForEvent(UnityEvent<T0, T1, T2> invoker, float timeout = Single.MaxValue)
        {
            if(invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            UnityAction<T0, T1, T2> action = (arg0, arg1, arg2) => {
                this.eventFired = true;
                invoker.RemoveListener((UnityAction<T0, T1, T2>)this.reset);
            };

            this.eventFired = false;
            this.reset = action;
            this.startTime = Time.time;
            this.timeout = timeout;
            invoker.AddListener(action);
        }

        /// <summary>
        /// Wait for a specific event to fire.
        /// </summary>
        /// <param name="invoker">The event to be listened on.</param>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        public WaitForEvent(SharedEvent<T0, T1, T2> invoker, float timeout = Single.MaxValue)
        {
            if(invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            Action<T0, T1, T2> action = (arg0, arg1, arg2) => {
                this.eventFired = true;
                invoker.RemoveListener((Action<T0, T1, T2>)this.reset);
            };

            this.eventFired = false;
            this.reset = action;
            this.startTime = Time.time;
            this.timeout = timeout;
            invoker.AddListener(action);
        }
    }

    /// <summary>
    /// Wait for event coroutine yield instruction.
    /// </summary>
    /// <typeparam name="T0">Event parameter.</typeparam>
    /// <typeparam name="T1">Event parameter.</typeparam>
    /// <typeparam name="T2">Event parameter.</typeparam>
    /// <typeparam name="T3">Event parameter.</typeparam>
    public class WaitForEvent<T0, T1, T2, T3> : CustomYieldInstruction
    {
        protected bool eventFired;
        protected Delegate reset;
        protected float startTime;
        protected float timeout;

        public override bool keepWaiting => !eventFired && (Time.time - startTime <= timeout);

        /// <summary>
        /// Wait for a specific event to fire.
        /// </summary>
        /// <param name="invoker">The event to be listened on.</param>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        public WaitForEvent(UnityEvent<T0, T1, T2, T3> invoker, float timeout = Single.MaxValue)
        {
            if(invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            UnityAction<T0, T1, T2, T3> action = (arg0, arg1, arg2, arg3) => {
                this.eventFired = true;
                invoker.RemoveListener((UnityAction<T0, T1, T2, T3>)this.reset);
            };

            this.eventFired = false;
            this.reset = action;
            this.startTime = Time.time;
            this.timeout = timeout;
            invoker.AddListener(action);
        }

        /// <summary>
        /// Wait for a specific event to fire.
        /// </summary>
        /// <param name="invoker">The event to be listened on.</param>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        public WaitForEvent(SharedEvent<T0, T1, T2, T3> invoker, float timeout = Single.MaxValue)
        {
            if(invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            Action<T0, T1, T2, T3> action = (arg0, arg1, arg2, arg3) => {
                this.eventFired = true;
                invoker.RemoveListener((Action<T0, T1, T2, T3>)this.reset);
            };

            this.eventFired = false;
            this.reset = action;
            this.startTime = Time.time;
            this.timeout = timeout;
            invoker.AddListener(action);
        }
    }
}
