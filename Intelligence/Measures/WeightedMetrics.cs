using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Measures
{
    public sealed class WeightedMetrics : IMeasure
    {
        float _p;
        public readonly float PNormMin = 1.0f;
        public readonly float PNormMax = 10000.0f;

        public float PNorm
        {
            get { return _p; }
            set { _p = value.Clamp(PNormMin, PNormMax); }
        }

        public float Calculate(ICollection<Utility> elements)
        {
            var count = elements.Count;
            if (count == 0)
                return 0.0f;

            var wsum = 0.0f;
            foreach (var el in elements)
                wsum += el.Weight;

            if (AIMath.AeqZero(wsum))
                return 0.0f;

            var vlist = new List<float>(count);
            foreach (var el in elements)
            {
                var v = el.Weight / wsum * (float)Math.Pow(el.Value, _p);
                vlist.Add(v);
            }

            var sum = vlist.Sum();
            var res = (float)Math.Pow(sum, 1 / _p);

            return res;
        }

        public IMeasure Clone()
        {
            return new WeightedMetrics(PNorm);
        }

        public WeightedMetrics()
        {
            PNorm = 2.0f;
        }

        public WeightedMetrics(float pNorm)
        {
            PNorm = pNorm;
        }
    }

}
