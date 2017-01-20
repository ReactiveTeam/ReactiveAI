using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IOptionCollection
    {
        IActionCollection Actions { get; }
        IConsiderationCollection Considerations { get; }

        bool Add(IOption option);
        bool Contains(string nameId);
        void Clear();
        void ClearAll();
        IOption Create(string nameId);
    }
}
