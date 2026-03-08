namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyLong.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Long", order = 100)]
    public sealed class SharedReadOnlyLong : SharedLongBase, IReadableVariable<long>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public long Value => value;
    }
}
