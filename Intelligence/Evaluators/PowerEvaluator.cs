using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Evaluators
{
    public class PowerEvaluator : EvaluatorBase
    {
        float _dy;
        float _p;

        public override float Evaluate(float x)
        {
            var cx = x.Clamp<float>(Xa, Xb);
            cx = _dy * (float)Math.Pow((cx - Xa) / (Xb - Xa), _p) + Ya;
            return cx;
        }

        public PowerEvaluator()
        {
            _p = 2.0f;
            Initialize();
        }

        public PowerEvaluator(Pointf ptA,Pointf ptB) : base(ptA, ptB)
        {
            _p = 2.0f;
            Initialize();
        }

        public PowerEvaluator(Pointf ptA, Pointf ptB,float power ) : base(ptA, ptB)
        {
            _p = power.Clamp<float>(MinP,MaxP);
            Initialize();
        }

        void Initialize()
        {
            _dy = Yb - Ya;
        }

        const float MinP = 0.0f;
        const float MaxP = 10000f;
    }
}
