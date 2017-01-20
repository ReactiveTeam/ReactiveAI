using ReactiveAI.Intelligence.Considerations;
using ReactiveAI.Intelligence.General;
using ReactiveAI.Intelligence.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Options
{
    public class Option : CompositeConsideration, IOption
    {
        IOptionCollection _collection;

        /// <summary>
        ///   The action to be executed when this option is selected.
        /// </summary>
        public IAction Action { get; private set; }

        public bool SetAction(IAction action)
        {
            if (action == null)
                return false;

            Action = action;
            return true;
        }

        public bool SetAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId))
                return false;
            if (_collection == null)
                return false;
            if (_collection.Actions.Contains(actionId) == false)
                return false;

            Action = _collection.Actions.Create(actionId);
            return true;
        }

        /// <summary>
        ///   Calculates the utility for this option given the provided context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The utility of this option.</returns>
        public override void Consider(IContext context)
        {
            if (Action.InCooldown)
                Utility = new Utility(0.0f, Weight);
            base.Consider(context);
        }

        public override IConsideration Clone()
        {
            return new Option(this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="T:Crystal.Option"/> class.
        /// </summary>
        public Option()
        {
            Initialize();
        }

        protected Option(Option other) : base(other)
        {
            _collection = other._collection;
            Weight = other.Weight;
            Measure = other.Measure?.Clone();
            Action = other.Action?.Clone();
        }

        public Option(string nameId, IOptionCollection collection) : base(collection?.Considerations)
        {
            _collection = collection;
            NameID = nameId;
            Initialize();
            if (_collection.Add(this) == false)
                throw new OptionAlreadyExistsInCollectionException(nameId);
        }

        void Initialize()
        {
            Weight = 1.0f;
            Measure = new WeightedMetrics();
        }

        internal class OptionAlreadyExistsInCollectionException : Exception
        {
            string _message;

            public override string Message
            {
                get { return _message; }
            }

            public OptionAlreadyExistsInCollectionException(string nameId)
            {
                _message = string.Format("{0} already exists in options collection", nameId);
            }
        }
    }
}
