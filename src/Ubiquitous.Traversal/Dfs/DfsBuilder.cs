namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>
        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
        where TStackFactory : IFactory<TGraph, TStack>
    {
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        private TStackFactory StackFactory { get; }

        public DfsBuilder(TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
            TColorMapFactory colorMapFactory, TStackFactory stackFactory)
        {
            if (vertexConcept == null)
                throw new ArgumentNullException(nameof(vertexConcept));

            if (edgeConcept == null)
                throw new ArgumentNullException(nameof(edgeConcept));

            if (colorMapFactory == null)
                throw new ArgumentNullException(nameof(colorMapFactory));

            if (stackFactory == null)
                throw new ArgumentNullException(nameof(stackFactory));

            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
            ColorMapFactory = colorMapFactory;
            StackFactory = stackFactory;
        }

        public Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
            TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory> Create()
        {
            return new Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>(
                VertexConcept, EdgeConcept, ColorMapFactory, StackFactory);
        }
    }


    public struct DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept, TColorMapFactory>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>
        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
    {
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        public DfsBuilder(TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
            TColorMapFactory colorMapFactory)
        {
            if (vertexConcept == null)
                throw new ArgumentNullException(nameof(vertexConcept));

            if (edgeConcept == null)
                throw new ArgumentNullException(nameof(edgeConcept));

            if (colorMapFactory == null)
                throw new ArgumentNullException(nameof(colorMapFactory));

            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
            ColorMapFactory = colorMapFactory;
        }

        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
            TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory> WithStackFactory<TStackFactory>()
            where TStackFactory : struct, IFactory<TGraph, TStack>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>(
                VertexConcept, EdgeConcept, ColorMapFactory, default(TStackFactory));
        }

        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
            TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory> WithStackFactory<TStackFactory>(
            TStackFactory stackFactory)
            where TStackFactory : IFactory<TGraph, TStack>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>(
                VertexConcept, EdgeConcept, ColorMapFactory, stackFactory);
        }
    }


    public struct DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>
        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
    {
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        public DfsBuilder(TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
        {
            if (vertexConcept == null)
                throw new ArgumentNullException(nameof(vertexConcept));

            if (edgeConcept == null)
                throw new ArgumentNullException(nameof(edgeConcept));

            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
        }

        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
            TVertexConcept, TEdgeConcept, TColorMapFactory> WithColorMapFactory<TColorMapFactory>()
            where TColorMapFactory : struct, IFactory<TGraph, TColorMap>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(VertexConcept, EdgeConcept, default(TColorMapFactory));
        }

        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
            TVertexConcept, TEdgeConcept, TColorMapFactory> WithColorMapFactory<TColorMapFactory>(
            TColorMapFactory colorMapFactory)
            where TColorMapFactory : IFactory<TGraph, TColorMap>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(VertexConcept, EdgeConcept, colorMapFactory);
        }
    }


    public struct DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>
        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
    {
        private TVertexConcept VertexConcept { get; }

        public DfsBuilder(TVertexConcept vertexConcept)
        {
            if (vertexConcept == null)
                throw new ArgumentNullException(nameof(vertexConcept));

            VertexConcept = vertexConcept;
        }

        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
            TVertexConcept, TEdgeConcept> WithEdgeConcept<TEdgeConcept>()
            where TEdgeConcept : struct, IGetTargetConcept<TGraph, TVertex, TEdge>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept>(VertexConcept, default(TEdgeConcept));
        }

        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
            TVertexConcept, TEdgeConcept> WithEdgeConcept<TEdgeConcept>(TEdgeConcept edgeConcept)
            where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept>(VertexConcept, edgeConcept);
        }
    }


    public struct DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>
    {
        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
            TVertexConcept> WithVertexConcept<TVertexConcept>()
            where TVertexConcept : struct, IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept>(default(TVertexConcept));
        }

        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
            TVertexConcept> WithVertexConcept<TVertexConcept>(TVertexConcept vertexConcept)
            where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept>(vertexConcept);
        }
    }


    public struct DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
    {
        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack> WithStack<TStack>()
            where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack>();
        }
    }


    public struct DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap> WithColorMap<TColorMap>()
            where TColorMap : IDictionary<TVertex, Color>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>();
        }
    }


    public struct DfsBuilder<TGraph, TVertex, TEdge>
    {
        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator> WithEdgeEnumerator<TEdgeEnumerator>()
            where TEdgeEnumerator : IEnumerator<TEdge>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator>();
        }
    }


    public struct DfsBuilder<TGraph, TVertex>
    {
        public DfsBuilder<TGraph, TVertex, TEdge> WithEdge<TEdge>()
        {
            return new DfsBuilder<TGraph, TVertex, TEdge>();
        }
    }


    public struct DfsBuilder<TGraph>
    {
        public DfsBuilder<TGraph, TVertex> WithVertex<TVertex>()
        {
            return new DfsBuilder<TGraph, TVertex>();
        }
    }


    public struct DfsBuilder
    {
        public static DfsBuilder<TGraph> WithGraph<TGraph>()
        {
            return new DfsBuilder<TGraph>();
        }
    }
}
