using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IOption :ICompositeConsideration
    {
        IAction Action { get; }

        bool SetAction(IAction action);
        bool SetAction(string actionId);
    }
}
