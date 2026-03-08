namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedVector4.asset", menuName = "Koboldgames/Primitives/Variables/Vector4", order = 100)]
    public sealed class SharedVector4 : SharedVector4Base, IReadableVariable<Vector4>, IWriteableVariable<Vector4>
    {
        [SerializeField] private Vector4 initialValue = default(Vector4);

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector4 Value
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
