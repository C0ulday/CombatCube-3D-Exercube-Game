namespace Koboldgames.Primitives.Events
{
    using System;
    using UnityEngine;
    using Koboldgames.NonNull;

    public class SharedEventAny : MonoBehaviour
    {
        [SerializeField, NonNull] protected SharedEventBase[] dependencies = new SharedEventBase[2];
        [SerializeField, NonNull] protected SharedEvent target;
        [SerializeField] protected bool active = true;

        private Delegate[] listeners;
        private bool[] firedState;

        /// <summary>
        /// Gets or sets the active state of this invoker.
        /// </summary>
        /// <value><c>true</c> for setting this listener active; otherwise <c>false</c>.</value>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Will reset the event tracking (reset dependency states).
        /// Reset is also called when the user hits the Reset button in the Inspector's
        /// context menu or when adding the component the first time.
        /// </summary>
        public void Reset()
        {
            if(firedState == null)
                return;

            for(int i = 0; i < firedState.Length; i++)
                firedState[i] = false;
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            active = gameObject.activeInHierarchy;

            if((firedState == null) || (firedState.Length != dependencies.Length))
            {
                firedState = new bool[dependencies.Length];
                listeners = new Delegate[dependencies.Length];
            }

            for(int i = 0; i < dependencies.Length; i++)
            {
                int tmp = i;
                firedState[i] = false;
                listeners[i] = dependencies[i].AddWrappedListener(() => {
                    bool firedAny = false;
                    firedState[tmp] = active;

                    for(int j = 0; j < firedState.Length; j++)
                        firedAny |= firedState[j];

                    if((target != null) && firedAny)
                        target.Invoke();
                });
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            if(firedState == null)
                return;

            active = gameObject.activeInHierarchy;

            for(int i = 0; i < dependencies.Length; i++)
                dependencies[i].RemoveListener(listeners[i]);
        }
    }
}
