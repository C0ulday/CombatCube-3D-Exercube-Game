namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyVector3.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Vector3", order = 100)]
    public sealed class SharedReadOnlyVector3 : SharedVector3Base, IReadableVariable<Vector3>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector3 Value => value;
    }
}
