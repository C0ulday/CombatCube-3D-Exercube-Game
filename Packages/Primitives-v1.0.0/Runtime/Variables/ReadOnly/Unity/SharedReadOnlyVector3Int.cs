namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyVector3Int.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Vector3Int", order = 100)]
    public sealed class SharedReadOnlyVector3Int : SharedVector3IntBase, IReadableVariable<Vector3Int>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector3Int Value => value;
    }
}
