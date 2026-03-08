namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyVector2Int.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Vector2Int", order = 100)]
    public sealed class SharedReadOnlyVector2Int : SharedVector2IntBase, IReadableVariable<Vector2Int>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector2Int Value => value;
    }
}
