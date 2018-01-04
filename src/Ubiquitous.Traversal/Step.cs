namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public static class Step
    {
        public static Step<TStepKind, TVertex, TEdge> Create<TStepKind, TVertex, TEdge>(TStepKind kind, TVertex vertex, TEdge edge)
        {
            return new Step<TStepKind, TVertex, TEdge>(kind, vertex, edge);
        }

        internal static string StepToString(object kind, object vertex, object edge)
        {
            var s = new System.Text.StringBuilder();
            s.Append('[');

            s.Append(kind);
            s.Append(", ");

            if (vertex != null)
            {
                s.Append(vertex);
            }
            s.Append(", ");

            if (edge != null)
            {
                s.Append(edge);
            }

            s.Append(']');

            return s.ToString();
        }
    }

    public struct Step<TStepKind, TVertex, TEdge> : IEquatable<Step<TStepKind, TVertex, TEdge>>
    {
        public TStepKind Kind { get; }

        public TVertex Vertex { get; }

        public TEdge Edge { get; }

        public Step(TStepKind kind, TVertex vertex, TEdge edge)
        {
            Kind = kind;
            Vertex = vertex;
            Edge = edge;
        }

        public override string ToString()
        {
            return Step.StepToString(Kind, Vertex, Edge);
        }

        public bool Equals(Step<TStepKind, TVertex, TEdge> other)
        {
            EqualityComparer<TStepKind> kindComparer = EqualityComparer<TStepKind>.Default;
            if (!kindComparer.Equals(Kind, other.Kind))
                return false;

            EqualityComparer<TVertex> vertexComparer = EqualityComparer<TVertex>.Default;
            if (!vertexComparer.Equals(Vertex, other.Vertex))
                return false;

            EqualityComparer<TEdge> edgeComparer = EqualityComparer<TEdge>.Default;
            if (!edgeComparer.Equals(Edge, other.Edge))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Step<TStepKind, TVertex, TEdge>))
                return false;

            var other = (Step<TStepKind, TVertex, TEdge>)obj;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Kind.GetHashCode();
        }

        public static bool operator ==(Step<TStepKind, TVertex, TEdge> left, Step<TStepKind, TVertex, TEdge> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Step<TStepKind, TVertex, TEdge> left, Step<TStepKind, TVertex, TEdge> right)
        {
            return !left.Equals(right);
        }
    }
}
