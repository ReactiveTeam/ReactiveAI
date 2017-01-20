using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Measures
{
    public sealed class Chebyshev : IMeasure
    {
        public float Calculate(ICollection<Utility> elements)
        {
            var wsum = 0.0f;
            int count = elements.Count;

            if (count == 0)
                return 0.0f;

            foreach (var el in elements)
                wsum += el.Weight;

            if (AIMath.AeqZero(wsum))
                return 0.0f;

            var vlist = new List<float>(count);
            foreach (var el in elements)
                vlist.Add(el.Value * (el.Weight / wsum));

            var ret = vlist.Max<float>();
            return ret;
        }

        public IMeasure Clone()
        {
            return new Chebyshev();
        }
    }
}
