namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedVector2Int.asset", menuName = "Koboldgames/Primitives/Variables/Vector2Int", order = 100)]
    public sealed class SharedVector2Int : SharedVector2IntBase, IReadableVariable<Vector2Int>, IWriteableVariable<Vector2Int>
    {
        [SerializeField] private Vector2Int initialValue = default(Vector2Int);

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector2Int Value
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
