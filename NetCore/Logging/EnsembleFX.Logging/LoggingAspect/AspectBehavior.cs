using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Practices.Unity.InterceptionExtension;

namespace EnsembleFX.Logging.LoggingAspect
{
    public abstract class AspectBehavior //: IInterceptionBehavior //TO DO Find a way to do this .net core dependency inject procedure
    {
        internal bool WillExecuteBehavior = true;

        //TO DO Find a way to do this .net core dependency inject procedure
       // public abstract IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext);

        public virtual IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        /// <summary>
        /// Denotes if the interception behavior will execute, true by default unless overriden
        /// </summary>
        public virtual bool WillExecute
        {
            get
            {
                return WillExecuteBehavior;
            }
            set
            {
                WillExecuteBehavior = value;
            }
        }
    }
}
