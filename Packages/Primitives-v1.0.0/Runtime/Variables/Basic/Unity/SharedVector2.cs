namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedVector2.asset", menuName = "Koboldgames/Primitives/Variables/Vector2", order = 100)]
    public sealed class SharedVector2 : SharedVector2Base, IReadableVariable<Vector2>, IWriteableVariable<Vector2>
    {
        [SerializeField] private Vector2 initialValue = default(Vector2);

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector2 Value
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
