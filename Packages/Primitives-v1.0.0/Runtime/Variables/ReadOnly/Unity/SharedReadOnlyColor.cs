namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyColor.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Color", order = 100)]
    public sealed class SharedReadOnlyColor : SharedColorBase, IReadableVariable<Color>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Color Value => value;
    }
}
