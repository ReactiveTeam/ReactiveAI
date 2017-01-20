using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IActionCollection
    {
        bool Add(IAction action);
        bool Contains(string nameId);
        void Clear();
        IAction Create(string nameId);
    }
}
