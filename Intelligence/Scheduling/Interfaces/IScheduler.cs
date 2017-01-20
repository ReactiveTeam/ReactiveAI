using ReactiveAI.Intelligence.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IScheduler
    {
        /// <summary>
        ///   The Think cycle.
        /// </summary>
        CommandStream ThinkStream { get; }

        /// <summary>
        ///   The Update cycle.
        /// </summary>
        CommandStream UpdateStream { get; }

        /// <summary>
        ///   Invoking this will execute one think and update cycle.
        /// </summary>
        void Tick();
    }
}
