using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Actions
{
    public class ActionCollection : IActionCollection
    {
        Dictionary<string, IAction> _actionsMap;

        public bool Add(IAction action)
        {
            if (action == null)
                return false;
            if (_actionsMap.ContainsKey(action.NameId))
                return false;
            if (string.IsNullOrEmpty(action.NameId))
                return false;

            _actionsMap.Add(action.NameId, action);
            return true;
        }

        public bool Contains(string nameId)
        {
            return _actionsMap.ContainsKey(nameId);
        }

        public void Clear()
        {
            _actionsMap.Clear();
        }

        public IAction Create(string nameId)
        {
            return _actionsMap.ContainsKey(nameId) ? _actionsMap[nameId].Clone() : null;
        }

        public ActionCollection()
        {
            _actionsMap = new Dictionary<string, IAction>();
        }
    }
}
