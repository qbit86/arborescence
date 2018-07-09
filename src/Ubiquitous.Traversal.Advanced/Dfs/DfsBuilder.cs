namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public struct DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphConcept, TColorMapConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        private TGraphConcept GraphConcept { get; }

        private TColorMapConcept ColorMapConcept { get; }

        public DfsBuilder(TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            if (colorMapConcept == null)
                throw new ArgumentNullException(nameof(colorMapConcept));

            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
        }

        public Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapConcept> Create()
        {
            return new Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>(
                GraphConcept, ColorMapConcept);
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
            TGraphConcept, TColorMapConcept> WithColorMapConcept<TColorMapConcept>()
            where TColorMapConcept : struct, IMapConcept<TColorMap, TVertex, Color>, IFactory<TColorMap>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>(GraphConcept, default(TColorMapConcept));
        }

        public DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapConcept> WithColorMapConcept<TColorMapConcept>(
            TColorMapConcept colorMapConcept)
            where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TColorMap>
        {
            return new DfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>(GraphConcept, colorMapConcept);
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
