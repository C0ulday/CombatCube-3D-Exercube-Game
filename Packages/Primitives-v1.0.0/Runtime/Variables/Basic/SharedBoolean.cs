namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedBoolean.asset", menuName = "Koboldgames/Primitives/Variables/Boolean", order = 100)]
    public sealed class SharedBoolean : SharedBooleanBase, IReadableVariable<bool>, IWriteableVariable<bool>
    {
        [SerializeField] private bool initialValue = default(bool);

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public bool Value
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
