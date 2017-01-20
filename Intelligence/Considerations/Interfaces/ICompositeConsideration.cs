using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface ICompositeConsideration : IConsideration
    {
        IMeasure Measure { get; set; }
        bool AddConsideration(IConsideration consideration);
        bool AddConsideration(string considerationId);
    }
}
