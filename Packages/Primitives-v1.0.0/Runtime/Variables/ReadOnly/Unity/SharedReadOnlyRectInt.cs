namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyRectInt.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/RectInt", order = 100)]
    public sealed class SharedReadOnlyRectInt : SharedRectIntBase, IReadableVariable<RectInt>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public RectInt Value => value;
    }
}
