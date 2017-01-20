using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IMeasure : IAIPrototype<IMeasure>
    {
        float Calculate(ICollection<Utility> elements);
    }
}
