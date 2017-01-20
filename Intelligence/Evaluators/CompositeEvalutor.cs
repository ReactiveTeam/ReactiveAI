using ReactiveAI.Intelligence.Evaluators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Evaluators
{
    public class CompositeEvalutor : EvaluatorBase
    {
        public override float Evaluate(float x)
        {
            var ev = FindEvaluator(x);
            return ev != null ? ev.Evaluate(x) : LinearHoleInterpolator(x);
        }

        public void Add(IEvaluator ev)
        {
            if (DoesNotOverlapWithAnyEV(ev))
                Evaluators.Add(ev);

            Evaluators.Sort((e1, e2) => e1.XInterval.CompareTo(e2.XInterval));
            UpdateXyPoints();
        }

        public CompositeEvalutor()
        {
            Evaluators = new List<IEvaluator>();
        }

        bool DoesNotOverlapWithAnyEV(IEvaluator ev)
        {
            foreach (var cev in Evaluators)
                if (ev.XInterval.Overlaps(cev.XInterval))
                    if (ev.XInterval.Adjacent(cev.XInterval))
                        continue;
                    else
                        return false;

            return true;
        }

        void UpdateXyPoints()
        {
            var count = Evaluators.Count;
            if (count == 1)
            {
                SingleEvaluatorXyPointsUpdate();
            }
            else
            {
                MultiEvaluatorXyPointsUpdate();
            }
        }

        void SingleEvaluatorXyPointsUpdate()
        {
            Xa = Evaluators[0].PtA.X;
            Ya = Evaluators[0].PtA.Y;
            Xb = Evaluators[0].PtB.X;
            Yb = Evaluators[0].PtB.Y;
        }

        void MultiEvaluatorXyPointsUpdate()
        {
            foreach(var ev in Evaluators)
            {
                if(Xa >= ev.MinX)
                {
                    Xa = ev.MinX;
                    Ya = ev.PtA.Y;
                }

                if(Xb <= ev.MaxX)
                {
                    Xb = ev.MaxX;
                    Yb = ev.PtB.Y;
                }
            }
        }

        IEvaluator FindEvaluator(float x)
        {
            int evCount = Evaluators.Count;
            if (x.InInterval(XInterval))
                return FindInternalEvaluator(x);

            if (x.AboveInterval(XInterval))
                return Evaluators[evCount - 1];

            return x.BelowInterval(XInterval) ? Evaluators[0] : null;
        }

        IEvaluator FindInternalEvaluator(float x)
        {
            int evCount = Evaluators.Count;
            for(int i=0;i< evCount; i++)
            {
                if (x.InInterval(Evaluators[i].XInterval))
                    return Evaluators[i];
            }

            return null;
        }

        float LinearHoleInterpolator(float x)
        {
            var lrev = FindLeftAndRightInterpolators(x);
            var x1 = lrev.Key.MaxX;
            var y1 = lrev.Key.Evaluate(x1);
            var xr = lrev.Value.MinX;
            var yr = lrev.Value.Evaluate(xr);
            var alpha = (x - x1) / (xr - x1);
            return y1 + alpha * (yr - y1);
        }

        KeyValuePair<IEvaluator,IEvaluator> FindLeftAndRightInterpolators(float x)
        {
            int evCount = Evaluators.Count;
            IEvaluator lev = null;
            IEvaluator rev = null;
            for(int i = 0; i < evCount - 1; i++)
            {
                lev = Evaluators[i];
                rev = Evaluators[i + 1];
                if (x.AboveInterval(lev.XInterval) && x.BelowInterval(rev.XInterval))
                    break;
            }

            return new KeyValuePair<IEvaluator, IEvaluator>(lev, rev);
        }

        internal List<IEvaluator> Evaluators;
    }
}
