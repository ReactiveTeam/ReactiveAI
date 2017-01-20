using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Measures
{
    public sealed class ConstrainedWeightedMetrics : IMeasure
    {
        float _lowerBound;
        WeightedMetrics _measure;

        public float PNormMin
        {
            get { return _measure.PNormMin; }
        }

        public float PNormMax
        {
            get { return _measure.PNormMax; }
        }

        public float PNorm
        {
            get { return _measure.PNorm; }
            set { _measure.PNorm = value; }
        }

        /// <summary>
        ///   If the combined value of any utility is below this, the value of this measure will be 0.
        /// </summary>
        public float LowerBound
        {
            get { return _lowerBound; }
            set { _lowerBound = value.Clamp01(); }
        }

        public float Calculate(ICollection<Utility> elements)
        {
            if (elements.Any(el => el.Combined < LowerBound))
                return 0.0f;

            return _measure.Calculate(elements);
        }

        public IMeasure Clone()
        {
            return new ConstrainedWeightedMetrics(PNorm, LowerBound);
        }

        public ConstrainedWeightedMetrics()
        {
            _measure = new WeightedMetrics();
        }

        public ConstrainedWeightedMetrics(float pNorm)
        {
            _measure = new WeightedMetrics(pNorm);
        }

        public ConstrainedWeightedMetrics(float pNorm, float lowerBound)
        {
            _measure = new WeightedMetrics(pNorm);
            LowerBound = lowerBound;
        }
    }

}
