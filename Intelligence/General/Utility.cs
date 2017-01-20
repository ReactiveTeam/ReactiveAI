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

        public float Value
        {
            get { return _value; }
            set { _value = value.Clamp01(); }
        }

        public float Weight
        {
            get { return _weight; }
            set { _weight = value.Clamp01(); }
        }

        public float Combined
        {
            get { return Value * Weight; }
        }

        public bool IsZero
        {
            get { return AIMath.AeqZero(Combined); }
        }

        public bool IsOne
        {
            get { return AIMath.AeqB(Combined, 1.0f); }
        }

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

        public Utility(float value)
        {
            _value = value.Clamp01();
            _weight = 1.0f;
        }

        public Utility(float value,float weight)
        {
            _value = value.Clamp01();
            _weight = weight.Clamp01();
        }
    }
}
