namespace Koboldgames.Primitives.Events
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    [Serializable]
    public class ListenerWrapper : IListener
    {
        [SerializeField] protected bool active = true;
        [SerializeField] protected SharedEventBase[] sharedEvents = new SharedEventBase[1];
        [SerializeField] protected UnitySharedEvent trigger = null;

        private Delegate[] listeners;

        /// <summary>
        /// Gets or sets the active state of this listener.
        /// </summary>
        /// <value><c>true</c> for setting this listener active; otherwise <c>false</c>.</value>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Subscribe to the serialized shared event.
        /// </summary>
        public void Subscribe()
        {
            if((listeners != null) && (listeners.Length > 0))
                Unsubscribe();

            listeners = new Delegate[sharedEvents.Length];

            for(int i = 0; i < sharedEvents.Length; i++)
            {
                if(sharedEvents[i] == null)
                    continue;

                // Copy index for boxing variable preventing outer-scope cache referencing
                int tmp = i;
                listeners[i] = sharedEvents[i].AddWrappedListener(() => OnInvoke(sharedEvents[tmp]));
            }
        }

        /// <summary>
        /// Unsubscribe from the serialized shared event.
        /// </summary>
        public void Unsubscribe()
        {
            if((listeners == null) || (listeners.Length == 0))
                return;

            for(int i = 0; i < sharedEvents.Length; i++)
            {
                if(sharedEvents[i] == null)
                    continue;

                sharedEvents[i].RemoveListener(listeners[i]);
            }

            listeners = null;
        }

        protected void OnInvoke(SharedEventBase sender)
        {
            if(active && (trigger != null))
                trigger.Invoke(sender);
        }

        [Serializable]
        protected class UnitySharedEvent : UnityEvent<SharedEventBase> { }
    }
}
