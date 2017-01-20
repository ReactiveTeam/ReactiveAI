using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.General
{
    public abstract class AIConstructor 
    {
        public IAICollection Collection { get; protected set; }

        public IActionCollection Actions
        {
            get { return Collection.Actions; }
        }

        public IConsiderationCollection Considerations
        {
            get { return Collection.Considerations; }
        }

        public IOptionCollection Options
        {
            get { return Collection.Options; }
        }

        public IBehaviourCollection Behaviours
        {
            get { return Collection.Behaviours; }
        }

        public IAICollection AIs
        {
            get { return Collection; }
        }

        public IUtilityAI Create(string name)
        {
            return AIs.Create(name);
        }

        protected void IsOkay(bool expression)
        {
            if (expression == false)
                throw new AiConfigurationException();
        }

        protected abstract void DefineActions();
        protected abstract void DefineConsiderations();
        protected abstract void DefineOptions();
        protected abstract void DefineBehaviours();
        protected abstract void ConfigureAI();

        protected AIConstructor(IAICollection collection)
        {
            if (collection == null)
                throw new AiCollectionNullException();

            Collection = collection;
            Initialize();
        }

        void Initialize()
        {
            DefineActions();
            DefineBehaviours();
            DefineConsiderations();
            DefineOptions();
            ConfigureAI();
        }

        protected IAction A;
        protected IConsideration C;
        protected ICompositeConsideration Cc;
        protected IOption O;
        protected IBehaviour B;
        protected IUtilityAI Ai;

        internal class AiCollectionNullException : Exception { }

        internal class AiConfigurationException : Exception { }
    }
}
