using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface ISelector : IAIPrototype<ISelector>
    {
        int Select(ICollection<Utility> elements);
    }
}
