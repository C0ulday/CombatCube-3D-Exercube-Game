namespace Koboldgames.Primitives.Events
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class WaitForAllEvent : CustomYieldInstruction
    {
        protected bool[] eventsFired;
        protected Delegate[] reset;
        protected float startTime;
        protected float timeout;

        public override bool keepWaiting
        {
            get
            {
                bool state = true;

                for(int i = 0; i < eventsFired.Length; i++)
                    state &= eventsFired[i];

                return !state && (Time.time - startTime <= timeout);
            }
        }

        /// <summary>
        /// Wait for all basic events to fire.
        /// </summary>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAllEvent(params UnityEvent[] invokers) : this(Single.MaxValue, invokers) { }

        /// <summary>
        /// Wait for all basic events to fire.
        /// </summary>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAllEvent(float timeout, params UnityEvent[] invokers)
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

                UnityAction action = () => {
                    this.eventsFired[tmp] = true;
                    invokers[tmp].RemoveListener((UnityAction)this.reset[tmp]);
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
        public WaitForAllEvent(params SharedEvent[] invokers) : this(Single.MaxValue, invokers) { }

        /// <summary>
        /// Wait for all events to fire.
        /// </summary>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        /// <param name="invokers">The events to be listened on.</param>
        public WaitForAllEvent(float timeout, params SharedEvent[] invokers)
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

                Action action = () => {
                    this.eventsFired[tmp] = true;
                    invokers[tmp].RemoveListener((Action)this.reset[tmp]);
                };

                this.eventsFired[i] = false;
                this.reset[i] = action;
                invokers[i].AddListener(action);
            }
        }
    }
}
