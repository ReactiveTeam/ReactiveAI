using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Actions
{
    public sealed class ActionSequence : ActionBase
    {
        List<IAction> _actions = new List<IAction>(2);
        Dictionary<int, ActionStatus> _actionStatusMap = new Dictionary<int, ActionStatus>();

        public IList<IAction> Actions
        {
            get { return _actions; }
        }

        public override IAction Clone()
        {
            return new ActionSequence(this);
        }

        protected override void OnExecute(IContext context)
        {
            int count = _actions.Count;
            for (int i = 0; i < count; i++)
            {
                _actions[i].Execute(context);
                UpdateStatusMap(i);
            }

            ResolveActionStatusesThenEnd(context);
        }

        protected override void OnUpdate(IContext context)
        {
            int count = _actions.Count;
            for (int i = 0; i < count; i++)
            {
                if (_actionStatusMap[i] != ActionStatus.Running)
                    continue;

                _actions[i].Execute(context);
                UpdateStatusMap(i);
            }

            ResolveActionStatusesThenEnd(context);
        }

        public ActionSequence() { }

        ActionSequence(ActionSequence other)
        {
            _actions = new List<IAction>();
            _actionStatusMap = new Dictionary<int, ActionStatus>();

            for (int i = 0; i < other._actions.Count; i++)
            {
                var n = other._actions[i].Clone();
                _actions.Add(n);
                _actionStatusMap.Add(i, n.ActionStatus);
            }
        }

        public ActionSequence(string nameId, IActionCollection collection) : base(nameId, collection) { }

        void UpdateStatusMap(int idx)
        {
            _actionStatusMap[idx] = _actions[idx].ActionStatus;
        }

        void ResolveActionStatusesThenEnd(IContext context)
        {
            if (_actionStatusMap.ContainsValue(ActionStatus.Running))
                return;

            if (_actionStatusMap.Values.All(s => s == ActionStatus.Success))
                EndInSuccess(context);
            else
                EndInFailure(context);

            _actionStatusMap.Clear();
        }
    }
}
