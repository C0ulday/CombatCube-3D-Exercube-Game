namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedObservableVector4.asset", menuName = "Koboldgames/Primitives/Variables/Observable/Vector4", order = 100)]
    public sealed class SharedObservableVector4 : SharedVector4Base, IReadableVariable<Vector4>, IWriteableVariable<Vector4>
    {
        [SerializeField] private Vector4 initialValue = default(Vector4);

        public event Action<Vector4> OnChange;

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector4 Value
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
