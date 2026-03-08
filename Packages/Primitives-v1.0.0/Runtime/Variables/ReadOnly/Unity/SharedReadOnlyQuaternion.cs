namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyQuaternion.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Quaternion", order = 100)]
    public sealed class SharedReadOnlyQuaternion : SharedQuaternionBase, IReadableVariable<Quaternion>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Quaternion Value => value;
    }
}
