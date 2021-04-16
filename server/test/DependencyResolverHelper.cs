using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Chad
{
    public class DependencyResolverHelper
    {
        private readonly IHost _webHost;

        
        public DependencyResolverHelper(IHost webHost) => _webHost = webHost;

        public IServiceScope GetScope()
            => _webHost.Services.CreateScope();
        public T GetService<T>()
        where T:notnull
        {
            var serviceScope = _webHost.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            try
            {
                var scopedService = services.GetRequiredService<T>();
                return scopedService;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            ;
        }
    }
}
