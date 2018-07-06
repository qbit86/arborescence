namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public struct BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphConcept, TColorMapConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TGraph, TColorMap>
    {
        private TGraphConcept GraphConcept { get; }

        private TColorMapConcept ColorMapConcept { get; }

        public BaselineDfsBuilder(TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            if (colorMapConcept == null)
                throw new ArgumentNullException(nameof(colorMapConcept));

            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
        }

        public BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapConcept> Create()
        {
            return new BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>(GraphConcept, ColorMapConcept);
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
            TGraphConcept, TColorMapConcept> WithColorMapConcept<TColorMapConcept>()
            where TColorMapConcept : struct, IMapConcept<TColorMap, TVertex, Color>, IFactory<TGraph, TColorMap>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>(GraphConcept, default(TColorMapConcept));
        }

        public BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapConcept> WithColorMapConcept<TColorMapConcept>(
            TColorMapConcept colorMapConcept)
            where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TGraph, TColorMap>
        {
            return new BaselineDfsBuilder<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>(GraphConcept, colorMapConcept);
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
