using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IAICollection
    {
        IBehaviourCollection Behaviours { get; }
        IOptionCollection Options { get; }
        IConsiderationCollection Considerations { get; }
        IActionCollection Actions { get; }

        bool Add(IUtilityAI ai);
        bool Contains(string nameId);
        IUtilityAI GetAi(string nameId);
        void Clear();
        void ClearAll();
        IUtilityAI Create(string nameId);
    }
}
