namespace Koboldgames.Primitives.Events
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Wait for all event coroutine yield instruction.
    /// </summary>
    /// <typeparam name="T">Event parameter.</typeparam>
    public class WaitForAnyEvent<T> : CustomYieldInstruction
    {
        protected bool[] eventsFired;
        protected Delegate[] reset;
        protected float startTime;
        protected float timeout;

        public override bool keepWaiting
        {
            get
            {
                bool state = false;

                for(int i = 0; i < eventsFired.Length; i++)
                    state |= eventsFired[i];

                return !state && (Time.time - startTime <= timeout);
            }
        }

        /// <summary>
        /// Wait for all basic events to fire.
        /// </summary>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(params UnityEvent<T>[] invokers) : this(Single.MaxValue, invokers) { }

        /// <summary>
        /// Wait for all basic events to fire.
        /// </summary>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(float timeout, params UnityEvent<T>[] invokers)
        {
            if(invokers == null)
                throw new ArgumentNullException(nameof(invokers));

            this.startTime = 0f;
            this.timeout = timeout;
            this.eventsFired = new bool[invokers.Length];
            this.reset = new Delegate[invokers.Length];

            for(int i = 0; i < invokers.Length; i++)
            {
                int tmp = i;

                UnityAction<T> action = (arg) => {
                    this.eventsFired[tmp] = true;
                    invokers[tmp].RemoveListener((UnityAction<T>)this.reset[tmp]);
                };

                this.eventsFired[i] = false;
                this.reset[i] = action;
                invokers[i].AddListener(action);
            }
        }

        /// <summary>
        /// Wait for all events to fire.
        /// </summary>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(params SharedEvent<T>[] invokers) : this(Single.MaxValue, invokers) { }

        /// <summary>
        /// Wait for all events to fire.
        /// </summary>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(float timeout, params SharedEvent<T>[] invokers)
        {
            if(invokers == null)
                throw new ArgumentNullException(nameof(invokers));

            this.startTime = 0f;
            this.timeout = timeout;
            this.eventsFired = new bool[invokers.Length];
            this.reset = new Delegate[invokers.Length];

            for(int i = 0; i < invokers.Length; i++)
            {
                int tmp = i;

                Action<T> action = (arg) => {
                    this.eventsFired[tmp] = true;
                    invokers[tmp].RemoveListener((Action<T>)this.reset[tmp]);
                };

                this.eventsFired[i] = false;
                this.reset[i] = action;
                invokers[i].AddListener(action);
            }
        }
    }

    /// <summary>
    /// Wait for allevent coroutine yield instruction.
    /// </summary>
    /// <typeparam name="T0">Event parameter.</typeparam>
    /// <typeparam name="T1">Event parameter.</typeparam>
    public class WaitForAnyEvent<T0, T1> : CustomYieldInstruction
    {
        protected bool[] eventsFired;
        protected Delegate[] reset;
        protected float startTime;
        protected float timeout;

        public override bool keepWaiting
        {
            get
            {
                bool state = false;

                for(int i = 0; i < eventsFired.Length; i++)
                    state |= eventsFired[i];

                return !state && (Time.time - startTime <= timeout);
            }
        }

        /// <summary>
        /// Wait for all basic events to fire.
        /// </summary>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(params UnityEvent<T0, T1>[] invokers) : this(Single.MaxValue, invokers) { }

        /// <summary>
        /// Wait for all basic events to fire.
        /// </summary>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(float timeout, params UnityEvent<T0, T1>[] invokers)
        {
            if(invokers == null)
                throw new ArgumentNullException(nameof(invokers));

            this.startTime = 0f;
            this.timeout = timeout;
            this.eventsFired = new bool[invokers.Length];
            this.reset = new Delegate[invokers.Length];

            for(int i = 0; i < invokers.Length; i++)
            {
                int tmp = i;

                UnityAction<T0, T1> action = (arg0, arg1) => {
                    this.eventsFired[tmp] = true;
                    invokers[tmp].RemoveListener((UnityAction<T0, T1>)this.reset[tmp]);
                };

                this.eventsFired[i] = false;
                this.reset[i] = action;
                invokers[i].AddListener(action);
            }
        }

        /// <summary>
        /// Wait for all events to fire.
        /// </summary>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(params SharedEvent<T0, T1>[] invokers) : this(Single.MaxValue, invokers) { }

        /// <summary>
        /// Wait for all events to fire.
        /// </summary>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(float timeout, params SharedEvent<T0, T1>[] invokers)
        {
            if(invokers == null)
                throw new ArgumentNullException(nameof(invokers));

            this.startTime = 0f;
            this.timeout = timeout;
            this.eventsFired = new bool[invokers.Length];
            this.reset = new Delegate[invokers.Length];

            for(int i = 0; i < invokers.Length; i++)
            {
                int tmp = i;

                Action<T0, T1> action = (arg0, arg1) => {
                    this.eventsFired[tmp] = true;
                    invokers[tmp].RemoveListener((Action<T0, T1>)this.reset[tmp]);
                };

                this.eventsFired[i] = false;
                this.reset[i] = action;
                invokers[i].AddListener(action);
            }
        }
    }

    /// <summary>
    /// Wait for allevent coroutine yield instruction.
    /// </summary>
    /// <typeparam name="T0">Event parameter.</typeparam>
    /// <typeparam name="T1">Event parameter.</typeparam>
    /// <typeparam name="T2">Event parameter.</typeparam>
    public class WaitForAnyEvent<T0, T1, T2> : CustomYieldInstruction
    {
        protected bool[] eventsFired;
        protected Delegate[] reset;
        protected float startTime;
        protected float timeout;

        public override bool keepWaiting
        {
            get
            {
                bool state = false;

                for(int i = 0; i < eventsFired.Length; i++)
                    state |= eventsFired[i];

                return !state && (Time.time - startTime <= timeout);
            }
        }

        /// <summary>
        /// Wait for all basic events to fire.
        /// </summary>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(params UnityEvent<T0, T1, T2>[] invokers) : this(Single.MaxValue, invokers) { }

        /// <summary>
        /// Wait for all basic events to fire.
        /// </summary>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(float timeout, params UnityEvent<T0, T1, T2>[] invokers)
        {
            if(invokers == null)
                throw new ArgumentNullException(nameof(invokers));

            this.startTime = 0f;
            this.timeout = timeout;
            this.eventsFired = new bool[invokers.Length];
            this.reset = new Delegate[invokers.Length];

            for(int i = 0; i < invokers.Length; i++)
            {
                int tmp = i;

                UnityAction<T0, T1, T2> action = (arg0, arg1, arg2) => {
                    this.eventsFired[tmp] = true;
                    invokers[tmp].RemoveListener((UnityAction<T0, T1, T2>)this.reset[tmp]);
                };

                this.eventsFired[i] = false;
                this.reset[i] = action;
                invokers[i].AddListener(action);
            }
        }

        /// <summary>
        /// Wait for all events to fire.
        /// </summary>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(params SharedEvent<T0, T1, T2>[] invokers) : this(Single.MaxValue, invokers) { }

        /// <summary>
        /// Wait for all events to fire.
        /// </summary>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(float timeout, params SharedEvent<T0, T1, T2>[] invokers)
        {
            if(invokers == null)
                throw new ArgumentNullException(nameof(invokers));

            this.startTime = 0f;
            this.timeout = timeout;
            this.eventsFired = new bool[invokers.Length];
            this.reset = new Delegate[invokers.Length];

            for(int i = 0; i < invokers.Length; i++)
            {
                int tmp = i;

                Action<T0, T1, T2> action = (arg0, arg1, arg2) => {
                    this.eventsFired[tmp] = true;
                    invokers[tmp].RemoveListener((Action<T0, T1, T2>)this.reset[tmp]);
                };

                this.eventsFired[i] = false;
                this.reset[i] = action;
                invokers[i].AddListener(action);
            }
        }
    }

    /// <summary>
    /// Wait for allevent coroutine yield instruction.
    /// </summary>
    /// <typeparam name="T0">Event parameter.</typeparam>
    /// <typeparam name="T1">Event parameter.</typeparam>
    /// <typeparam name="T2">Event parameter.</typeparam>
    /// <typeparam name="T3">Event parameter.</typeparam>
    public class WaitForAnyEvent<T0, T1, T2, T3> : CustomYieldInstruction
    {
        protected bool[] eventsFired;
        protected Delegate[] reset;
        protected float startTime;
        protected float timeout;

        public override bool keepWaiting
        {
            get
            {
                bool state = false;

                for(int i = 0; i < eventsFired.Length; i++)
                    state |= eventsFired[i];

                return !state && (Time.time - startTime <= timeout);
            }
        }

        /// <summary>
        /// Wait for all basic events to fire.
        /// </summary>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(params UnityEvent<T0, T1, T2, T3>[] invokers) : this(Single.MaxValue, invokers) { }

        /// <summary>
        /// Wait for all basic events to fire.
        /// </summary>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(float timeout, params UnityEvent<T0, T1, T2, T3>[] invokers)
        {
            if(invokers == null)
                throw new ArgumentNullException(nameof(invokers));

            this.startTime = 0f;
            this.timeout = timeout;
            this.eventsFired = new bool[invokers.Length];
            this.reset = new Delegate[invokers.Length];

            for(int i = 0; i < invokers.Length; i++)
            {
                int tmp = i;

                UnityAction<T0, T1, T2, T3> action = (arg0, arg1, arg2, arg3) => {
                    this.eventsFired[tmp] = true;
                    invokers[tmp].RemoveListener((UnityAction<T0, T1, T2, T3>)this.reset[tmp]);
                };

                this.eventsFired[i] = false;
                this.reset[i] = action;
                invokers[i].AddListener(action);
            }
        }

        /// <summary>
        /// Wait for all events to fire.
        /// </summary>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(params SharedEvent<T0, T1, T2, T3>[] invokers) : this(Single.MaxValue, invokers) { }

        /// <summary>
        /// Wait for all events to fire.
        /// </summary>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAnyEvent(float timeout, params SharedEvent<T0, T1, T2, T3>[] invokers)
        {
            if(invokers == null)
                throw new ArgumentNullException(nameof(invokers));

            this.startTime = 0f;
            this.timeout = timeout;
            this.eventsFired = new bool[invokers.Length];
            this.reset = new Delegate[invokers.Length];

            for(int i = 0; i < invokers.Length; i++)
            {
                int tmp = i;

                Action<T0, T1, T2, T3> action = (arg0, arg1, arg2, arg3) => {
                    this.eventsFired[tmp] = true;
                    invokers[tmp].RemoveListener((Action<T0, T1, T2, T3>)this.reset[tmp]);
                };

                this.eventsFired[i] = false;
                this.reset[i] = action;
                invokers[i].AddListener(action);
            }
        }
    }
}
