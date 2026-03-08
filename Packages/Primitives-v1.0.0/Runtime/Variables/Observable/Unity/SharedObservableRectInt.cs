namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedObservableRectInt.asset", menuName = "Koboldgames/Primitives/Variables/Observable/RectInt", order = 100)]
    public sealed class SharedObservableRectInt : SharedRectIntBase, IReadableVariable<RectInt>, IWriteableVariable<RectInt>
    {
        [SerializeField] private RectInt initialValue = default(RectInt);

        public event Action<RectInt> OnChange;

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public RectInt Value
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
