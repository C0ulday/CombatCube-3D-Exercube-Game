namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedObservableBoolean.asset", menuName = "Koboldgames/Primitives/Variables/Observable/Boolean", order = 100)]
    public sealed class SharedObservableBoolean : SharedBooleanBase, IReadableVariable<bool>, IWriteableVariable<bool>
    {
        [SerializeField] private bool initialValue = default(bool);

        public event Action<bool> OnChange;

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public bool Value
        {
            get { return value; }
            set
            {
                if(!base.value.Equals(value))
                {
                    base.value = value;
                    OnChange?.Invoke(value);
                }
            }
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
