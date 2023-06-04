namespace Arborescence.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal sealed partial class Queue<T>
    {
        public IEnumerator<T> GetEnumerator() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void CopyTo(Array array, int index) => throw new NotSupportedException();

        public void CopyTo(T[] array, int index) => throw new NotSupportedException();

        public T[] ToArray() => throw new NotSupportedException();
    }
}
