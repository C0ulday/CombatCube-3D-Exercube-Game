namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyVector4.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Vector4", order = 100)]
    public sealed class SharedReadOnlyVector4 : SharedVector4Base, IReadableVariable<Vector4>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector4 Value => value;
    }
}
