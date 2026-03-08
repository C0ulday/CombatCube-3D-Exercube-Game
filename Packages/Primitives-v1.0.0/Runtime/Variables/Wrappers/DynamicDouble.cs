namespace Koboldgames.Primitives.Variables
{
    using System;

    [Serializable]
    public sealed class DynamicDouble : DynamicVariableBase<double, SharedDoubleBase>
    {
        public DynamicDouble(SharedDoubleBase value = null)
        {
            this.sharedValue = value;
        }

        #region Comparison And Equality

        /// <summary>
        /// Compares this instance to a specified double-precision floating-point number and returns
        /// an integer that indicates whether the value of this instance is less than, equal to, or
        /// greater than the value of the specified double-precision floating-point number.
        /// </summary>
        /// <param name="other">A double-precision floating-point number to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(double other) => Value.CompareTo(other);

        /// <summary>
        /// Returns a value indicating whether this instance and a specified double-precision
        /// floating-point number represent the same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(double other) => Value.Equals(other);

        #endregion
    }
}
