namespace Arborescence
{
    using System;
    using System.Collections.Generic;

    public readonly struct DefaultAbsence<T> : IEquatable<T>
    {
        public bool Equals(T? other) => EqualityComparer<T>.Default.Equals(other!, default!);
    }
}
