using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Selectors
{
    public sealed class SequentialSelector : ISelector
    {
        int _count;
        int _curIdx;

        public int Select(ICollection<Utility> elements)
        {
            var count = elements.Count;
            if (count == 0)
                return -1;

            if (_count != count)
            {
                _curIdx = 0;
                _count = count;
                return _curIdx;
            }

            _curIdx++;
            _curIdx = _curIdx % _count;
            return _curIdx;
        }

        public ISelector Clone()
        {
            return new SequentialSelector();
        }
    }
}
