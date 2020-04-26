namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class DfsVertexStep
    {
        public static DfsVertexStep<TVertex> Create<TVertex>(DfsStepKind kind, TVertex vertex) =>
            new DfsVertexStep<TVertex>(kind, vertex);

        internal static string StepToString(string kind, string vertex)
        {
            var s = new StringBuilder();
            s.Append('[');
            s.Append(kind);
            s.Append(", ");

            if (vertex != null)
                s.Append(vertex);

            s.Append(']');
            return s.ToString();
        }
    }

    public readonly struct DfsVertexStep<TVertex> : IEquatable<DfsVertexStep<TVertex>>
    {
        public DfsStepKind Kind { get; }
        public TVertex Vertex { get; }

        public DfsVertexStep(DfsStepKind kind, TVertex vertex)
        {
            Kind = kind;
            Vertex = vertex;
        }

        public override string ToString() =>
            DfsVertexStep.StepToString(Kind.ToString(), Vertex.ToString());

        public bool Equals(DfsVertexStep<TVertex> other) =>
            Kind == other.Kind && EqualityComparer<TVertex>.Default.Equals(Vertex, other.Vertex);

        public override bool Equals(object obj) => obj is DfsVertexStep<TVertex> other && Equals(other);

        public override int GetHashCode() =>
            unchecked((int)Kind * 397) ^ EqualityComparer<TVertex>.Default.GetHashCode(Vertex);

        public static bool operator ==(DfsVertexStep<TVertex> left, DfsVertexStep<TVertex> right) =>
            left.Equals(right);

        public static bool operator !=(DfsVertexStep<TVertex> left, DfsVertexStep<TVertex> right) =>
            !left.Equals(right);
    }
}
