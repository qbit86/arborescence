namespace Arborescence
{
    using System;
    using System.Collections.Generic;

    public readonly struct DefaultAbsencePolicy<T> : IEquatable<T> where T : struct
    {
        public bool Equals(T other) => EqualityComparer<T>.Default.Equals(default, other);
    }
}