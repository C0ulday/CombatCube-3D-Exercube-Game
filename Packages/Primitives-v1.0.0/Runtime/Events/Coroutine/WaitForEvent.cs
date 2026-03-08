namespace Koboldgames.Primitives.Events
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class WaitForEvent : CustomYieldInstruction
    {
        protected bool eventFired;
        protected Delegate reset;
        protected float startTime;
        protected float timeout;

        public override bool keepWaiting => !eventFired && (Time.time - startTime <= timeout);

        /// <summary>
        /// Wait for a specific basic event to fire.
        /// </summary>
        /// <param name="invoker">The event to be listened on.</param>
        /// <param name="timeout">The time after which this instruction should timeout.</param>
        public WaitForEvent(UnityEvent invoker, float timeout = Single.MaxValue)
        {
            if(invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            UnityAction action = () => {
                this.eventFired = true;
                invoker.RemoveListener((UnityAction)this.reset);
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
        public WaitForEvent(SharedEvent invoker, float timeout = Single.MaxValue)
        {
            if(invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            Action action = () => {
                this.eventFired = true;
                invoker.RemoveListener((Action)this.reset);
            };

            this.eventFired = false;
            this.reset = action;
            this.startTime = Time.time;
            this.timeout = timeout;
            invoker.AddListener(action);
        }
    }
}
