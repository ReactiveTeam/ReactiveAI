using ReactiveAI.Intelligence.Behaviours;
using ReactiveAI.Intelligence.General;
using ReactiveAI.Intelligence.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Actors
{
    public sealed class UtilityAi : IUtilityAI
    {
        Dictionary<string, Behaviour> _behaviourMap;
        List<Behaviour> _behaviours;
        List<Utility> _behaviourUtilities;
        IAICollection _collection;

        ISelector _selector;

        public string NameId { get; set; }

        public ISelector Selector
        {
            get { return _selector; }
            set { _selector = value ?? _selector; }
        }

        public bool Add(Behaviour behaviour)
        {
            if (behaviour == null)
                return false;
            if (_behaviours.Contains(behaviour))
                return false;

            return InternalAddBehaviour(behaviour);
        }

        public bool Remove(Behaviour behaviour)
        {
            if (string.IsNullOrEmpty(behaviour?.NameID))
                return false;

            return InternalRemove(behaviour.NameID);
        }

        public bool InternalRemove(string behaviourId)
        {
            if (_behaviourMap.ContainsKey(behaviourId) == false)
                return false;

            var idx = _behaviours.IndexOf(_behaviourMap[behaviourId]);
            _behaviourUtilities.RemoveAt(idx);
            _behaviours.RemoveAt(idx);
            _behaviourMap.Remove(behaviourId);
            return true;
        }

        public bool AddBehaviour(string behaviourId)
        {
            if (_collection == null)
                return false;
            if (_collection.Behaviours.Contains(behaviourId) == false)
                return false;
            if (_behaviours.Any(b => string.Equals(b.NameID, behaviourId)))
                return false;

            return InternalAddBehaviour(behaviourId);
        }

        public bool RemoveBehaviour(string behaviourId)
        {
            return InternalRemove(behaviourId);
        }

        public IAction Select(IContext context)
        {
            if (_behaviours.Count == 0)
                return null;
            if (_behaviours.Count == 1)
                return _behaviours[0].Select(context);

            UpdateBehaviourUtilitites(context);
            return SelectAction(context);
        }

        public IUtilityAI Clone()
        {
            return new UtilityAi(this);
        }

        public UtilityAi()
        {
            Initialize();
        }

        UtilityAi(UtilityAi other)
        {
            NameId = other.NameId;
            _collection = other._collection;
            Initialize();
            _selector = other._selector.Clone();

            for (int i = 0; i < other._behaviours.Count; i++)
            {
                var b = other._behaviours[i].Clone() as Behaviour;
                _behaviours.Add(b);
                _behaviourUtilities.Add(b.Utility);
            }
        }

        public UtilityAi(string nameId, IAICollection collection)
        {
            if (string.IsNullOrEmpty(nameId))
                throw new NameIdIsNullOrEmptyException();
            if (collection == null)
                throw new AiCollectionNullException();

            NameId = nameId;
            _collection = collection;
            Initialize();
            if (_collection.Add(this) == false)
                throw new AiAlreadyExistsInCollectionException(nameId);
        }

        bool InternalAddBehaviour(Behaviour behaviour)
        {
            if (string.IsNullOrEmpty(behaviour.NameID) ||
               _behaviourMap.ContainsKey(behaviour.NameID))
                return false;

            _behaviourMap.Add(behaviour.NameID, behaviour);
            _behaviours.Add(behaviour);
            _behaviourUtilities.Add(new Utility(0.0f, 0.0f));
            return true;
        }

        bool InternalAddBehaviour(string behaviourId)
        {
            if (_behaviourMap.ContainsKey(behaviourId))
                return false;

            var behaviour = _collection.Behaviours.Create(behaviourId) as Behaviour;
            _behaviourMap.Add(behaviourId, behaviour);
            _behaviours.Add(behaviour);
            _behaviourUtilities.Add(new Utility(0.0f, 0.0f));
            return true;
        }

        void Initialize()
        {
            _selector = new MaxUtilitySelector();
            _behaviours = new List<Behaviour>();
            _behaviourMap = new Dictionary<string, Behaviour>();
            _behaviourUtilities = new List<Utility>();
        }

        void UpdateBehaviourUtilitites(IContext context)
        {
            for (int i = 0, count = _behaviours.Count; i < count; i++)
            {
                _behaviours[i].Consider(context);
                _behaviourUtilities[i] = _behaviours[i].Utility;
            }
        }

        IAction SelectAction(IContext context)
        {
            var idx = Selector.Select(_behaviourUtilities);
            IBehaviour selectedBehaviour = idx >= 0 ? _behaviours[idx] : null;
            return selectedBehaviour?.Select(context);
        }

        internal class AiCollectionNullException : Exception
        {
        }

        internal class NameIdIsNullOrEmptyException : Exception
        {
        }

        internal class AiAlreadyExistsInCollectionException : Exception
        {
            string _message;

            public override string Message
            {
                get { return _message; }
            }

            public AiAlreadyExistsInCollectionException()
            {
            }

            public AiAlreadyExistsInCollectionException(string nameId)
            {
                _message = string.Format("{0} already exists in behaviour collection", nameId);
            }
        }
    }
}
