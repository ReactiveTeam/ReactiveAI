using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public static class IntervalExtensions
    {
        static readonly IntervalType Open = IntervalType.Open;
        static readonly IntervalType Closed = IntervalType.Closed;

        public static bool Overlaps(this Interval<float> @this, Interval<float> other)
        {
            return @this.Contains(other.LowerBound) || @this.Contains(other.UpperBound);
        }

        public static bool Adjacent(this Interval<float> val, Interval<float> other)
        {
            if (AIMath.AeqB(val.UpperBound, other.LowerBound) && val.UpperBoundType == Closed && other.LowerBoundType == Closed)
                return true;

            if (AIMath.AeqB(val.LowerBound, other.UpperBound) && val.LowerBoundType == Closed && other.UpperBoundType == Closed)
                return true;

            return false;

        }

        public static bool LeftAdjacent(this Interval<float> val,Interval<float> other)
        {
            if (AIMath.AeqB(val.UpperBound, other.LowerBound) && val.UpperBoundType == Closed && other.LowerBoundType == Closed)
                return true;

            return false;
        }

        public static bool RightAdjacent(this Interval<float> val, Interval<float> other)
        {
            if (AIMath.AeqB(val.LowerBound, other.UpperBound) && val.LowerBoundType == Closed && other.UpperBoundType == Closed)
                return true;

            return false;
        }

        public static bool LessThan(this Interval<float> val, Interval<float> other)
        {
            return val.UpperBound <= other.LowerBound;
        }

        public static bool GreaterThan(this Interval<float> val, Interval<float> other)
        {
            return val.LowerBound >= other.UpperBound;
        }

        public static int CompareTo(this Interval<float> val, Interval<float> other)
        {
            if (val.LessThan(other))
            {
                return -1;
            }
            return val.GreaterThan(other) ? 1 : 0;
        }

        public static int Length(this Interval<int> @this)
        {
            return @this.UpperBound - @this.LowerBound;
        }

        public static float Length(this Interval<float> @this)
        {
            return @this.UpperBound - @this.LowerBound;
        }

        public static double Length(this Interval<double> @this)
        {
            return @this.UpperBound - @this.LowerBound;
        }
    }
}
