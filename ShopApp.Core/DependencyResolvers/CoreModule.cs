using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ShopApp.Core.CrossCuttingConcerns.Caching;
using ShopApp.Core.CrossCuttingConcerns.Caching.Microsoft;
using ShopApp.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace ShopApp.Core.DependencyResolvers
{
    public class CoreModule:ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<Stopwatch>();
        }
    }
}
