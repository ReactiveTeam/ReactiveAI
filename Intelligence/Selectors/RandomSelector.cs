using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Selectors
{
    public sealed class RandomSelector : ISelector
    {
        Pcg _random;

        public int Select(ICollection<Utility> elements)
        {
            var count = elements.Count;
            return count == 0 ? -1 : _random.Next(0, count);
        }

        public ISelector Clone()
        {
            return new RandomSelector();
        }

        public RandomSelector()
        {
            _random = new Pcg();
        }

        public RandomSelector(Pcg random)
        {
            _random = random;
        }
    }
}
