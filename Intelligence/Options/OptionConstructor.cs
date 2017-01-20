using ReactiveAI.Intelligence.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Options
{
    public static class OptionConstructor
    {
        public static IOption Chebyshev()
        {
            var option = new Option();
            option.Measure = new Chebyshev();
            return option;
        }

        public static IOption Chebyshev(string nameId, IOptionCollection collection)
        {
            var option = new Option(nameId, collection);
            option.Measure = new Chebyshev();
            return option;
        }

        public static IOption WeightedMetrics(float pNorm = 2.0f)
        {
            var option = new Option();
            option.Measure = new WeightedMetrics(pNorm);
            return option;
        }

        public static IOption WeightedMetrics(string nameId, IOptionCollection collection,
                                              float pNorm = 2.0f)
        {
            var option = new Option(nameId, collection);
            option.Measure = new WeightedMetrics(pNorm);
            return option;
        }

        public static IOption ConstrainedChebyshev(float lowerBound = 0.0f)
        {
            var option = new Option();
            option.Measure = new ConstrainedChebyshev(lowerBound);
            return option;
        }

        public static IOption ConstrainedChebyshev(string nameId, IOptionCollection collection,
                                                   float lowerBound = 0.0f)
        {
            var option = new Option(nameId, collection);
            option.Measure = new ConstrainedChebyshev(lowerBound);
            return option;
        }

        public static IOption ConstrainedWeightedMetrics(float pNorm = 2.0f, float lowerBound = 0.0f)
        {
            var option = new Option();
            option.Measure = new ConstrainedWeightedMetrics(pNorm, lowerBound);
            return option;
        }

        public static IOption ConstrainedWeightedMetrics(string nameId, IOptionCollection collection,
                                                         float pNorm = 2.0f, float lowerBound = 0.0f)
        {
            var option = new Option(nameId, collection);
            option.Measure = new ConstrainedWeightedMetrics(pNorm, lowerBound);
            return option;
        }
    }

}
