namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    // https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/ArraySegment.cs

#pragma warning disable CA1710 // Identifiers should have correct suffix
    /// <inheritdoc cref="System.Collections.Generic.IEnumerator{T}"/>
    public struct ArrayPrefixEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    {
        private readonly T[] _array;
        private readonly int _end;
        private int _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayPrefixEnumerator{T}"/> structure.
        /// </summary>
        /// <param name="array">The array which prefix to enumerate.</param>
        /// <param name="count">The number of elements in the prefix.</param>
        public ArrayPrefixEnumerator(T[] array, int count)
        {
            if (array is null || unchecked((uint)count > (uint)array.Length))
                ArrayPrefixEnumeratorHelper.ThrowCtorValidationFailedExceptions(array, count);

            _array = array;
            _end = count;
            _current = -1;
        }

        /// <inheritdoc/>
        public T Current
        {
            get
            {
                if (_current < 0)
                    ArrayPrefixEnumeratorHelper.ThrowInvalidOperationException_InvalidOperation_EnumNotStarted();
                if (_current >= _end)
                    ArrayPrefixEnumeratorHelper.ThrowInvalidOperationException_InvalidOperation_EnumEnded();
                return _array[_current];
            }
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            if (_current < _end)
            {
                _current++;
                return _current < _end;
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An enumerator for the array prefix.</returns>
        public ArrayPrefixEnumerator<T> GetEnumerator()
        {
            ArrayPrefixEnumerator<T> ator = this;
            ator._current = -1;
            return ator;
        }

        object IEnumerator.Current => Current;

        void IDisposable.Dispose() { }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this;
        }

        void IEnumerator.Reset()
        {
            _current = -1;
        }
    }
#pragma warning restore CA1710 // Identifiers should have correct suffix
}
