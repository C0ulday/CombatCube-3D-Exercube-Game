namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedReadOnlyDouble.asset", menuName = "Koboldgames/Primitives/Variables/Read Only/Double", order = 100)]
    public sealed class SharedReadOnlyDouble : SharedDoubleBase, IReadableVariable<double>
    {
        /// <summary>
        /// Gets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public double Value => value;
    }
}
