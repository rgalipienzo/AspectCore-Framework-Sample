using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace AspectCoreApi.AOP
{
    public class CacheSample : AbstractInterceptorAttribute
    {
        [FromServiceContext]
        public IMemoryCache MemoryCache { get; set; }

        private int _cacheTimeInSeconds;

        public CacheSample(int cacheTimeInSeconds)
        {
            _cacheTimeInSeconds = cacheTimeInSeconds;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var key = $"{context.ImplementationMethod.DeclaringType.Name}:{context.ImplementationMethod.Name}";
            var returnValue = await MemoryCache.GetOrCreateAsync(key, async cacheEntry =>
            {
                await next(context);         // So the real method logic is in next, 
                                             // so just call it
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheTimeInSeconds);
                return context.ReturnValue;  // The results are all in ReturnValue. 
                                             // For simplicity, I won’t write 
                                             // void / Task<T> / ValueTask<T> and so on. 
                                             // Compatible codes for various return values.
            });
            context.ReturnValue = returnValue;   // Set ReturnValue, because next will not be 
                                                 // called within the validity period of the 
                                                 // cache, so ReturnValue will not have a value, 
                                                 // we need to set the cached result 
                                                 // to ReturnValue
        }
    }
}
