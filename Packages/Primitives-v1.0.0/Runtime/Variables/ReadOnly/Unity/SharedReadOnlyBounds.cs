namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyBounds.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Bounds", order = 100)]
    public sealed class SharedReadOnlyBounds : SharedBoundsBase, IReadableVariable<Bounds>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Bounds Value => value;
    }
}
