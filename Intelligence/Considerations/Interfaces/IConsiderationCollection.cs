using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IConsiderationCollection
    {
        bool Add(IConsideration consideration);
        bool Contains(string nameId);
        void Clear();
        IConsideration Create(string nameId);
    }
}
