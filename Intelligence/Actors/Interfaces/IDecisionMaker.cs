using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IDecisionMaker
    {
        //DecisionMakerState State { get; }
        void Start();
        void Stop();
        void Pause();
        void Resume();
        void Think();
        void Update();
    }
}
