namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    // https://github.com/dotnet/corefx/blob/master/src/System.Collections.Immutable/src/System/Collections/Immutable/ImmutableArray_1.Enumerator.cs
    public struct ImmutableArrayEnumeratorAdapter<T> : IEnumerator<T>
    {
        private ImmutableArray<T>.Enumerator _enumerator;

        public ImmutableArrayEnumeratorAdapter(ImmutableArray<T>.Enumerator enumerator)
        {
            _enumerator = enumerator;
        }

        public T Current => _enumerator.Current;

        object IEnumerator.Current => _enumerator.Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}
