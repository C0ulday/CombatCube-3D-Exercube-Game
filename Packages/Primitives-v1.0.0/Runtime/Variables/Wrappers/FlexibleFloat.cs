namespace Koboldgames.Primitives.Variables
{
    using System;

    [Serializable]
    public sealed class FlexibleFloat : FlexibleVariableBase<float, SharedFloatBase>
    {
        public FlexibleFloat(float localValue = default(float))
        {
            this.localValue = localValue;
        }

        #region Comparison And Equality

        /// <summary>
        /// Compares this instance to a specified single-precision floating-point number and returns
        /// an integer that indicates whether the value of this instance is less than, equal to, or
        /// greater than the value of the specified single-precision floating-point number.
        /// </summary>
        /// <param name="other">A single-precision floating-point number to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(float other) => Value.CompareTo(other);

        /// <summary>
        /// Returns a value indicating whether this instance and a specified single-precision
        /// floating-point number represent the same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(float other) => Value.Equals(other);

        #endregion
    }
}
