using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Actors
{
    public class AiCollection : IAICollection
    {
        Dictionary<string, IUtilityAI> _aiMap;
        public IBehaviourCollection Behaviours { get; private set; }

        public IOptionCollection Options
        {
            get { return Behaviours.Options; }
        }

        public IConsiderationCollection Considerations
        {
            get { return Behaviours.Options.Considerations; }
        }

        public IActionCollection Actions
        {
            get { return Behaviours.Options.Actions; }
        }

        public bool Add(IUtilityAI ai)
        {
            if (ai == null)
                return false;
            if (string.IsNullOrEmpty(ai.NameId))
                return false;
            if (_aiMap.ContainsKey(ai.NameId))
                return false;

            _aiMap.Add(ai.NameId, ai);
            return true;
        }

        public bool Contains(string nameId)
        {
            return _aiMap.ContainsKey(nameId);
        }

        public IUtilityAI GetAi(string nameId)
        {
            return _aiMap.ContainsKey(nameId) ? _aiMap[nameId] : null;
        }

        public IUtilityAI Create(string nameId)
        {
            return _aiMap.ContainsKey(nameId) ? _aiMap[nameId].Clone() : null;
        }

        public void Clear()
        {
            _aiMap.Clear();
        }

        public void ClearAll()
        {
            _aiMap.Clear();
            Behaviours.Clear();
            Options.Clear();
            Considerations.Clear();
            Actions.Clear();
        }

        public AiCollection(IBehaviourCollection behaviourCollection)
        {
            if (behaviourCollection == null)
                throw new BehaviourCollectionNullException();

            _aiMap = new Dictionary<string, IUtilityAI>();
            Behaviours = behaviourCollection;
        }

        internal class BehaviourCollectionNullException : Exception
        {
        }
    }

}
