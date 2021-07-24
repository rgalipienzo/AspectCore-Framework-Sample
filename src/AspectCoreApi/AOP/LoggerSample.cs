using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AspectCoreApi.AOP
{
    public class LoggerSample : AbstractInterceptorAttribute
    {
        [FromServiceContext]
        public ILogger<LoggerSample> Logger { get; set; }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var sw = Stopwatch.StartNew();
            Logger.LogInformation($"In - {context.ImplementationMethod.DeclaringType.Name}:{context.ImplementationMethod.Name}");
            try
            {
                await next(context); // Run the function
            }
            catch(Exception ex)
            {
                Logger.LogError($"In - {context.ImplementationMethod.DeclaringType.Name}:{context.ImplementationMethod.Name}", ex);
                throw;
            }
            Logger.LogInformation($"Out - {context.ImplementationMethod.DeclaringType.Name}:{context.ImplementationMethod.Name} in {sw.ElapsedMilliseconds} ms");
        }
    }
}
