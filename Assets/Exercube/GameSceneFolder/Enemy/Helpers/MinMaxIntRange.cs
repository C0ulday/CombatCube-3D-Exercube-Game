using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sphery.ExerCube.Helpers
{
    [Serializable]
    public struct MinMaxIntRange : IComparable, IComparable<MinMaxIntRange>, IEquatable<MinMaxIntRange>, IStructuralComparable, IStructuralEquatable
    {
        [SerializeField] private int min;
        [SerializeField] private int max;

        /// <summary>
        /// Initialize a min-max range.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public MinMaxIntRange(int min, int max)
        {
            if (min > max)
                throw new ArgumentException($"{nameof(min)} cannot be greater than {nameof(max)}!");

            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        public int Min => min;

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        public int Max => max;

        /// <summary>
        /// Generate a random value between minimum and maximum (both inclusive).
        /// </summary>
        /// <returns>A random value between minimum and maximum.</returns>
        public int GetRandom() => Random.Range(min, max + 1);

        /// <summary>
        /// Check if a given value lies between minimum and maximum (both inclusive).
        /// </summary>
        /// <param name="value">A value to check.</param>
        /// <returns><c>true</c> if the given value is in range; otherwise <c>false</c>.</returns>
        public bool InRange(int value) => (value >= min) && (value <= max);

        /// <summary>
        /// Clamp a value to the range.
        /// </summary>
        /// <param name="value">A value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public int Clamp(int value) => Mathf.Clamp(value, min, max);

        /// <summary>
        /// Implicitly casting of a <see cref="KoboldTools.MinMaxRange" instance.
        /// </summary>
        /// <param name="instance">The convertable instance.</param>
        public static implicit operator MinMaxIntRange(MinMaxRange instance)
        {
            return new MinMaxIntRange(
                Mathf.RoundToInt(instance.Min),
                Mathf.RoundToInt(instance.Max)
            );
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether
        /// the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            return CompareTo((MinMaxIntRange)obj);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether
        /// the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(MinMaxIntRange other)
        {
            float diff = max - min;
            float otherDiff = other.Max - other.Min;

            if (diff > otherDiff)
                return -1;
            else if (diff < otherDiff)
                return 1;

            return 0;
        }

        /// <summary>
        /// Determines whether the current collection object precedes, occurs in the same position as, or follows another
        /// object in the sort order.
        /// </summary>
        /// <param name="other">The object to compare with the current instance.</param>
        /// <param name="comparer">An object that compares members of the current collection object with the corresponding members of <c>other</c>.</param>
        /// <returns>An integer that indicates the relationship of the current collection object to <c>other</c>.</returns>
        public int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
                return 1;

            MinMaxIntRange otherRange = (MinMaxIntRange)other;

            float diff = max - min;
            float otherDiff = otherRange.Max - otherRange.Min;

            return comparer.Compare(diff, otherDiff);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return Equals((MinMaxIntRange)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <c>other</c> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(MinMaxIntRange other)
        {
            return Mathf.Approximately(min, other.Min) &&
                   Mathf.Approximately(max, other.Max);
        }

        /// <summary>
        /// Determines whether an object is structurally equal to the current instance.
        /// </summary>
        /// <param name="other">The object to compare with the current instance.</param>
        /// <param name="comparer">An object that determines whether the current instance and <c>other</c> are equal.</param>
        /// <returns><c>true</c> if the two objects are equal; otherwise, <c>false</c>.</returns>
        public bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null)
                return false;

            MinMaxIntRange otherRange = (MinMaxIntRange)other;

            return comparer.Equals(min, otherRange.Min) && comparer.Equals(max, otherRange.Max);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = min.GetHashCode();

                result = (result * 397) ^ max.GetHashCode();
                result = (result * 397) ^ (min - max).GetHashCode();

                return result;
            }
        }

        /// <summary>
        /// Returns a hash code for the current instance.
        /// </summary>
        /// <param name="comparer">An object that computes the hash code of the current object.</param>
        /// <returns>The hash code for the current instance.</returns>
        public int GetHashCode(IEqualityComparer comparer) => comparer.GetHashCode(max - min);
    }
}
