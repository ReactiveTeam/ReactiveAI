using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public struct Pointf : IEquatable<Pointf>
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public bool Equals(Pointf other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override string ToString()
        {
            return string.Format("({0}. {1})", X, Y);
        }

        public Pointf(float x,float y) : this()
        {
            X = x;
            Y = y;
        }
    }
}
