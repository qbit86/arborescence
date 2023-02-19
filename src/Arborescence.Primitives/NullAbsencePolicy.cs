namespace Arborescence
{
    using System;

    public readonly struct NullAbsencePolicy<T> : IEquatable<T>
    {
        public bool Equals(T? other) => other is null;
    }
}
