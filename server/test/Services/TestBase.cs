using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chad.Services.Tests
{
    public abstract class TestBase<T> where T : notnull
    {
        protected static string GetRandomString() => Guid.NewGuid().ToString();
        protected T GetInstance() => Dependency.GetService<T>();
        protected DependencyResolverHelper Dependency { get; }
        protected TestBase()
        {
            Dependency = new(
                ChadApi.Program.CreateHostBuilder(Array.Empty<string>()).Build());
        }
    }
}
