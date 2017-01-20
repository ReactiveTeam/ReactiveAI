using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public static class AITime
    {
        static readonly Stopwatch Clock;

        public static float Time
        {
            get { return (float)Clock.Elapsed.TotalSeconds; }
        }

        static AITime()
        {
            Clock = new Stopwatch();
            Clock.Start();
        }
    }
}
