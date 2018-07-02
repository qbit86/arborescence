namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public struct BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactory>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
    {
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        public BaselineDfsBuilder(TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
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

        public BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept, TEdgeConcept, TColorMapFactory> Create()
        {
            return new BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(VertexConcept, EdgeConcept, ColorMapFactory);
        }
    }


    public struct BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TVertexConcept, TEdgeConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
    {
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        public BaselineDfsBuilder(TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
        {
            if (vertexConcept == null)
                throw new ArgumentNullException(nameof(vertexConcept));

            if (edgeConcept == null)
                throw new ArgumentNullException(nameof(edgeConcept));

            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
        }

        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept, TEdgeConcept, TColorMapFactory> WithColorMapFactory<TColorMapFactory>()
            where TColorMapFactory : struct, IFactory<TGraph, TColorMap>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(VertexConcept, EdgeConcept, default(TColorMapFactory));
        }

        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept, TEdgeConcept, TColorMapFactory> WithColorMapFactory<TColorMapFactory>(
            TColorMapFactory colorMapFactory)
            where TColorMapFactory : IFactory<TGraph, TColorMap>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(VertexConcept, EdgeConcept, colorMapFactory);
        }
    }


    public struct BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TVertexConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
    {
        private TVertexConcept VertexConcept { get; }

        public BaselineDfsBuilder(TVertexConcept vertexConcept)
        {
            if (vertexConcept == null)
                throw new ArgumentNullException(nameof(vertexConcept));

            VertexConcept = vertexConcept;
        }

        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept, TEdgeConcept> WithEdgeConcept<TEdgeConcept>()
            where TEdgeConcept : struct, IGetTargetConcept<TGraph, TVertex, TEdge>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept>(VertexConcept, default(TEdgeConcept));
        }

        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept, TEdgeConcept> WithEdgeConcept<TEdgeConcept>(
            TEdgeConcept edgeConcept)
            where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept>(VertexConcept, edgeConcept);
        }
    }


    public struct BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
    {
        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept> WithVertexConcept<TVertexConcept>()
            where TVertexConcept : struct, IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept>(default(TVertexConcept));
        }

        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept> WithVertexConcept<TVertexConcept>(
            TVertexConcept vertexConcept)
            where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept>(vertexConcept);
        }
    }


    public struct BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap> WithColorMap<TColorMap>()
            where TColorMap : IDictionary<TVertex, Color>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>();
        }
    }


    public struct BaselineDfsBuilder<TGraph, TVertex, TEdge>
    {
        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator> WithEdgeEnumerator<TEdgeEnumerator>()
            where TEdgeEnumerator : IEnumerator<TEdge>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator>();
        }
    }


    public struct BaselineDfsBuilder<TGraph, TVertex>
    {
        public BaselineDfsBuilder<TGraph, TVertex, TEdge> WithEdge<TEdge>()
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge>();
        }
    }


    public struct BaselineDfsBuilder<TGraph>
    {
        public BaselineDfsBuilder<TGraph, TVertex> WithVertex<TVertex>()
        {
            return new BaselineDfsBuilder<TGraph, TVertex>();
        }
    }


    public struct BaselineDfsBuilder
    {
        public static BaselineDfsBuilder<TGraph> WithGraph<TGraph>()
        {
            return new BaselineDfsBuilder<TGraph>();
        }
    }
}
