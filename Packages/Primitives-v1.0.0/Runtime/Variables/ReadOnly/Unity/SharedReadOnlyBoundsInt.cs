namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyBoundsInt.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/BoundsInt", order = 100)]
    public sealed class SharedReadOnlyBoundsInt : SharedBoundsIntBase, IReadableVariable<BoundsInt>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public BoundsInt Value => value;
    }
}
