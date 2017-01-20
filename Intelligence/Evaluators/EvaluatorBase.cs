using ReactiveAI.Intelligence.Evaluators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Evaluators
{
    public class EvaluatorBase : IEvaluator, IComparable<IEvaluator>
    {
        protected float Xa;
        protected float Xb;
        protected float Ya;
        protected float Yb;

        public bool isInverted
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public float MaxX
        {
            get
            {
                return Xb;
            }
        }

        public float MaxY
        {
            get
            {
                return Math.Max(Ya, Yb);
            }
        }

        public float MinX
        {
            get
            {
                return Xa;
            }
        }

        public float MinY
        {
            get
            {
                return Math.Min(Ya, Yb);
            }
        }

        public Pointf PtA
        {
            get { return new Pointf(Xa, Ya); }
        }

        public Pointf PtB
        {
            get
            {
                return new Pointf(Xb, Yb);
            }
        }

        public Interval<float> XInterval
        {
            get
            {
                return new Interval<float>(MinX, MaxY);
            }
        }

        public Interval<float> YInterval
        {
            get
            {
                return new Interval<float>(MinY, MaxY);
            }
        }

        public int CompareTo(IEvaluator other)
        {
            return XInterval.CompareTo(other.XInterval);
        }

        float IEvaluator.Evaluate(float x)
        {
            return isInverted ? 1f - Evaluate(x) : Evaluate(x);
        }
        public virtual float Evaluate(float x)
        {
            return 0f;
        }

        protected EvaluatorBase()
        {
            Initialize(0.0f, 0.0f, 1.0f, 1.0f);
        }

        protected EvaluatorBase(Pointf ptA,Pointf ptB)
        {
            Initialize(ptA.X, ptA.Y, PtB.X, PtB.Y);
        }

        void Initialize(float xA,float yA,float xB, float yB)
        {
            if (xA > xB)
                throw new EvaluatorXaGreaterThanXbException();

            Xa = xA;
            Xb = xB;
            Ya = yB;
            Yb = yB;
        }

        internal class EvaluatorDxZeroException : Exception { }
        internal class EvaluatorXaGreaterThanXbException : Exception { }
    }
}
