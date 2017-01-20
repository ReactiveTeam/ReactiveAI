using ReactiveAI.Intelligence.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Considerations
{
    public static class ConsiderationConstructor
    {
        public static ICompositeConsideration Create(IMeasure measure)
        {
            var consideration = new CompositeConsideration
            {
                Measure = measure
            };
            return consideration;
        }

        public static ICompositeConsideration Create(string nameId, IConsiderationCollection collection,
                                                     IMeasure measure)
        {
            var consideration = new CompositeConsideration(nameId, collection)
            {
                Measure = measure
            };
            return consideration;
        }

        public static ICompositeConsideration Chebyshev()
        {
            var consideration = new CompositeConsideration
            {
                Measure = new Chebyshev()
            };
            return consideration;
        }

        public static ICompositeConsideration Chebyshev(string nameId, IConsiderationCollection collection)
        {
            var consideration = new CompositeConsideration(nameId, collection)
            {
                Measure = new Chebyshev()
            };
            return consideration;
        }

        public static ICompositeConsideration WeightedMetrics(float pNorm = 2.0f)
        {
            var consideration = new CompositeConsideration
            {
                Measure = new WeightedMetrics(pNorm)
            };
            return consideration;
        }

        public static ICompositeConsideration WeightedMetrics(string nameId, IConsiderationCollection collection,
                                                              float pNorm = 2.0f)
        {
            var consideration = new CompositeConsideration(nameId, collection)
            {
                Measure = new WeightedMetrics(pNorm)
            };
            return consideration;
        }

        public static ICompositeConsideration ConstrainedChebyshev(float lowerBound = 0.0f)
        {
            var consideration = new CompositeConsideration
            {
                Measure = new ConstrainedChebyshev(lowerBound)
            };
            return consideration;
        }

        public static ICompositeConsideration ConstrainedChebyshev(string nameId, IConsiderationCollection collection,
                                                                   float lowerBound = 0.0f)
        {
            var consideration = new CompositeConsideration(nameId, collection)
            {
                Measure = new ConstrainedChebyshev(lowerBound)
            };
            return consideration;
        }

        public static ICompositeConsideration ConstrainedWeightedMetrics(float pNorm = 2.0f, float lowerBound = 0.0f)
        {
            var consideration = new CompositeConsideration
            {
                Measure = new ConstrainedWeightedMetrics(pNorm, lowerBound)
            };
            return consideration;
        }

        public static ICompositeConsideration ConstrainedWeightedMetrics(string nameId, IConsiderationCollection collection,
                                                                         float pNorm = 2.0f, float lowerBound = 0.0f)
        {
            var consideration = new CompositeConsideration(nameId, collection)
            {
                Measure = new ConstrainedWeightedMetrics(pNorm, lowerBound)
            };
            return consideration;
        }

        public static ICompositeConsideration Multiplicative()
        {
            var consideration = new CompositeConsideration
            {
                Measure = new MultiplicativePseudoMeasure()
            };
            return consideration;
        }

        public static ICompositeConsideration Multiplicative(string nameId, IConsiderationCollection collection)
        {
            var consideration = new CompositeConsideration(nameId, collection)
            {
                Measure = new MultiplicativePseudoMeasure()
            };
            return consideration;
        }
    }
}
