using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Selectors
{
    public sealed class WeightedRandomSelector : ISelector
    {
        float _proportion = 0.2f;
        Pcg _random;

        public float Proportion
        {
            get { return _proportion; }
            set { _proportion = value.Clamp01(); }
        }

        public int Select(ICollection<Utility> elements)
        {
            var count = elements.Count;
            if (count == 0)
                return -1;

            var maxElemIdx = (_proportion * count).CeilToInt().Clamp(0, count - 1);

            var sorted = elements.Select((x, i) => new KeyValuePair<Utility, int>(x, i))
                                 .OrderByDescending(x => x.Key.Combined)
                                 .ToList();

            List<Utility> sortedUtils = sorted.Select(x => x.Key).ToList();
            List<int> sortedUtilIndices = sorted.Select(x => x.Value).ToList();

            if (maxElemIdx == 0)
                return sortedUtilIndices[0];

            var cumSum = new float[maxElemIdx];
            cumSum[0] = sortedUtils[0].Weight;
            for (int i = 1; i < maxElemIdx; i++)
                cumSum[i] = cumSum[i - 1] + sortedUtils[i].Weight;
            for (int i = 0; i < maxElemIdx; i++)
                cumSum[i] /= cumSum[maxElemIdx - 1];

            float rval = (float)_random.NextDouble();
            int index = Array.BinarySearch(cumSum, rval);

            // From MSDN: If the index is negative, it represents the bitwise
            // complement of the next larger element in the array.
            if (index < 0)
                index = ~index;
            return sortedUtilIndices[index];
        }

        public ISelector Clone()
        {
            return new WeightedRandomSelector(Proportion);
        }

        public WeightedRandomSelector()
        {
            _random = new Pcg();
        }

        public WeightedRandomSelector(float proportion)
        {
            Proportion = proportion;
        }

        public WeightedRandomSelector(Pcg random)
        {
            _random = random;
        }
    }
}
