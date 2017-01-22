using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.General
{
    [Serializable]
    public struct Utility : IEquatable<Utility>, IComparable<Utility>
    {
        float _weight;
        float _value;

        /// <summary>
        /// The value of the option associated with this Utility
        /// This parameter is normalized to the interval [0,1]
        /// </summary>
        public float Value
        {
            get { return _value; }
            set { _value = value.Clamp01(); }
        }

        /// <summary>
        /// The weighting of the option associated with this Utility
        /// This parameter is normalized to the interval [0,1]
        /// </summary>
        /// <value>The weight</value>
        public float Weight
        {
            get { return _weight; }
            set { _weight = value.Clamp01(); }
        }

        /// <summary>
        /// Returns the Value*Weight of this Utility
        /// </summary>
        public float Combined
        {
            get { return Value * Weight; }
        }

        /// <summary>
        /// Gets a value indicating wether the combined utility is zero
        /// </summary>
        /// <value><c>true</c> if the combined utility is zero; otherwise. <c>false</c></value>
        public bool IsZero
        {
            get { return AIMath.AeqZero(Combined); }
        }

        /// <summary>
        /// Gets a value indicating wether the combined utility is one
        /// </summary>
        /// <value><c>true</c> if the combined utility is one; otherwise. <c>false</c></value>
        public bool IsOne
        {
            get { return AIMath.AeqB(Combined, 1.0f); }
        }

        /// <summary>
        /// Determines wether the specified <see cref="Utility"/> is equal to the current
        /// <see cref="Utility"/>
        /// </summary>
        /// <param name="other">The <see cref="Utility"/> to compare with current <see cref="Utility"/></param>
        /// <returns><c>true</c> if the specified parameter is equal to the current <see cref="Utility"/>;
        /// otherwise <c>false</c></returns>
        public bool Equals(Utility other)
        {
            return AIMath.AeqB(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var util = (Utility)obj;
            return Equals(util);
        }

        public override int GetHashCode()
        {
            return Combined.GetHashCode();
        }

        /// <summary>
        /// Compares to
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Utility other)
        {
            return Combined.CompareTo(other.Combined);
        }

        public static implicit operator Utility(float a)
        {
            return new Utility(a);
        }

        public static bool operator ==(Utility a, Utility b)
        {
            return AIMath.AeqB(a.Value, b.Value) && AIMath.AeqB(a.Weight, b.Weight);
        }

        public static bool operator !=(Utility a, Utility b)
        {
            return AIMath.AneqB(a.Value, b.Value) && AIMath.AneqB(a.Weight, b.Weight);
        }

        public static bool operator >(Utility a, Utility b)
        {
            return a.Combined > b.Combined;
        }

        public static bool operator <(Utility a, Utility b)
        {
            return a.Combined < b.Combined;
        }

        public static bool operator >=(Utility a, Utility b)
        {
            return a.Combined >= b.Combined;
        }

        public static bool operator <=(Utility a, Utility b)
        {
            return a.Combined <= b.Combined;
        }

        public override string ToString()
        {
            return string.Format("[Utility: Value={0}, Weight={1}, Combined={2}]", Value, Weight, Combined);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Utility"/> struct.
        /// </summary>
        /// <param name="value"></param>
        public Utility(float value)
        {
            _value = value.Clamp01();
            _weight = 1.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Utility"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="weight"></param>
        public Utility(float value,float weight)
        {
            _value = value.Clamp01();
            _weight = weight.Clamp01();
        }
    }
}
