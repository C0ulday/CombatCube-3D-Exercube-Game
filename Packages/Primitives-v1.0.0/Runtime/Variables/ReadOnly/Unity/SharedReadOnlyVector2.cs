namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyVector2.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Vector2", order = 100)]
    public sealed class SharedReadOnlyVector2 : SharedVector2Base, IReadableVariable<Vector2>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Vector2 Value => value;
    }
}
