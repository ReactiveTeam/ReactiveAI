﻿using ReactiveAI.Intelligence.General;
using ReactiveAI.Intelligence.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Considerations
{
    public class CompositeConsideration : ICompositeConsideration
    {
        IConsiderationCollection _collection;
        List<IConsideration> _considerations;
        List<Utility> _considerationUtilities;

        Utility _defaultUtility = new Utility(0.0f, 1.0f);
        IMeasure _measure;
        float _weight = 1.0f;

        bool _isInverted;

        /// <summary>
        ///   A string alias for ID.
        /// </summary>
        public string NameID { get; set; }

        /// <summary>
        ///   The measure to be used to evaluate the utility of this consideration.
        /// </summary>
        public IMeasure Measure
        {
            get { return _measure; }
            set { _measure = value ?? _measure; }
        }

        /// <summary>
        ///   Gets or sets the default utility.
        /// </summary>
        /// <value>The default utility.</value>
        public Utility DefaultUtil
        {
            get { return _defaultUtility; }
            set
            {
                _defaultUtility = value;
                Utility = value;
            }
        }

        /// <summary>
        ///   Returns the combined utility for this consideration.
        /// </summary>
        /// <value>The utility.</value>
        public Utility Utility { get; protected set; }

        /// <summary>
        ///   Gets the weight of this consideration.
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
            get { return _isInverted; }
            set
            {
                if (_isInverted == value ||
                   _considerations.Count == 0)
                    return;

                foreach (var c in _considerations)
                    c.IsInverted = value;
            }
        }

        public bool AddConsideration(IConsideration consideration)
        {
            if (consideration == null)
                return false;
            if (_considerations.Contains(consideration))
                return false;
            if (_considerations.Any(c => string.Equals(c.NameID, consideration.NameID)))
                return false;

            InternalAddConsideration(consideration);
            return true;
        }

        public bool AddConsideration(string considerationId)
        {
            if (_collection == null)
                return false;
            if (string.IsNullOrEmpty(considerationId))
                return false;
            if (_considerations.Any(c => string.Equals(c.NameID, considerationId)))
                return false;
            if (_collection.Contains(considerationId) == false)
                return false;

            InternalAddConsideration(considerationId);
            return true;
        }

        /// <summary>
        ///   Calculates the utility for this option given the provided context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The utility of this option.</returns>
        public virtual void Consider(IContext context)
        {
            if (_considerations.Count == 0)
                return;

            UpdateConsiderationUtilities(context);
            var mValue = Measure.Calculate(_considerationUtilities);
            Utility = new Utility(mValue, Weight);
        }

        public virtual IConsideration Clone()
        {
            return new CompositeConsideration(this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Crystal.CompositeConsideration"/> class.
        /// </summary>
        public CompositeConsideration()
        {
            Initialize();
        }

        protected CompositeConsideration(CompositeConsideration other)
        {
            CreateLists();
            NameID = other.NameID;
            _collection = other._collection;
            _measure = other._measure.Clone();
            _defaultUtility = other._defaultUtility;
            Utility = other.Utility;
            Weight = other.Weight;

            for (int i = 0; i < other._considerations.Count; i++)
            {
                _considerations.Add(other._considerations[i].Clone());
                _considerationUtilities.Add(other._considerationUtilities[i]);
            }
        }

        protected CompositeConsideration(IConsiderationCollection collection)
        {
            if (collection == null)
                throw new ConsiderationCollectionNullException();

            _collection = collection;
            Initialize();
        }

        public CompositeConsideration(string nameId, IConsiderationCollection collection)
        {
            if (string.IsNullOrEmpty(nameId))
                throw new NameIdIsNullOrEmptyException();
            if (collection == null)
                throw new ConsiderationCollectionNullException();

            NameID = nameId;
            _collection = collection;
            Initialize();
            if (_collection.Add(this) == false)
                throw new NameIdAlreadyExistsInCollectionException(nameId);
        }

        void Initialize()
        {
            Weight = 1.0f;
            _measure = new WeightedMetrics();
            CreateLists();
        }

        void CreateLists()
        {
            _considerations = new List<IConsideration>();
            _considerationUtilities = new List<Utility>();
        }

        void UpdateConsiderationUtilities(IContext context)
        {
            for (int i = 0, count = _considerations.Count; i < count; i++)
            {
                _considerations[i].Consider(context);
                _considerationUtilities[i] = _considerations[i].Utility;
            }
        }

        void InternalAddConsideration(IConsideration c)
        {
            _considerations.Add(c);
            _considerationUtilities.Add(new Utility(0.0f, 0.0f));
        }

        void InternalAddConsideration(string nameId)
        {
            _considerations.Add(_collection.Create(nameId));
            _considerationUtilities.Add(new Utility(0.0f, 0.0f));
        }

        internal class NameIdIsNullOrEmptyException : Exception
        {
        }

        internal class ConsiderationCollectionNullException : Exception
        {
        }

        internal class NameIdAlreadyExistsInCollectionException : Exception
        {
            string _message;

            public override string Message
            {
                get { return _message; }
            }

            public NameIdAlreadyExistsInCollectionException(string msg)
            {
                _message = string.Format("{0} already exists in the consideration collection", msg);
            }
        }
    }
}
