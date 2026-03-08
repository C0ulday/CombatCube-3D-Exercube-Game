namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [Serializable]
    public sealed class FlexibleVector2 : FlexibleVariableBase<Vector2, SharedVector2Base>
    {
        public FlexibleVector2(Vector2 localValue = default(Vector2))
        {
            this.localValue = localValue;
        }

        #region Comparison And Equality

        /// <summary>
        /// Compares this instance to a specified vector and returns a new integer that indicates
        /// whether the value of this instance is less than, equal to, or greater than the value of
        /// the specified vector.
        /// </summary>
        /// <param name="other">A vector to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(Vector2 other) => Value.magnitude.CompareTo(other.magnitude);

        /// <summary>
        /// Returns a value indicating whether this instance and a specified vector represent the
        /// same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(Vector2 other) => Value.Equals(other);

        #endregion
    }
}
