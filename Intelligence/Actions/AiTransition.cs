using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Actions
{
    public sealed class AiTransition : ActionBase, ITransition
    {
        IAICollection _aiCollection;
        string _aiNameId;

        IUtilityAI _targetAi;

        public override IAction Clone()
        {
            return new AiTransition(this);
        }

        public IAction Select(IContext context)
        {
            return TargetAi.Select(context);
        }

        internal AiTransition() { }

        public AiTransition(IUtilityAI ai)
        {
            if (ai == null)
                throw new TargetAiNullExcetion();

            _targetAi = ai;
        }

        AiTransition(AiTransition other) : base(other)
        {
            _aiNameId = other._aiNameId;
            _aiCollection = other._aiCollection;
        }

        public AiTransition(string nameId, string aiNameId, IAICollection collection) : base(nameId, collection?.Actions)
        {
            if (string.IsNullOrEmpty(nameId))
                throw new NameEmptyOrNullException();
            if (string.IsNullOrEmpty(aiNameId))
                throw new NameEmptyOrNullException();

            NameId = nameId;
            _aiNameId = aiNameId;
            _aiCollection = collection;
        }

        internal IUtilityAI TargetAi
        {
            get
            {
                if (_targetAi == null)
                {
                    if (_aiCollection.Contains(_aiNameId) == false)
                        throw new TargetAiDoesNotExistsException(_aiNameId);

                    _targetAi = _aiCollection.Create(_aiNameId);
                }
                return _targetAi;
            }
        }

        internal class TargetAiNullExcetion : Exception { }

        internal class TargetAiDoesNotExistsException : Exception
        {
            string _message;

            public override string Message
            {
                get
                {
                    return base.Message;
                }
            }

            public TargetAiDoesNotExistsException(string nameId)
            {
                _message = string.Format("Error: {0} does not exists in the AI collection!", nameId);
            }
        }

    }
}
