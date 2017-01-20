using ReactiveAI.Intelligence.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Actors
{
    public abstract class DecisionMakerBase : IDecisionMaker
    {
        IUtilityAI _ai;
        IContextProvider _contextProvider;
        IAction _currentAction;
        IContext _currentContext;
        int _recursionCounter;
        ITransition _transitionAction;
        public DecisionMakerState State { get; protected set; }

        /// <summary>
        ///   Start the decison maker Ai.
        /// </summary>
        public void Start()
        {
            if (State != DecisionMakerState.Stopped)
                return;

            State = DecisionMakerState.Running;
            OnStart();
        }

        public void Stop()
        {
            if (State == DecisionMakerState.Stopped)
                return;

            State = DecisionMakerState.Stopped;
            OnStop();
        }

        public void Pause()
        {
            if (State != DecisionMakerState.Running)
                return;

            State = DecisionMakerState.Paused;
            OnPause();
        }

        public void Resume()
        {
            if (State != DecisionMakerState.Paused)
                return;

            State = DecisionMakerState.Running;
            OnResume();
        }

        /// <summary>
        ///   Makes a decision on what should be the next action to be executed.
        /// </summary>
        public void Think()
        {
            if (ActionStillRunning())
                return;

            if (CouldNotUpdateContext())
                return;

            if (AiDidSelectAction())
            {
                while (IsTransition())
                    ConnectorSelectAction();

                ExecuteCurrentAction();
            }
        }

        /// <summary>
        ///   Updates the action selected by Think() - that is if it needs updating, otherwise this will
        ///   simply return.
        /// </summary>
        public void Update()
        {
            if (CouldNotUpdateContext())
                return;

            ExecuteCurrentAction();
        }

        protected abstract void OnStart();

        protected abstract void OnStop();

        protected abstract void OnPause();

        protected abstract void OnResume();


        protected DecisionMakerBase(IUtilityAI ai, IContextProvider contextProvider)
        {
            if (ai == null)
                throw new UtilityAiNullException();
            if (contextProvider == null)
                throw new ContextProviderNullException();

            _ai = ai;
            _contextProvider = contextProvider;
            State = DecisionMakerState.Stopped;
        }

        bool ActionStillRunning()
        {
            if (_currentAction != null)
                return _currentAction.ActionStatus == ActionStatus.Running;

            return false;
        }

        bool CouldNotUpdateContext()
        {
            _recursionCounter = 0;
            _currentContext = _contextProvider.Context();
            return _currentContext == null;
        }

        bool AiDidSelectAction()
        {
            _currentAction = _ai.Select(_currentContext);
            return _currentAction != null;
        }

        bool IsTransition()
        {
            CheckForRecursions();
            _transitionAction = _currentAction as ITransition;
            return _transitionAction != null;
        }

        void CheckForRecursions()
        {
            _recursionCounter++;
            if (_recursionCounter >= MaxRecursions)
                throw new PotentialCircularDependencyException(_recursionCounter);
        }

        void ConnectorSelectAction()
        {
            _currentAction = _transitionAction.Select(_currentContext);
        }

        void ExecuteCurrentAction()
        {
            if (_currentAction == null)
                return;

            _currentAction.Execute(_currentContext);
            if (_currentAction.ActionStatus != ActionStatus.Running)
                _currentAction = null;
        }

        const int MaxRecursions = 100;

        internal class UtilityAiNullException : Exception
        {
        }

        internal class ContextProviderNullException : Exception
        {
        }

        internal class PotentialCircularDependencyException : Exception
        {
            int _loopCount;

            public override string Message
            {
                get
                {
                    return string.Format("The Think() loop completed {0} iterations without " +
                                         "reaching to an executable Action! It appears that there is a circular " +
                                         "dependency at play here.",
                                         _loopCount);
                }
            }

            public PotentialCircularDependencyException(int loopCount) : base(loopCount.ToString())
            {
                _loopCount = loopCount;
            }
        }
    }
}
