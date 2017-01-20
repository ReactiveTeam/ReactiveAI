using ReactiveAI.Intelligence.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Actions
{
    public sealed class BehaviourTransition : ActionBase, ITransition
    {
        Behaviour _behaviour;
        IBehaviourCollection _behaviourCollection;
        string _behaviourId;

        public Behaviour Behaviour
        {
            get
            {
                if (_behaviour != null)
                    return _behaviour;

                if (string.IsNullOrEmpty(_behaviourId) || _behaviourCollection.Contains(_behaviourId) == false)
                    throw new BehaviourDoesNotExistsException(_behaviourId);

                _behaviour = _behaviourCollection.Create(_behaviourId) as Behaviour;
                return _behaviour;
            }

            set { _behaviour = value ?? _behaviour; }
        }

        public IAction Select(IContext context)
        {
            return Behaviour.Select(context);
        }

        public override IAction Clone()
        {
            return new BehaviourTransition(this);
        }

        internal BehaviourTransition() { }

        BehaviourTransition(BehaviourTransition other) : base(other)
        {
            _behaviourId = other._behaviourId;
            _behaviourCollection = other._behaviourCollection;
        }

        public BehaviourTransition(Behaviour other)
        {
            if (other == null)
                throw new BehaviourNullException();

            _behaviour = other;
        }

        public BehaviourTransition(string nameId, string behaviourId, IBehaviourCollection collection) : base(nameId, collection?.Options?.Actions)
        {
            if (string.IsNullOrEmpty(behaviourId))
                throw new NameEmptyOrNullException();

            _behaviourId = behaviourId;
            _behaviourCollection = collection;
        }

        internal class BehaviourNullException : Exception { }

        internal class BehaviourDoesNotExistsException : Exception
        {
            string _message;

            public override string Message
            {
                get
                {
                    return _message;
                }
            }

            public BehaviourDoesNotExistsException(string nameId)
            {
                _message = string.Format("Error: {0} does not exist in the Behaviour collection!", nameId);
            }
        }
    }
}
