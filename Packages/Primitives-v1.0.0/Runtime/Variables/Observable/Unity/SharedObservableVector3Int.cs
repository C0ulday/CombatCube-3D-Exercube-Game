namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedObservableVector3Int.asset", menuName = "Koboldgames/Primitives/Variables/Observable/Vector3Int", order = 100)]
    public sealed class SharedObservableVector3Int : SharedVector3IntBase, IReadableVariable<Vector3Int>, IWriteableVariable<Vector3Int>
    {
        [SerializeField] private Vector3Int initialValue = default(Vector3Int);

        public event Action<Vector3Int> OnChange;

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector3Int Value
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
