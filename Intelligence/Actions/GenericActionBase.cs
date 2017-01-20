using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Actions
{
    public class ActionBase<TContext> : IAction where TContext : class, IContext
    {

        readonly IActionCollection _collection;
        readonly Stopwatch _cooldownTimer = new Stopwatch();
        float _cooldown;
        float _startedTime;
        ActionStatus _actionStatus = ActionStatus.Idle;

        public string NameId { get; }

        public float ElapsedTime
        {
            get
            {
                if (ActionStatus == ActionStatus.Running)
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

                return (float)_cooldownTimer.Elapsed.TotalSeconds < _cooldown;
            }
        }

        public ActionStatus ActionStatus
        {
            get { return _actionStatus; }
            protected set { _actionStatus = value; }
        }

        public void Execute(TContext context)
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
            return new ActionBase<TContext>(this);
        }

        protected void EndInSuccess(IContext context)
        {
            EndInSuccess((TContext)context);
        }

        protected void EndInFailure(IContext context)
        {
            EndInFailure((TContext)context);
        }

        protected void EndInSuccess(TContext context)
        {
            if (ActionStatus != ActionStatus.Running)
                return;

            ActionStatus = ActionStatus.Success;
            FinalizeAction(context);
        }

        protected void EndInFailure(TContext context)
        {
            if (ActionStatus != ActionStatus.Running)
                return;

            ActionStatus = ActionStatus.Failure;
            FinalizeAction(context);
        }

        protected virtual void OnExecute(TContext context)
        {
            EndInSuccess(context);
        }

        protected virtual void OnUpdate(TContext context)
        {
        }

        protected virtual void OnStop(TContext context)
        {
        }

        public ActionBase() { }

        protected ActionBase(ActionBase<TContext> other)
        {
            NameId = other.NameId;
            _collection = other._collection;
            Cooldown = other.Cooldown;
            _cooldownTimer = new Stopwatch();
        }

        public ActionBase(string nameId, IActionCollection collection)
        {
            if (String.IsNullOrEmpty(nameId))
                throw new ActionBase.NameEmptyOrNullException();
            if (collection == null)
                throw new ActionCollectionNullException();

            NameId = nameId;
            _collection = collection;
            AddSelfToCollection();

        }

        void IAction.Execute(IContext context)
        {
            Execute((TContext)context);
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

        bool TryUpdate(TContext context)
        {
            if (ActionStatus == ActionStatus.Running)
            {
                OnUpdate(context);
                return true;
            }

            return false;
        }

        void FinalizeAction(TContext context)
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
                throw new ActionAlreadyExistsInCollectionException(NameId);
        }

        internal class ActionCollectionNullException : Exception { }

        internal class ActionAlreadyExistsInCollectionException : Exception
        {
            public override string Message
            {
                get;
            }

            public ActionAlreadyExistsInCollectionException(string name)
            {
                Message = string.Format("Error: {0} already exists in the action collection!", name);
            }
        }
    }
}
