namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyFloat.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Float", order = 100)]
    public sealed class SharedReadOnlyFloat : SharedFloatBase, IReadableVariable<float>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public float Value => value;
    }
}
