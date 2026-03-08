namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedVector3Int.asset", menuName = "Koboldgames/Primitives/Variables/Vector3Int", order = 100)]
    public sealed class SharedVector3Int : SharedVector3IntBase, IReadableVariable<Vector3Int>, IWriteableVariable<Vector3Int>
    {
        [SerializeField] private Vector3Int initialValue = default(Vector3Int);

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector3Int Value
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
