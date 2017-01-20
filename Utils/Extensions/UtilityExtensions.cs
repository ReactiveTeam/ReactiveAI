using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public static class UtilityExtensions
    {
        /// <summary>
        ///   Chebyshev norm for the given utility list.
        /// </summary>
        /// <param name="ulist">Utility list.</param>
        public static float Chebyshev(this ICollection<Utility> ulist)
        {
            if (ulist.Count == 0)
                return 0.0f;

            var wsum = ulist.SumWeights();
            if (AIMath.AeqZero(wsum))
                return 0.0f;

            var vlist = new List<float>(ulist.Count);
            foreach (var util in ulist)
            {
                var v = util.Value * (util.Weight / wsum);
                vlist.Add(v);
            }

            var ret = vlist.Max<float>();
            return ret;
        }

        /// <summary>
        ///   Returns the p-weighted metrics norm for the given Utility list.
        /// </summary>
        /// <returns>The metrics.</returns>
        /// <param name="ulist">Utility list.</param>
        /// <param name="p">The norm</param>
        public static float WeightedMetrics(this ICollection<Utility> ulist, float p = 2.0f)
        {
            if (p < 1.0f)
                throw new PowerLessThanOneInWeightedMetricsException();

            if (ulist.Count == 0)
                return 0.0f;

            var wsum = ulist.SumWeights();
            var vlist = new List<float>(ulist.Count);
            foreach (var util in ulist)
            {
                var v = util.Weight / wsum * (float)Math.Pow(util.Value, p);
                vlist.Add(v);
            }

            var sum = vlist.Sum();
            var res = (float)Math.Pow(sum, 1 / p);
            return res;
        }

        public static float MultiplyCombined(this ICollection<Utility> ulist)
        {
            var count = ulist.Count;
            if (count == 0)
                return 0.0f;

            var res = 1.0f;
            foreach (var el in ulist)
                res *= el.Combined;

            return res;
        }

        public static float MultiplyValues(this ICollection<Utility> ulist)
        {
            var count = ulist.Count;
            if (count == 0)
                return 0.0f;

            var res = 1.0f;
            foreach (var el in ulist)
                res *= el.Value;

            return res;
        }

        public static float MultiplyWeights(this ICollection<Utility> ulist)
        {
            var count = ulist.Count;
            if (count == 0)
                return 0.0f;

            var res = 1.0f;
            foreach (var el in ulist)
                res *= el.Weight;

            return res;
        }

        public static float SumValues(this ICollection<Utility> ulist)
        {
            var count = ulist.Count;
            if (count == 0)
                return 0.0f;

            var res = 0.0f;
            foreach (var el in ulist)
                res += el.Value;

            return res;
        }

        public static float SumWeights(this ICollection<Utility> ulist)
        {
            var count = ulist.Count;
            if (count == 0)
                return 0.0f;

            var res = 0.0f;
            foreach (var el in ulist)
                res += el.Weight;

            return res;
        }

        class PowerLessThanOneInWeightedMetricsException : Exception
        {
        }
    }
}
