using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IBehaviourCollection
    {
        IOptionCollection Options { get; }

        bool Add(IBehaviour behaviour);
        bool Contains(string nameId);
        void Clear();
        void ClearAll();
        IBehaviour Create(string nameId);
    }
}
