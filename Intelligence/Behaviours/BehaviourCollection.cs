using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Behaviours
{
    public class BehaviourCollection : IBehaviourCollection
    {
        Dictionary<string, IBehaviour> _behavioursMap;
        public IOptionCollection Options { get; private set; }

        public bool Add(IBehaviour behaviour)
        {
            if (behaviour == null)
                return false;
            if (_behavioursMap.ContainsKey(behaviour.NameID))
                return false;
            if (string.IsNullOrEmpty(behaviour.NameID))
                return false;

            _behavioursMap.Add(behaviour.NameID, behaviour);
            return true;
        }

        public bool Contains(string nameId)
        {
            return _behavioursMap.ContainsKey(nameId);
        }

        public void Clear()
        {
            _behavioursMap.Clear();
        }

        public void ClearAll()
        {
            _behavioursMap.Clear();
            Options.ClearAll();
        }

        public IBehaviour Create(string nameId)
        {
            return _behavioursMap.ContainsKey(nameId) ? _behavioursMap[nameId].Clone() as IBehaviour : null;
        }

        public BehaviourCollection(IOptionCollection optionCollection)
        {
            if (optionCollection == null)
                throw new OptionCollectionNullException();

            _behavioursMap = new Dictionary<string, IBehaviour>();
            Options = optionCollection;
        }

        internal class OptionCollectionNullException : Exception
        {
        }
    }
}
