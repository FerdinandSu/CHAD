using Chad.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Chad
{
    public static class ChadExtensions
    {
        /// <summary>
        ///     向服务集合诸如CHAD提供的管理器
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>注入后的服务集合</returns>
        public static IServiceCollection AddChadManagers(this IServiceCollection services)
        {
            return services
                .AddTransient<JwtManager>()
                .AddScoped<AccountManager>()
                .AddSingleton<SystemManager>()
                .AddScoped<ChadManager>();
        }
    }
}