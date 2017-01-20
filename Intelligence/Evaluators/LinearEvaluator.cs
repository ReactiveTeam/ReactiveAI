using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Evaluators
{
    public class LinearEvaluator : EvaluatorBase
    {
        float _dyOverDx;

        public override float Evaluate(float x)
        {
            return (Ya + _dyOverDx * (x - Xa)).Clamp01();
        }

        public LinearEvaluator()
        {
            Initialize();
        }

        public LinearEvaluator(Pointf ptA,Pointf ptB) :base(ptA, ptB)
        {
            Initialize();
        }

        void Initialize()
        {
            _dyOverDx = (Yb - Ya) / (Xb - Xa);
        }
    }
}
