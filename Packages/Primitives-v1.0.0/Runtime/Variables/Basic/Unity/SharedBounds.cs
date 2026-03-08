namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedBounds.asset", menuName = "Koboldgames/Primitives/Variables/Bounds", order = 100)]
    public sealed class SharedBounds : SharedBoundsBase, IReadableVariable<Bounds>, IWriteableVariable<Bounds>
    {
        [SerializeField] private Bounds initialValue = default(Bounds);

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Bounds Value
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
