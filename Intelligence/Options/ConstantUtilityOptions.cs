using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Options
{
    public sealed class ConstantUtilityOption : Option
    {
        /// <summary>
        ///   Calculates the utility given the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The utility.</returns>
        public override void Consider(IContext context)
        {
        }

        public override IConsideration Clone()
        {
            return new ConstantUtilityOption(this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConstantUtilityOption"/> class.
        /// </summary>
        public ConstantUtilityOption()
        {
            Weight = 1.0f;
            DefaultUtil = new Utility(0.0f, Weight);
        }

        ConstantUtilityOption(ConstantUtilityOption other) : base(other)
        {
            Weight = other.Weight;
            DefaultUtil = other.DefaultUtil;
        }

        public ConstantUtilityOption(string nameId, IOptionCollection collection) : base(nameId, collection)
        {
        }
    }
}
