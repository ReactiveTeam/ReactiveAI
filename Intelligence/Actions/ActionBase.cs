using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Actions
{
    public class ActionBase : IAction
    {
        ActionStatus _actionStatus = ActionStatus.Idle;
        IActionCollection _collection;
        float _cooldown;
        float _startedTime;
        Stopwatch _cooldownTimer = new Stopwatch();

        public string NameId { get; set; }

        public float ElapsedTime
        {
            get
            {
                if (_actionStatus == ActionStatus.Running)
                    return AITime.Time - _startedTime;

                return 0f;
            }
        }

        public float Cooldown
        {
            get { return _cooldown; }
            set { _cooldown = value.ClampToLowerBound(0.0f); }
        }

        public bool InCooldown
        {
            get
            {
                if (ActionStatus == ActionStatus.Running || ActionStatus == ActionStatus.Idle)
                    return false;

                return true;
            }
        }

        public ActionStatus ActionStatus
        {
            get { return _actionStatus; }
            protected set { _actionStatus = value; }
        }

        public void Execute(IContext context)
        {
            if (CanExecute() == false)
                return;

            if (TryUpdate(context) == false)
            {
                _startedTime = AITime.Time;
                ActionStatus = ActionStatus.Running;
                OnExecute(context);
            }
        }

        public virtual IAction Clone()
        {
            return new ActionBase(this);
        }

        protected void EndInSuccess(IContext context)
        {
            if (ActionStatus != ActionStatus.Running)
                return;

            ActionStatus = ActionStatus.Success;
            FinalizeAction(context);
        }

        protected void EndInFailure(IContext context)
        {
            if (ActionStatus != ActionStatus.Running)
                return;

            ActionStatus = ActionStatus.Failure;
            FinalizeAction(context);
        }

        protected virtual void OnExecute(IContext context)
        {
            EndInSuccess(context);
        }

        protected virtual void OnUpdate(IContext context) { }

        protected virtual void OnStop(IContext context) { }

        public ActionBase() { }

        protected ActionBase(ActionBase other)
        {
            NameId = other.NameId;
            _collection = other._collection;
            Cooldown = other.Cooldown;
            _cooldownTimer = new Stopwatch();
        }

        public ActionBase(string nameId, IActionCollection collection)
        {
            if (string.IsNullOrEmpty(nameId))
                throw new NameEmptyOrNullException();

            if (collection == null)
                throw new ActionCollectionNullException();

            NameId = nameId;
            _collection = collection;
            AddSelfToCollection();
        }

        bool CanExecute()
        {
            if (InCooldown)
            {
                ActionStatus = ActionStatus.Failure;
                return false;
            }
            return true;
        }

        bool TryUpdate(IContext context)
        {
            if (ActionStatus == ActionStatus.Running)
            {
                OnUpdate(context);
                return true;
            }
            return false;
        }

        void FinalizeAction(IContext context)
        {
            OnStop(context);
            ResetAndStartCooldownTimer();
        }

        void ResetAndStartCooldownTimer()
        {
            _cooldownTimer.Reset();
            _cooldownTimer.Start();
        }

        void AddSelfToCollection()
        {
            if (_collection.Add(this) == false)
                throw new ActionAlreadyExistsException(NameId);
        }

        internal class NameEmptyOrNullException : Exception { }
        internal class ActionCollectionNullException : Exception { }
        internal class ActionAlreadyExistsException : Exception
        {
            string _message;

            public override string Message
            {
                get
                {
                    return _message;
                }
            }

            public ActionAlreadyExistsException(string nameId)
            {
                _message = string.Format("Error: {0} already exists in the collection", nameId);
            }
        }
    }
}
