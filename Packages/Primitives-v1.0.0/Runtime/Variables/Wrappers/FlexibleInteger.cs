namespace Koboldgames.Primitives.Variables
{
    using System;

    [Serializable]
    public sealed class FlexibleInteger : FlexibleVariableBase<int, SharedIntegerBase>
    {
        public FlexibleInteger(int localValue = default(int))
        {
            this.localValue = localValue;
        }

        #region Comparison And Equality

        /// <summary>
        /// Compares this instance to a specified integer number and returns a new integer that
        /// indicates whether the value of this instance is less than, equal to, or greater than the
        /// value of the specified integer number.
        /// </summary>
        /// <param name="other">A integer number to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(int other) => Value.CompareTo(other);

        /// <summary>
        /// Returns a value indicating whether this instance and a specified integer number
        /// represent the same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(int other) => Value.Equals(other);

        #endregion
    }
}
