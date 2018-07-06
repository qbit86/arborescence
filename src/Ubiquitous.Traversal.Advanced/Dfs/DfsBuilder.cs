namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public struct DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphConcept, TColorMapFactory>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IMapConcept<TColorMap, TVertex, Color>, IFactory<TGraph, TColorMap>
    {
        private TGraphConcept GraphConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        public DfsBuilder(TGraphConcept graphConcept, TColorMapFactory colorMapFactory)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            if (colorMapFactory == null)
                throw new ArgumentNullException(nameof(colorMapFactory));

            GraphConcept = graphConcept;
            ColorMapFactory = colorMapFactory;
        }

        public Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapFactory> Create()
        {
            return new Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapFactory>(
                GraphConcept, ColorMapFactory);
        }
    }


    public struct DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
    {
        private TGraphConcept GraphConcept { get; }

        public DfsBuilder(TGraphConcept graphConcept)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            GraphConcept = graphConcept;
        }

        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapFactory> WithColorMapFactory<TColorMapFactory>()
            where TColorMapFactory : struct, IMapConcept<TColorMap, TVertex, Color>, IFactory<TGraph, TColorMap>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapFactory>(GraphConcept, default(TColorMapFactory));
        }

        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapFactory> WithColorMapFactory<TColorMapFactory>(
            TColorMapFactory colorMapFactory)
            where TColorMapFactory : IMapConcept<TColorMap, TVertex, Color>, IFactory<TGraph, TColorMap>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapFactory>(GraphConcept, colorMapFactory);
        }
    }


    public struct DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphConcept>
            WithGraphConcept<TGraphConcept>()
            where TGraphConcept : struct, IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetConcept<TGraph, TVertex, TEdge>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphConcept>(
                default(TGraphConcept));
        }

        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphConcept>
            WithGraphConcept<TGraphConcept>(TGraphConcept graphConcept)
            where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetConcept<TGraph, TVertex, TEdge>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphConcept>(
                graphConcept);
        }
    }


    public struct DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap> WithColorMap<TColorMap>()
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
