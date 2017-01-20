using ReactiveAI.Intelligence.Actions;
using ReactiveAI.Intelligence.Behaviours;
using ReactiveAI.Intelligence.Considerations;
using ReactiveAI.Intelligence.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Actors
{
    public static class AiCollectionConstructor
    {

        public static IAICollection Create()
        {
            var a = new ActionCollection();
            var c = new ConsiderationCollection();
            var o = new OptionCollection(a, c);
            var b = new BehaviourCollection(o);
            return new AiCollection(b);
        }
    }
}
