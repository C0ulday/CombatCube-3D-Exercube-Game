namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedObservableVector2Int.asset", menuName = "Koboldgames/Primitives/Variables/Observable/Vector2Int", order = 100)]
    public sealed class SharedObservableVector2Int : SharedVector2IntBase, IReadableVariable<Vector2Int>, IWriteableVariable<Vector2Int>
    {
        [SerializeField] private Vector2Int initialValue = default(Vector2Int);

        public event Action<Vector2Int> OnChange;

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector2Int Value
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
