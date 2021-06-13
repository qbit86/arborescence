namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    // https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/ArraySegment.cs

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
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        public ArrayPrefixEnumerator(T[] array, int count)
        {
            if (array is null || (uint)count > (uint)array.Length)
                ArrayPrefixEnumeratorHelper.ThrowCtorValidationFailedExceptions(array, count);

            _array = array;
            _end = count;
            _current = -1;
        }

        /// <summary>
        /// Gets an empty <see cref="ArrayPrefixEnumerator{T}"/> struct.
        /// </summary>
        public static ArrayPrefixEnumerator<T> Empty { get; } = new ArrayPrefixEnumerator<T>(Array.Empty<T>(), 0);

        /// <inheritdoc/>
        public readonly T Current
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
        public readonly ArrayPrefixEnumerator<T> GetEnumerator()
        {
            ArrayPrefixEnumerator<T> ator = this;
            ator._current = -1;
            return ator;
        }

        object IEnumerator.Current => Current;

        void IDisposable.Dispose() { }

        IEnumerator IEnumerable.GetEnumerator() => this;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;

        void IEnumerator.Reset() => _current = -1;
    }
}
