namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyString.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/String", order = 100)]
    public sealed class SharedReadOnlyString : SharedStringBase, IReadableVariable<string>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public string Value => value;
    }
}
