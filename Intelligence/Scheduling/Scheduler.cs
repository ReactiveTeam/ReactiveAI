using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Scheduling
{
    public sealed class Scheduler : IScheduler
    {
        /// <summary>
        ///   The Think cycle.
        /// </summary>
        public CommandStream ThinkStream { get; private set; }

        /// <summary>
        ///   The Update cycle.
        /// </summary>
        /// <value>The update queue.</value>
        public CommandStream UpdateStream { get; private set; }

        /// <summary>
        ///   Tick this instance.
        /// </summary>
        public void Tick()
        {
            ThinkStream.Process();
            UpdateStream.Process();
        }

        public Scheduler()
        {
            ThinkStream = new CommandStream(128)
            {
                MaxProcessingTime = 1
            };
            UpdateStream = new CommandStream(128)
            {
                MaxProcessingTime = 3
            };
        }
    }
}
