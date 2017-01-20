using ReactiveAI.Intelligence.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public interface IAction : IAIPrototype<IAction>
    {
        string NameId { get; }

        float ElapsedTime { get; }
        float Cooldown { get; set; }
        bool InCooldown { get; }
        ActionStatus ActionStatus { get; }
        void Execute(IContext context);
    }
}
