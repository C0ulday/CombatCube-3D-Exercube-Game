namespace Koboldgames.Primitives.Events
{
    using UnityEngine;

    public class SharedEventListener : MonoBehaviour
    {
        [SerializeField] private ListenerWrapper[] eventContainers = new ListenerWrapper[1];

        /// <summary>
        /// Get the event listener container object at a specific index.
        /// </summary>
        /// <param name="index">The index from the container object.</param>
        /// <returns>The listener container object.</returns>
        public IListener GetEventListener(int index) => eventContainers[index];

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            for(int i = 0; i < eventContainers.Length; i++)
            {
                eventContainers[i].Subscribe();
                eventContainers[i].Active = gameObject.activeInHierarchy;
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            for(int i = 0; i < eventContainers.Length; i++)
            {
                eventContainers[i].Unsubscribe();
                eventContainers[i].Active = gameObject.activeInHierarchy;
            }
        }
    }
}
