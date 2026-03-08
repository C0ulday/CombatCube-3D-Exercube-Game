namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [Serializable]
    public abstract class FlexibleVariableBase<TLocal, TShared> : IReadableVariable<TLocal>, IWriteableVariable<TLocal>, IComparable, IComparable<TLocal>, IEquatable<TLocal>
        where TShared : SharedVariableBase<TLocal>
    {
        [SerializeField] protected bool useLocal = true;
        [SerializeField] protected TLocal localValue = default(TLocal);
        [SerializeField] protected TShared sharedValue = default(TShared);

        /// <summary>
        /// Gets or sets the value of this flexible variable.
        /// </summary>
        /// <value>The value of this flexible variable.</value>
        public TLocal Value
        {
            get
            {
                if(useLocal)
                    return localValue;
                else if((sharedValue != null) && (sharedValue is IReadableVariable<TLocal>))
                    return ((IReadableVariable<TLocal>)sharedValue).Value;

                return default(TLocal);
            }

            set
            {
                if(useLocal)
                    localValue = value;
                else if((sharedValue != null) && (sharedValue is IWriteableVariable<TLocal>))
                    ((IWriteableVariable<TLocal>)sharedValue).Value = value;
            }
        }

        /// <summary>
        /// Gets a value indicating if this flexible variable is using a local variable.
        /// </summary>
        /// <value><c>true</c> if a local variable is used; otherwise <c>false</c>.</value>
        public bool IsLocal => useLocal;

        /// <summary>
        /// Gets the wrapped local variable.
        /// This can be used for better caching utilization on the access side.
        /// </summary>
        /// <value>The assigned shared variable, or <c>null</c> if none is specified.</value>
        public TLocal LocalVariable => localValue;

        /// <summary>
        /// Gets the wrapped shared variable.
        /// This can be used for better caching utilization on the access side.
        /// </summary>
        /// <value>The assigned shared variable, or <c>null</c> if none is specified.</value>
        public TShared SharedVariable => sharedValue;

        /// <summary>
        /// Resets the value of the shared variable to its initial value.
        /// </summary>
        public void Reset()
        {
            if(sharedValue != null)
                (sharedValue as IWriteableVariable<TLocal>)?.Reset();
        }

        #region Comparison And Equality

        /// <summary>
        /// Compares this instance with a specified object and indicates whether this instance
        /// precedes, follows, or appears in the same position in the sort order as the specified
        /// object.
        /// </summary>
        /// <param name="obj">An object that evaluates to <see cref="T"/>.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>obj</c> parameter.
        /// </returns>
        public int CompareTo(object obj)
        {
            var v = obj as IReadableVariable<TLocal>;

            if(v == null)
                return 1;

            if((Value == null) && (v.Value == null))
                return 0;

            if(Value.Equals(v.Value))
                return 0;

            return -1;
        }

        public abstract int CompareTo(TLocal other);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var v = obj as IReadableVariable<TLocal>;

            if(v == null)
                return false;

            if((Value == null) && (v.Value == null))
                return true;

            return Value.Equals(v.Value);
        }

        public abstract bool Equals(TLocal other);

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => Value.GetHashCode();

        #endregion

        #region Convert/Casting Overloads

        public static implicit operator TLocal(FlexibleVariableBase<TLocal, TShared> variable)
        {
            if(variable != null)
                return variable.Value;
            else
                throw new Exception("Cannot implicitly convert a null reference to a value of the underlying type");
        }

        #endregion
    }
}
