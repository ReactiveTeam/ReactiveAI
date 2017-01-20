using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IUtilityAI : IAIPrototype<IUtilityAI>
    {
        string NameId { get; }
        ISelector Selector { get; set; }
        bool AddBehaviour(string behaviourId);
        bool RemoveBehaviour(string behaviourId);
        IAction Select(IContext context);
    }
}
