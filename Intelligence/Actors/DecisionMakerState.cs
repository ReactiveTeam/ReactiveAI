using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Actors
{
    public enum DecisionMakerState
    {
        /// <summary>
        ///   AI Client Stopped state.
        /// </summary>
        Stopped,

        /// <summary>
        ///   AI Client Running state.
        /// </summary>
        Running,

        /// <summary>
        ///   AI Client Paused state.
        /// </summary>
        Paused
    }
}
