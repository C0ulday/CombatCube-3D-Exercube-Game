namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyRect.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Rect", order = 100)]
    public sealed class SharedReadOnlyRect : SharedRectBase, IReadableVariable<Rect>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Rect Value => value;
    }
}
