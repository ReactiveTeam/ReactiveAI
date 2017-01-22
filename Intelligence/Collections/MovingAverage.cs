using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Collections
{
    public class MovingAverage
    {
        CircularBuffer<float> _buffer;
        bool _latch = true;
        float _mean;

        float _oneOverN = 1.0f;

        public float Mean
        {
            get { return _mean; }
        }

        /// <summary>
        ///   The
        /// </summary>
        /// <value>The moving average rank.</value>
        public int MovingAverageDepth
        {
            get { return _buffer.Capacity; }
        }

        /// <summary>
        ///   Enqueue the specified value and updates the mean.
        /// </summary>
        public void Enqueue(float val)
        {
            _buffer.Enqueue(val);
            UpdateTheMean();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="MovingAverage"/> class.
        /// </summary>
        public MovingAverage()
        {
            Initialize(DefaultSize);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="MovingAverage"/> class.
        /// </summary>
        /// <param name="length">Length.</param>
        public MovingAverage(int length)
        {
            Initialize(length);
        }

        void Initialize(int size)
        {
            if (size < 2)
            {
                _buffer = new CircularBuffer<float>(DefaultSize);
                _oneOverN = 1.0f / DefaultSize;
            }
            else
            {
                _buffer = new CircularBuffer<float>(size);
                _oneOverN = 1.0f / size;
            }
        }

        float UpdateTheMean()
        {
            if (_latch)
            {
                _mean = _buffer.Mean();
                if (_buffer.Count == _buffer.Capacity)
                    _latch = false;
                return _mean;
            }

            _mean += _oneOverN * (_buffer.Head - _buffer.Tail);
            return _mean;
        }

        const int DefaultSize = 4;
    }
}
