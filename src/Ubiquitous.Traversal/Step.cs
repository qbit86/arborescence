namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public static class Step
    {
        public static Step<TVertex, TEdge> Create<TVertex, TEdge>(StepKind kind, TVertex vertex, TEdge edge)
        {
            return new Step<TVertex, TEdge>(kind, vertex, edge);
        }

        internal static string StepToString(StepKind kind, object vertex, object edge)
        {
            var s = new System.Text.StringBuilder();
            s.Append('[');

            s.Append(kind.ToString());
            s.Append(", ");

            if (vertex != null)
            {
                s.Append(vertex.ToString());
            }
            s.Append(", ");

            if (edge != null)
            {
                s.Append(edge.ToString());
            }

            s.Append(']');

            return s.ToString();
        }
    }

    public struct Step<TVertex, TEdge> : IEquatable<Step<TVertex, TEdge>>
    {
        public StepKind Kind { get; }

        public TVertex Vertex { get; }

        public TEdge Edge { get; }

        public Step(StepKind kind, TVertex vertex, TEdge edge)
        {
            Kind = kind;
            Vertex = vertex;
            Edge = edge;
        }

        public override string ToString()
        {
            return Step.StepToString(Kind, Vertex, Edge);
        }

        public bool Equals(Step<TVertex, TEdge> other)
        {
            if (Kind != other.Kind)
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
            if (!(obj is Step<TVertex, TEdge>))
                return false;

            var other = (Step<TVertex, TEdge>)obj;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Kind.GetHashCode();
        }

        public static bool operator ==(Step<TVertex, TEdge> left, Step<TVertex, TEdge> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Step<TVertex, TEdge> left, Step<TVertex, TEdge> right)
        {
            return !left.Equals(right);
        }
    }
}
