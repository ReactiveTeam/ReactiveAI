using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    /// <summary>
    /// An interval could be open and closed or combination of both at either
    /// end.
    /// </summary>
    public enum IntervalType
    {
        Open,
        Closed
    }

    /// <summary>
    /// Represents a vectorless interval of the form [a,b] or (a,b) or any 
    /// combination of exclusive and inclusive end points.
    /// </summary>
    /// <typeparam name="T">Any comparent type</typeparam>
    /// <remarks>
    /// This is a vectorless interval, therefore if end component is larger
    /// than start component, the interval will swap the two ends around
    /// such that a is always larger than b.
    /// </remarks>
    public struct Interval<T> where T : struct, IComparable
    {
        public T LowerBound { get; }
        public T UpperBound { get; }

        public IntervalType LowerBoundType { get; }
        public IntervalType UpperBoundType { get; }

        /// <summary>
        /// Check if given point lies within the interval
        /// </summary>
        /// <param name="point"></param>
        /// <returns>True if point lies within the interval, otherwise false</returns>
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

    /// <summary>
    /// Static class to generate regular Intervals using common types
    /// </summary>
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
