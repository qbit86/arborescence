namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public struct BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphConcept, TColorMapFactory>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IMapConcept<TColorMap, TVertex, Color>, IFactory<TGraph, TColorMap>
    {
        private TGraphConcept GraphConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        public BaselineDfsBuilder(TGraphConcept graphConcept,
            TColorMapFactory colorMapFactory)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            if (colorMapFactory == null)
                throw new ArgumentNullException(nameof(colorMapFactory));

            GraphConcept = graphConcept;
            ColorMapFactory = colorMapFactory;
        }

        public BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapFactory> Create()
        {
            return new BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapFactory>(GraphConcept, ColorMapFactory);
        }
    }


    public struct BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
    {
        private TGraphConcept GraphConcept { get; }

        public BaselineDfsBuilder(TGraphConcept graphConcept)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            GraphConcept = graphConcept;
        }

        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapFactory> WithColorMapFactory<TColorMapFactory>()
            where TColorMapFactory : struct, IMapConcept<TColorMap, TVertex, Color>, IFactory<TGraph, TColorMap>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapFactory>(GraphConcept, default(TColorMapFactory));
        }

        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapFactory> WithColorMapFactory<TColorMapFactory>(
            TColorMapFactory colorMapFactory)
            where TColorMapFactory : IMapConcept<TColorMap, TVertex, Color>, IFactory<TGraph, TColorMap>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapFactory>(GraphConcept, colorMapFactory);
        }
    }


    public struct BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept> WithGraphConcept<TGraphConcept>()
            where TGraphConcept : struct, IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetConcept<TGraph, TVertex, TEdge>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept>(default(TGraphConcept));
        }

        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept> WithGraphConcept<TGraphConcept>(TGraphConcept graphConcept)
            where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetConcept<TGraph, TVertex, TEdge>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept>(graphConcept);
        }
    }


    public struct BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap> WithColorMap<TColorMap>()
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
