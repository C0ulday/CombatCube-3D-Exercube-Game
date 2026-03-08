namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedObservableDouble.asset", menuName = "Koboldgames/Primitives/Variables/Observable/Double", order = 100)]
    public sealed class SharedObservableDouble : SharedDoubleBase, IReadableVariable<double>, IWriteableVariable<double>
    {
        [SerializeField] private double initialValue = default(double);

        public event Action<double> OnChange;

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public double Value
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
