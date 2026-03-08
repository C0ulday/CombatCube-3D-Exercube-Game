namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedInteger.asset", menuName = "Koboldgames/Primitives/Variables/Integer", order = 100)]
    public sealed class SharedInteger : SharedIntegerBase, IReadableVariable<int>, IWriteableVariable<int>
    {
        [SerializeField] private int initialValue = default(int);

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public int Value
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
