namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyInteger.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Integer", order = 100)]
    public sealed class SharedReadOnlyInteger : SharedIntegerBase, IReadableVariable<int>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public int Value => value;
    }
}
