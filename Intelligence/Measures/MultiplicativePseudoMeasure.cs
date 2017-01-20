using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Measures
{
    public sealed class MultiplicativePseudoMeasure : IMeasure
    {
        public float Calculate(ICollection<Utility> elements)
        {
            return elements.MultiplyCombined();
        }

        public IMeasure Clone()
        {
            return new MultiplicativePseudoMeasure();
        }
    }
}
