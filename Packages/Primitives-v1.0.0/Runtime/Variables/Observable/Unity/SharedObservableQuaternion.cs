namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedObservableQuaternion.asset", menuName = "Koboldgames/Primitives/Variables/Observable/Quaternion", order = 100)]
    public sealed class SharedObservableQuaternion : SharedQuaternionBase, IReadableVariable<Quaternion>, IWriteableVariable<Quaternion>
    {
        [SerializeField] private Quaternion initialValue = default(Quaternion);

        public event Action<Quaternion> OnChange;

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Quaternion Value
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
