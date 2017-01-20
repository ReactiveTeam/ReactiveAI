using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Selectors
{
    public sealed class MaxUtilitySelector : ISelector
    {
        public int Select(ICollection<Utility> elements)
        {
            var count = elements.Count;
            if (count == 0)
                return -1;
            if (count == 1)
                return 0;

            var elemList = elements.ToList();
            var maxUtil = 0.0f;
            var selIdx = -1;
            for (var i = 0; i < count; i++)
            {
                var el = elemList[i];
                if (el.Combined > maxUtil)
                {
                    maxUtil = el.Combined;
                    selIdx = i;
                }
            }

            return selIdx;
        }

        public ISelector Clone()
        {
            return new MaxUtilitySelector();
        }
    }
}
