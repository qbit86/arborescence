namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text;

    /// <summary>
    /// Provides the Create factory method for Endpoints&lt;TVertex&gt;.
    /// </summary>
    public static class Endpoints
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Endpoints<TVertex> Create<TVertex>(TVertex source, TVertex target)
        {
            return new Endpoints<TVertex>(source, target);
        }

        /// <summary>
        /// Used by Endpoints.ToString to reduce generic code.
        /// </summary>
        internal static string PairToString(string source, string target)
        {
            var s = new StringBuilder();
            s.Append('[');

            if (source != null)
                s.Append(source);

            s.Append(", ");

            if (target != null)
                s.Append(target);

            s.Append(']');

            return s.ToString();
        }
    }

    /// <summary>
    /// Holds endpoints of an oriented edge.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex descriptors.</typeparam>
    /// <remarks>
    /// See also:
    /// <a href="https://github.com/dotnet/coreclr/blob/master/src/mscorlib/shared/System/Collections/Generic/KeyValuePair.cs">KeyValuePair.cs</a>
    /// </remarks>
    public readonly struct Endpoints<TVertex> : IEquatable<Endpoints<TVertex>>
    {
        public TVertex Tail { get; }
        public TVertex Head { get; }

        public Endpoints(TVertex tail, TVertex head)
        {
            Tail = tail;
            Head = head;
        }

        public override string ToString()
        {
            return Endpoints.PairToString(Tail?.ToString(), Head?.ToString());
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out TVertex source, out TVertex target)
        {
            source = Tail;
            target = Head;
        }

        public bool Equals(Endpoints<TVertex> other)
        {
            EqualityComparer<TVertex> comparer = EqualityComparer<TVertex>.Default;

            if (!comparer.Equals(Tail, other.Tail))
                return false;

            if (!comparer.Equals(Head, other.Head))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Endpoints<TVertex> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return unchecked(EqualityComparer<TVertex>.Default.GetHashCode(Tail) * 397) ^
                EqualityComparer<TVertex>.Default.GetHashCode(Head);
        }

        public static bool operator ==(Endpoints<TVertex> left, Endpoints<TVertex> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Endpoints<TVertex> left, Endpoints<TVertex> right)
        {
            return !left.Equals(right);
        }
    }
}
