namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedObject.asset", menuName = "Koboldgames/Primitives/Variables/Object", order = 100)]
    public sealed class SharedObject : SharedObjectBase, IReadableVariable<Object>, IWriteableVariable<Object>
    {
        [SerializeField] private Object initialValue = default(Object);

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Object Value
        {
            get { return value; }
            set { base.value = value; }
        }

        /// <summary>
        /// Resets the value of the shared variable to its initial value.
        /// </summary>
        public void Reset() => value = initialValue;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable() => Reset();
    }
}
