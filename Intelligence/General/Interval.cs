using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public enum IntervalType
    {
        Open,
        Closed
    }

    public struct Interval<T> where T : struct, IComparable
    {
        public T LowerBound { get; }
        public T UpperBound { get; }

        public IntervalType LowerBoundType { get; }
        public IntervalType UpperBoundType { get; }

        public bool Contains(T point)
        {
            if (LowerBound.GetType() != typeof(T) || UpperBound.GetType() != typeof(T))
                throw new ArgumentException("Type mismatch", "point");

            var lower = LowerBoundType == IntervalType.Open ? LowerBound.CompareTo(point) < 0 : LowerBound.CompareTo(point) <= 0;
            var upper = UpperBoundType == IntervalType.Open ? UpperBound.CompareTo(point) > 0 : UpperBound.CompareTo(point) >= 0;

            return lower && upper;
        }

        public override string ToString()
        {
            return string.Format(
                "{0}{1}, {2}{3}",
                LowerBoundType == IntervalType.Open ? "(" : "[",
                LowerBound,
                UpperBound,
                UpperBoundType == IntervalType.Open ? ")" : "]");
        }

        public Interval(T lowerbound, T upperbound, IntervalType lowerBoundType = IntervalType.Closed,IntervalType upperBoundType = IntervalType.Closed) : this()
        {
            var a = lowerbound;
            var b = upperbound;
            var comparison = a.CompareTo(b);

            if(comparison > 0)
            {
                a = upperbound;
                b = lowerbound;
            }

            LowerBound = a;
            UpperBound = b;

            LowerBoundType = lowerBoundType;
            UpperBoundType = upperBoundType;
        }
    }

    public static class Interval
    {
        public static Interval<int> Range(int lower, int upper, IntervalType lowerType = IntervalType.Closed,IntervalType upperType = IntervalType.Closed)
        {
            return new Interval<int>(lower, upper, lowerType, upperType);
        }

        public static Interval<float> Range(float lower, float upper, IntervalType lowerType = IntervalType.Closed, IntervalType upperType = IntervalType.Closed)
        {
            return new Interval<float>(lower, upper, lowerType, upperType);
        }

        public static Interval<double> Range(double lower, double upper, IntervalType lowerType = IntervalType.Closed, IntervalType upperType = IntervalType.Closed)
        {
            return new Interval<double>(lower, upper, lowerType, upperType);
        }
    }
}
