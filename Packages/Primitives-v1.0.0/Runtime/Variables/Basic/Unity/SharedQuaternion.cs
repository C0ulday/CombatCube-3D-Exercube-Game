namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedQuaternion.asset", menuName = "Koboldgames/Primitives/Variables/Quaternion", order = 100)]
    public sealed class SharedQuaternion : SharedQuaternionBase, IReadableVariable<Quaternion>, IWriteableVariable<Quaternion>
    {
        [SerializeField] private Quaternion initialValue = default(Quaternion);

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Quaternion Value
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
