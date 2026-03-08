namespace Koboldgames.Primitives.Variables
{
    using System;

    [Serializable]
    public sealed class FlexibleBoolean : FlexibleVariableBase<bool, SharedBooleanBase>
    {
        public FlexibleBoolean(bool localValue = default(bool))
        {
            this.localValue = localValue;
        }

        #region Comparison And Equality

        /// <summary>
        /// Compares this instance to a specified boolean and returns a new integer that indicates
        /// whether the value of this instance is less than, equal to, or greater than the value of
        /// the specified boolean.
        /// </summary>
        /// <param name="other">A boolean to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(bool other) => Value.CompareTo(other);

        /// <summary>
        /// Returns a value indicating whether this instance and a specified boolean represent the
        /// same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(bool other) => Value.Equals(other);

        #endregion
    }
}
