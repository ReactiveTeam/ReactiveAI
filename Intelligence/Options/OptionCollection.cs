using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Options
{
    public class OptionCollection : IOptionCollection
    {
        Dictionary<string, IOption> _optionsMap;
        public IActionCollection Actions { get; private set; }
        public IConsiderationCollection Considerations { get; private set; }

        public bool Add(IOption option)
        {
            if (option == null)
                return false;
            if (string.IsNullOrEmpty(option.NameID))
                return false;
            if (_optionsMap.ContainsKey(option.NameID))
                return false;

            _optionsMap.Add(option.NameID, option);
            return true;
        }

        public bool Contains(string nameId)
        {
            return _optionsMap.ContainsKey(nameId);
        }

        public void Clear()
        {
            _optionsMap.Clear();
        }

        public void ClearAll()
        {
            _optionsMap.Clear();
            Actions.Clear();
            Considerations.Clear();
        }

        public IOption Create(string nameId)
        {
            return _optionsMap.ContainsKey(nameId) ? _optionsMap[nameId].Clone() as IOption : null;
        }

        public OptionCollection(IActionCollection actionCollection, IConsiderationCollection considerationCollection)
        {
            if (actionCollection == null)
                throw new ActionCollectionNullException();
            if (considerationCollection == null)
                throw new ConsiderationCollectionNullException();

            _optionsMap = new Dictionary<string, IOption>();
            Actions = actionCollection;
            Considerations = considerationCollection;
        }

        internal class ActionCollectionNullException : Exception
        {
        }

        internal class ConsiderationCollectionNullException : Exception
        {
        }
    }
}
