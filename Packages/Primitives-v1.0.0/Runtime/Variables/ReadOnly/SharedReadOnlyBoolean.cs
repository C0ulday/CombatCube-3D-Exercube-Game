namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyBoolean.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Boolean", order = 100)]
    public sealed class SharedReadOnlyBoolean : SharedBooleanBase, IReadableVariable<bool>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public bool Value => value;
    }
}
