namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Primitives;

    // https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/ArraySegment.cs

    /// <inheritdoc cref="System.Collections.Generic.IEnumerator{T}"/>
#if NETSTANDARD2_1 || NETCOREAPP2_1
    [Obsolete("Please use System.ArraySegment<T>.Enumerator instead.")]
#endif
    public struct ArraySegmentEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    {
        private readonly T[] _array;
        private readonly int _start;
        private readonly int _end;
        private int _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArraySegmentEnumerator{T}"/> structure.
        /// </summary>
        /// <param name="array">The array containing the range of elements to enumerate.</param>
        /// <param name="start">The inclusive start index of the range.</param>
        /// <param name="endExclusive">The exclusive end index of the range.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        public ArraySegmentEnumerator(T[] array, int start, int endExclusive)
        {
            if (array is null || (uint)start > (uint)array.Length || (uint)endExclusive > (uint)array.Length)
                ArraySegmentEnumeratorHelper.ThrowCtorValidationFailedExceptions(array, start, endExclusive);

            _array = array;
            _start = start;
            _end = endExclusive;
            _current = start - 1;
        }

        /// <summary>
        /// Gets an empty <see cref="ArraySegmentEnumerator{T}"/> struct.
        /// </summary>
        public static ArraySegmentEnumerator<T> Empty { get; } = new ArraySegmentEnumerator<T>(Array.Empty<T>(), 0, 0);

        /// <inheritdoc/>
        public readonly T Current
        {
            get
            {
                if (_current < _start)
                    ArraySegmentEnumeratorHelper.ThrowInvalidOperationException_InvalidOperation_EnumNotStarted();
                if (_current >= _end)
                    ArraySegmentEnumeratorHelper.ThrowInvalidOperationException_InvalidOperation_EnumEnded();
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
        /// <returns>An enumerator for the range of elements.</returns>
        public readonly ArraySegmentEnumerator<T> GetEnumerator()
        {
            ArraySegmentEnumerator<T> ator = this;
            ator._current = _start - 1;
            return ator;
        }

        readonly object? IEnumerator.Current => Current;

        void IEnumerator.Reset() => _current = _start - 1;

        /// <inheritdoc/>
        public void Dispose() { }

        readonly IEnumerator IEnumerable.GetEnumerator() => this;

        readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;
    }
}
