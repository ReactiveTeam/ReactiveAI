using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IBehaviour : ICompositeConsideration
    {
        ISelector Selector { get; set; }

        bool AddOption(IOption option);
        bool AddOption(string optionId);
        IAction Select(IContext context);
    }
}
