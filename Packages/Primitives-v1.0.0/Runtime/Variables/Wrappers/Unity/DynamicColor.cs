namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [Serializable]
    public sealed class DynamicColor : DynamicVariableBase<Color, SharedColorBase>
    {
        public DynamicColor(SharedColorBase value = null)
        {
            this.sharedValue = value;
        }

        #region Comparison And Equality

        /// <summary>
        /// Compares this instance to a specified color and returns a new integer that indicates
        /// whether the value of this instance is less than, equal to, or greater than the value of
        /// the specified color.
        /// </summary>
        /// <param name="other">A color to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(Color other) => Value.grayscale.CompareTo(other.grayscale);

        /// <summary>
        /// Returns a value indicating whether this instance and a specified color represent the
        /// same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(Color other) => Value.Equals(other);

        #endregion
    }
}
