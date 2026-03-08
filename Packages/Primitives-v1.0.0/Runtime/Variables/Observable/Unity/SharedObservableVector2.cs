namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedObservableVector2.asset", menuName = "Koboldgames/Primitives/Variables/Observable/Vector2", order = 100)]
    public sealed class SharedObservableVector2 : SharedVector2Base, IReadableVariable<Vector2>, IWriteableVariable<Vector2>
    {
        [SerializeField] private Vector2 initialValue = default(Vector2);

        public event Action<Vector2> OnChange;

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector2 Value
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
