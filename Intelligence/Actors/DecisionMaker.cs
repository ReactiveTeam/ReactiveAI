using ReactiveAI.Intelligence.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Actors
{
    public sealed class DecisionMaker : DecisionMakerBase
    {
        IScheduler _aiSched;
        float _initThinkDelayMax;
        float _initThinkDelayMin;
        float _initUpdateDelayMax;
        float _initUpdateDelayMin;
        float _thinkDelayMax;
        float _thinkDelayMin;
        float _updateDelayMax;
        float _updateDelayMin;
        DeferredCommand _thinkCommand;
        IDeferredCommandHandle _thinkCommandHandle;

        DeferredCommand _updateCommand;
        IDeferredCommandHandle _updateCommandHandle;

        public float InitThinkDelayMin
        {
            get { return _initThinkDelayMin; }
            set { _initThinkDelayMin = value.ClampToPositive();
                _initThinkDelayMax = _initThinkDelayMin.ClampToLowerBound(_initThinkDelayMin);
                _thinkCommand.InitExecutionDelayMin = _initThinkDelayMin; }
        }

        public float InitThinkDelayMax
        {
            get { return _initThinkDelayMax; }
            set { _initThinkDelayMax = value.ClampToLowerBound(_initThinkDelayMin);
                _thinkCommand.InitExecutionDelayMax = _initThinkDelayMax;
            }
        }

        public float ThinkDelayMin
        {
            get { return _thinkDelayMin; }
            set
            {
                _thinkDelayMin = value.ClampToPositive();
                _thinkDelayMax = _thinkDelayMax.ClampToLowerBound(_thinkDelayMin);
                _thinkCommand.ExecutionDelayMin = _thinkDelayMin;
            }
        }

        public float ThinkDelayMax
        {
            get { return _thinkDelayMax; }
            set { _thinkDelayMax = value.ClampToLowerBound(_thinkDelayMin);
                _thinkCommand.ExecutionDelayMax = _thinkDelayMax;
            }
        }

        public float InitUpdateDelayMin
        {
            get { return _initUpdateDelayMin; }
            set
            {
                _initUpdateDelayMin = value.ClampToPositive();
                _initUpdateDelayMax = _initUpdateDelayMin.ClampToLowerBound(_initUpdateDelayMin);
                _updateCommand.InitExecutionDelayMin = _initUpdateDelayMin;
            }
        }

        public float InitUpdateDelayMax
        {
            get { return _initUpdateDelayMax; }
            set
            {
                _initUpdateDelayMax = value.ClampToLowerBound(_initUpdateDelayMin);
                _updateCommand.InitExecutionDelayMax = _initUpdateDelayMax;
            }
        }

        public float UpdateDelayMin
        {
            get { return _updateDelayMin; }
            set
            {
                _updateDelayMin = value.ClampToPositive();
                _updateDelayMax = _updateDelayMax.ClampToLowerBound(_updateDelayMin);
                _updateCommand.ExecutionDelayMin = _updateDelayMin;
            }
        }

        public float UpdateDelayMax
        {
            get { return _thinkDelayMax; }
            set
            {
                _updateDelayMax = value.ClampToLowerBound(_updateDelayMin);
                _updateCommand.ExecutionDelayMax = _updateDelayMax;
            }
        }

        protected override void OnStart()
        {
            _thinkCommandHandle = _aiSched.ThinkStream.Add(_thinkCommand);
            _updateCommandHandle = _aiSched.UpdateStream.Add(_updateCommand);
        }

        protected override void OnPause()
        {
            _thinkCommandHandle.Pause();
            _updateCommandHandle.Pause();
        }

        protected override void OnResume()
        {
            _thinkCommandHandle.Resume();
            _updateCommandHandle.Resume();
        }

        public DecisionMaker(IUtilityAI uai, IContextProvider contextProvider, IScheduler aiSched) : base(uai, contextProvider)
        {
            if (aiSched == null)
                throw new SchedNullException();

            InitializeThinkCommand();
            InitializeUpdateCommand();
            _aiSched = aiSched;
        }

        void InitializeThinkCommand()
        {
            _thinkCommand = new DeferredCommand(Think)
            {
                InitExecutionDelayMin = InitThinkDelayMin,
                InitExecutionDelayMax = InitThinkDelayMax,
                ExecutionDelayMin = ThinkDelayMin,
                ExecutionDelayMax = ThinkDelayMax,
            };
        }

        void InitializeUpdateCommand()
        {
            _updateCommand = new DeferredCommand(Update)
            {
                InitExecutionDelayMin = InitUpdateDelayMin,
                InitExecutionDelayMax = InitUpdateDelayMax,
                ExecutionDelayMin = UpdateDelayMin,
                ExecutionDelayMax = UpdateDelayMax,
            };
        }

        protected override void OnStop()
        {
            _thinkCommandHandle.Pause();
            _updateCommandHandle.Pause();
        }

        internal class SchedNullException : Exception { }
    }
}
