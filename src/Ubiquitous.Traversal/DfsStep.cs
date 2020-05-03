namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class DfsStep
    {
        public static DfsStep<T> Create<T>(DfsStepKind kind, T value) =>
            new DfsStep<T>(kind, value);

        internal static string StepToString(string kind, string value)
        {
            var s = new StringBuilder();
            s.Append('[');
            s.Append(kind);
            s.Append(", ");

            if (value != null)
                s.Append(value);

            s.Append(']');
            return s.ToString();
        }
    }

    public readonly struct DfsStep<T> : IEquatable<DfsStep<T>>
    {
        public DfsStepKind Kind { get; }
        public T Value { get; }

        public DfsStep(DfsStepKind kind, T value)
        {
            Kind = kind;
            Value = value;
        }

        public override string ToString() =>
            DfsStep.StepToString(Kind.ToString(), Value.ToString());

        public bool Equals(DfsStep<T> other) =>
            Kind == other.Kind && EqualityComparer<T>.Default.Equals(Value, other.Value);

        public override bool Equals(object obj) => obj is DfsStep<T> other && Equals(other);

        public override int GetHashCode() =>
            unchecked((int)Kind * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value);

        public static bool operator ==(DfsStep<T> left, DfsStep<T> right) =>
            left.Equals(right);

        public static bool operator !=(DfsStep<T> left, DfsStep<T> right) =>
            !left.Equals(right);
    }
}
