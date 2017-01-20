using ReactiveAI.Intelligence.Evaluators.Interfaces;
using ReactiveAI.Intelligence.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Considerations
{
    public abstract class ConsiderationBase<TContext> : IConsideration
       where TContext : class, IContext
    {
        IConsiderationCollection _collection;
        float _weight = 1.0f;
        protected IEvaluator Evaluator;

        /// <summary>
        ///   A string alias for ID.
        /// </summary>
        public string NameID { get; set; }

        /// <summary>
        ///   Gets or sets the default utility.
        /// </summary>
        public Utility DefaultUtil { get; set; }

        /// <summary>
        ///   Returns the utility for this consideration.
        /// </summary>
        /// <value>The utility.</value>
        public Utility Utility { get; protected set; }

        /// <summary>
        ///   The weight of this consideration.
        /// </summary>
        public float Weight
        {
            get { return _weight; }
            set { _weight = value.Clamp01(); }
        }

        /// <summary>
        ///   If true, then the output of the associated evaluator is inverted, in effect, inverting the
        ///   consideration.
        /// </summary>
        public bool IsInverted
        {
            get
            {
                return Evaluator != null && Evaluator.isInverted;
            }
            set
            {
                if (Evaluator == null)
                    return;

                Evaluator.isInverted = value;
            }
        }

        /// <summary>Calculates the utility given the specified context.</summary>
        /// <param name="context">The context.</param>
        public abstract void Consider(TContext context);

        public abstract IConsideration Clone();

        protected ConsiderationBase()
        {
        }

        protected ConsiderationBase(ConsiderationBase<TContext> other)
        {
            _collection = other._collection;
            NameID = other.NameID;
            DefaultUtil = other.DefaultUtil;
            Utility = other.Utility;
            Weight = other.Weight;
        }

        protected ConsiderationBase(string nameId, IConsiderationCollection collection)
        {
            if (collection == null)
                throw new ConsiderationCollectionNullException();

            NameID = nameId;
            _collection = collection;
            if (_collection.Add(this) == false)
                throw new ConsiderationAlreadyExistsInCollectionException(nameId);
        }

        void IConsideration.Consider(IContext context)
        {
            Consider((TContext)context);
        }

        internal class ConsiderationCollectionNullException : Exception
        {
        }

        internal class ConsiderationAlreadyExistsInCollectionException : Exception
        {
            string _message;

            public override string Message
            {
                get { return _message; }
            }

            public ConsiderationAlreadyExistsInCollectionException(string msg)
            {
                _message = string.Format("{0} already exists in the consideration collection", msg);
            }
        }
    }
}
