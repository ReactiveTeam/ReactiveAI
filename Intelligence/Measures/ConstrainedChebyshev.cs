using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Measures
{
    public sealed class ConstrainedChebyshev : IMeasure
    {
        float _lowerBound;
        Chebyshev _measure;

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
            return new ConstrainedChebyshev(LowerBound);
        }

        public ConstrainedChebyshev()
        {
            _measure = new Chebyshev();
        }

        public ConstrainedChebyshev(float lowerBound)
        {
            LowerBound = lowerBound;
            _measure = new Chebyshev();
        }
    }

}
