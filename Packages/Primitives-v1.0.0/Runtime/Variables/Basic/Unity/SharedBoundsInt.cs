namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedBoundsInt.asset", menuName = "Koboldgames/Primitives/Variables/BoundsInt", order = 100)]
    public sealed class SharedBoundsInt : SharedBoundsIntBase, IReadableVariable<BoundsInt>, IWriteableVariable<BoundsInt>
    {
        [SerializeField] private BoundsInt initialValue = default(BoundsInt);

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public BoundsInt Value
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
