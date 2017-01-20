using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IConsideration : IAIPrototype<IConsideration>
    {
        /// <summary>
        ///   A unique named identifier for this consideration.
        /// </summary>
        string NameID { get; }

        /// <summary>
        ///   Gets or sets the default utility.
        /// </summary>
        Utility DefaultUtil { get; set; }

        /// <summary>
        ///   Returns the utility for this consideration.
        /// </summary>
        Utility Utility { get; }

        /// <summary>
        ///   The weight of this consideration.
        /// </summary>
        float Weight { get; set; }

        /// <summary>
        ///   If true, then the output of the associated evaluator is inverted, in effect, inverting the
        ///   consideration.
        /// </summary>
        bool IsInverted { get; set; }

        /// <summary>Calculates the utility given the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>The utility.</returns>
        void Consider(IContext context);
    }
}
