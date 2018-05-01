using System;
using EnsembleFX.Logging.Entities;

namespace EnsembleFX.Logging.LoggingAspect
{
    public class LoggingBehavior : AspectBehavior
    {
        //private DependencyManager dependencyManager;
        private ILogController logController;
        //TO DO Find a way to do this .net core dependency inject procedure
        //private IMethodReturn methodInvokeResult;
        internal const string MethodInfoFormatter = "Invoking method {0} at {1}";
        internal const string ExceptionFormatter = "Method {0} threw exception {1} at {2}";
        internal const string ReturnValueFormatter = "Method {0} returned {1} at {2}";

        //TODO: Injection has to be be defined in each api which uses this class library
        public LoggingBehavior(ILogController controller){
            if(controller != null)
                this.logController = controller;
        }
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            // Log method invocation message
            try
            {
                //dependencyManager = new DependencyManager();
                //logController = dependencyManager.Resolve<ILogController>(); //new LogController();
            }
            catch (Exception)
            {
                // Do nothing if resolution fails, do not fail the calling program
            }

            if (logController != null)
            {
                logController.Log(new ApplicationLogs()
                {
                    Message = String.Format(MethodInfoFormatter, input.MethodBase, DateTime.Now.ToLongTimeString()),
                    LogLevel = Enums.LogLevel.Info
                });

            }

            methodInvokeResult = getNext()(input, getNext);

            if (methodInvokeResult.Exception != null)
            {
                if (logController != null)
                {
                    // Log Exception
                    logController.Log(new ApplicationLogs()
                    {
                        Message = String.Format(ExceptionFormatter, input.MethodBase, methodInvokeResult.Exception.Message, DateTime.Now.ToLongTimeString()),
                        Exception = methodInvokeResult.Exception,
                        LogLevel = Enums.LogLevel.Error
                    });
                }
            }
            else
            {
                if (logController != null)
                {
                    // Log Info about method return
                    logController.Log(new ApplicationLogs()
                    {
                        Message = String.Format(ReturnValueFormatter, input.MethodBase, methodInvokeResult.ReturnValue, DateTime.Now.ToLongTimeString()),
                        LogLevel = Enums.LogLevel.Info
                    });
                }
            }

            return methodInvokeResult;
        }
    }
}
