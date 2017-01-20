using ReactiveAI.Intelligence.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IDeferredCommandHandle
    {
        /// <summary>
        ///   The scheduled command this handle refers to.
        /// </summary>
        DeferredCommand Command { get; }

        /// <summary>
        ///   If true the associated command is still being executed.
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        ///   Pause execution of command.
        /// </summary>
        void Pause();

        /// <summary>
        ///   Resume execution of this command.
        /// </summary>
        void Resume();
    }
}
