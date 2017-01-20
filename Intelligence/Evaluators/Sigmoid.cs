using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Evaluators
{
    public class Sigmoid : EvaluatorBase
    {
        float _dyOverTwo;
        float _k;
        float _oneMinusK;
        float _twoOverDx;
        float _xMean;
        float _yMean;

        public override float Evaluate(float x)
        {
            var cxMinusXMean = x.Clamp<float>(Xa, Xb) - _xMean;
            var num = _twoOverDx * cxMinusXMean * _oneMinusK;
            var den = _k * (1 - 2 * Math.Abs(_twoOverDx * cxMinusXMean)) + 1;
            var val = _dyOverTwo * (num / den) + _yMean;
            return val;
        }

        public Sigmoid(Pointf ptA, Pointf ptB) : base(ptA, ptB)
        {
            _k = -0.6f;
            Initialize();
        }

        public Sigmoid(Pointf ptA, Pointf ptB, float k) : base(ptA, ptB)
        {
            _k = k.Clamp<float>(MinK, MaxK);
            Initialize();
        }

        void Initialize()
        {
            _twoOverDx = Math.Abs(2.0f / (Xb - Xa));
            _xMean = (Xa + Xb) / 2.0f;
            _yMean = (Ya + Yb) / 2.0f;
            _dyOverTwo = (Yb - Ya) / 2.0f;
            _oneMinusK = 1.0f - _k;

        }

        const float MinK = -0.99999f;
        const float MaxK = 0.99999f;
    }
}
