﻿namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
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

        public BaselineDfsFactory(TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
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

        BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept, TEdgeConcept, TColorMapFactory> Create()
        {
            return new BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(VertexConcept, EdgeConcept, ColorMapFactory);
        }
    }


    public struct BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TVertexConcept, TEdgeConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
    {
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        public BaselineDfsFactory(TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
        {
            if (vertexConcept == null)
                throw new ArgumentNullException(nameof(vertexConcept));

            if (edgeConcept == null)
                throw new ArgumentNullException(nameof(edgeConcept));

            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
        }

        BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept, TEdgeConcept, TColorMapFactory> WithColorMapFactory<TColorMapFactory>()
            where TColorMapFactory : struct, IFactory<TGraph, TColorMap>
        {
            return new BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(VertexConcept, EdgeConcept, default(TColorMapFactory));
        }

        BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept, TEdgeConcept, TColorMapFactory> WithColorMapFactory<TColorMapFactory>(
            TColorMapFactory colorMapFactory)
            where TColorMapFactory : IFactory<TGraph, TColorMap>
        {
            return new BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(VertexConcept, EdgeConcept, colorMapFactory);
        }
    }


    public struct BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TVertexConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
    {
        private TVertexConcept VertexConcept { get; }

        public BaselineDfsFactory(TVertexConcept vertexConcept)
        {
            if (vertexConcept == null)
                throw new ArgumentNullException(nameof(vertexConcept));

            VertexConcept = vertexConcept;
        }

        BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept, TEdgeConcept> WithEdgeConcept<TEdgeConcept>()
            where TEdgeConcept : struct, IGetTargetConcept<TGraph, TVertex, TEdge>
        {
            return new BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept>(VertexConcept, default(TEdgeConcept));
        }

        BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept, TEdgeConcept> WithEdgeConcept<TEdgeConcept>(
            TEdgeConcept edgeConcept)
            where TEdgeConcept : struct, IGetTargetConcept<TGraph, TVertex, TEdge>
        {
            return new BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept>(VertexConcept, edgeConcept);
        }
    }


    public struct BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
    {
        BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept> WithVertexConcept<TVertexConcept>()
            where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        {
            return new BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept>(default(TVertexConcept));
        }

        BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TVertexConcept> WithVertexConcept<TVertexConcept>(
            TVertexConcept vertexConcept)
            where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        {
            return new BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept>(vertexConcept);
        }
    }


    public struct BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap> WithColorMap<TColorMap>()
            where TColorMap : IDictionary<TVertex, Color>
        {
            return new BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>();
        }
    }


    public struct BaselineDfsFactory<TGraph, TVertex, TEdge>
    {
        BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator> WithEdgeEnumerator<TEdgeEnumerator>()
            where TEdgeEnumerator : IEnumerator<TEdge>
        {
            return new BaselineDfsFactory<TGraph, TVertex, TEdge, TEdgeEnumerator>();
        }
    }


    public struct BaselineDfsFactory<TGraph, TVertex>
    {
        BaselineDfsFactory<TGraph, TVertex, TEdge> WithEdge<TEdge>()
        {
            return new BaselineDfsFactory<TGraph, TVertex, TEdge>();
        }
    }


    public struct BaselineDfsFactory<TGraph>
    {
        BaselineDfsFactory<TGraph, TVertex> WithVertex<TVertex>()
        {
            return new BaselineDfsFactory<TGraph, TVertex>();
        }
    }


    public struct BaselineDfsFactory
    {
        BaselineDfsFactory<TGraph> WithGraph<TGraph>()
        {
            return new BaselineDfsFactory<TGraph>();
        }
    }
}
