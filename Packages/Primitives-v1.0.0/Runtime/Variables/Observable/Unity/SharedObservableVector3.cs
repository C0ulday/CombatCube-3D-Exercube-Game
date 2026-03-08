namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedObservableVector3.asset", menuName = "Koboldgames/Primitives/Variables/Observable/Vector3", order = 100)]
    public sealed class SharedObservableVector3 : SharedVector3Base, IReadableVariable<Vector3>, IWriteableVariable<Vector3>
    {
        [SerializeField] private Vector3 initialValue = default(Vector3);

        public event Action<Vector3> OnChange;

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector3 Value
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
