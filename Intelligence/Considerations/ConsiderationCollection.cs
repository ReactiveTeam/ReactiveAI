using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Considerations
{
    public class ConsiderationCollection : IConsiderationCollection
    {
        Dictionary<string, IConsideration> _considerationsMap;

        public bool Add(IConsideration consideration)
        {
            if (consideration == null)
                return false;
            if (_considerationsMap.ContainsKey(consideration.NameID))
                return false;
            if (string.IsNullOrEmpty(consideration.NameID))
                return false;

            _considerationsMap.Add(consideration.NameID, consideration);
            return true;
        }

        public bool Contains(string nameId)
        {
            return _considerationsMap.ContainsKey(nameId);
        }

        public void Clear()
        {
            _considerationsMap.Clear();
        }

        public IConsideration Create(string nameId)
        {
            return _considerationsMap.ContainsKey(nameId) ? _considerationsMap[nameId].Clone() : null;
        }

        public ConsiderationCollection()
        {
            _considerationsMap = new Dictionary<string, IConsideration>();
        }
    }
}
