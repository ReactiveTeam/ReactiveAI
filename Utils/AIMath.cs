using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public static class AIMath
    {
        static float _eps;

        static double _deps;

        /// <summary>
        ///   Returns a machine specific estimate of the float EPSILON value.
        /// </summary>
        public static float Eps
        {
            get { return _eps; }
        }

        /// <summary>
        ///   Returns a machine specific estimate of the double EPSILON value.
        /// </summary>
        public static double Deps
        {
            get { return _deps; }
        }

        public static void UpdateEps(float centre = 1.0f)
        {
            EstimateEps(centre);
        }

        public static void UpdateDeps(double centre = 1.0)
        {
            EstimateDeps(centre);
        }

        /// <summary>
        ///   The correct way to test for a == b for floating point numbers.
        /// </summary>
        /// <returns><c>true</c>, if a == b, <c>false</c> otherwise.</returns>
        /// <param name="a">The alpha value.</param>
        /// <param name="b">The blue value.</param>
        public static bool AeqB(float a, float b)
        {
            var df = Math.Abs(a - b);
            return Math.Abs(a - b) < Eps;
        }

        /// <summary>
        ///   The correct way to test for a == b for double floating point numbers.
        /// </summary>
        /// <returns><c>true</c>, if a == b, <c>false</c> otherwise.</returns>
        /// <param name="a">The alpha value.</param>
        /// <param name="b">The blue value.</param>
        public static bool AeqB(double a, double b)
        {
            return Math.Abs(a - b) < Deps;
        }

        /// <summary>
        ///   The correct way to test for a != b for floating point numbers.
        /// </summary>
        /// <returns><c>true</c>, if a != b, <c>false</c> otherwise.</returns>
        /// <param name="a">The alpha value.</param>
        /// <param name="b">The blue value.</param>
        public static bool AneqB(float a, float b)
        {
            return Math.Abs(a - b) > Eps;
        }

        /// <summary>
        ///   The correct way to test for a != b for double floating point numbers.
        /// </summary>
        /// <returns><c>true</c>, if a != b, <c>false</c> otherwise.</returns>
        /// <param name="a">The alpha value.</param>
        /// <param name="b">The blue value.</param>
        public static bool AneqB(double a, double b)
        {
            return Math.Abs(a - b) > Deps;
        }

        /// <summary>
        ///   The correct way to test for a == 0 for floating point numbers.
        /// </summary>
        /// <returns><c>true</c>, if a == 0, <c>false</c> otherwise.</returns>
        /// <param name="a">The alpha value.</param>
        public static bool AeqZero(float a)
        {
            return Math.Abs(a) < Eps;
        }

        /// <summary>
        ///   The correct way to test for a == 0 for double floating point numbers.
        /// </summary>
        /// <returns><c>true</c>, if a == 0, <c>false</c> otherwise.</returns>
        /// <param name="a">The alpha value.</param>
        public static bool AeqZero(double a)
        {
            return Math.Abs(a) < Deps;
        }

        /// <summary>
        ///   The correct way to test for a != 0 for floating point numbers.
        /// </summary>
        /// <returns><c>true</c>, if a != 0, <c>false</c> otherwise.</returns>
        /// <param name="a">The alpha value.</param>
        public static bool AneqZero(float a)
        {
            return Math.Abs(a) > Eps;
        }

        /// <summary>
        ///   The correct way to test for a != 0 for double floating point numbers.
        /// </summary>
        /// <returns><c>true</c>, if a != 0, <c>false</c> otherwise.</returns>
        /// <param name="a">The alpha value.</param>
        public static bool AneqZero(double a)
        {
            return Math.Abs(a) > Deps;
        }

        /// <summary>
        ///   Generate a linearly spaced sample vector of the given count between the specified values (inclusive).
        ///   Equivalent to MATLAB linspace but with the count as first instead of last argument.
        /// </summary>
        public static float[] LinearSpaced(int count, float start, float stop)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count");

            if (count == 1)
                return new[] { stop };

            float step = (stop - start) / (count - 1);

            var data = new float[count];
            for (int i = 0; i < data.Length; i++)
                data[i] = start + i * step;

            data[data.Length - 1] = stop;
            return data;
        }

        /// <summary>
        ///   Find out whether the provided 32 bit integer is an even number.
        /// </summary>
        /// <param name="number">The number to very whether it's even.</param>
        /// <returns>True if and only if it is an even number.</returns>
        public static bool IsEven(this int number)
        {
            return (number & 0x1) == 0x0;
        }

        /// <summary>
        ///   Find out whether the provided 64 bit integer is an even number.
        /// </summary>
        /// <param name="number">The number to very whether it's even.</param>
        /// <returns>True if and only if it is an even number.</returns>
        public static bool IsEven(this long number)
        {
            return (number & 0x1) == 0x0;
        }

        /// <summary>
        ///   Find out whether the provided 32 bit integer is an odd number.
        /// </summary>
        /// <param name="number">The number to very whether it's odd.</param>
        /// <returns>True if and only if it is an odd number.</returns>
        public static bool IsOdd(this int number)
        {
            return (number & 0x1) == 0x1;
        }

        /// <summary>
        ///   Find out whether the provided 64 bit integer is an odd number.
        /// </summary>
        /// <param name="number">The number to very whether it's odd.</param>
        /// <returns>True if and only if it is an odd number.</returns>
        public static bool IsOdd(this long number)
        {
            return (number & 0x1) == 0x1;
        }

        static AIMath()
        {
            UpdateEps();
            UpdateDeps();
        }

        /// <summary>
        ///   This estimates the floating point EPSILON value for the current machine. Due to the way
        ///   MS defines their float.EPSILON it is unreliable... (way too small).
        /// </summary>
        static void EstimateEps(float centre)
        {
            _eps = 1f;
            while (_eps + centre > centre)
                _eps /= 2f;
        }

        /// <summary>
        ///   This estimates the double point EPSILON value for the current machine. Due to the way
        ///   MS defines their double.EPSILON it is unreliable... (way too small).
        /// </summary>
        static void EstimateDeps(double centre)
        {
            _deps = 1.0;
            while (_deps + centre > centre)
                _deps /= 2.0;
        }
    }
}
