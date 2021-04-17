using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Chad.Data;
using Chad.Models;
using Chad.Models.Common;
using ChoETL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chad.Services
{
    public class SystemManager
    {
        private const string SystemInitializedToken = "sys_init";

        public SystemManager(
            IServiceScopeFactory scopeFactory,
            ILogger<SystemManager> logger,
            IConfiguration configuration)
        {
            ScopeFactory = scopeFactory;
            Logger = logger;
            Configuration = configuration;
            using var scope = ScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ChadDb>();
            if (context == null)
                throw new NullReferenceException();
            var dbt = context.Configs.Find(SystemInitializedToken);
            Initialized = dbt != null;
        }

        private IServiceScopeFactory ScopeFactory { get; }
        private ILogger<SystemManager> Logger { get; }
        private IConfiguration Configuration { get; }
        public bool Initialized { get; private set; }


        public async Task InitializeAsync()
        {
            using var scope = ScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ChadDb>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var accountManager = scope.ServiceProvider.GetRequiredService<AccountManager>();
            Logger.LogInformation("Initializing......");
            // 初始化角色
            await roleManager.CreateAsync(new IdentityRole(UserRole.Administrator.AsUserRoleString()));
            await roleManager.CreateAsync(new IdentityRole(UserRole.Teacher.AsUserRoleString()));
            await roleManager.CreateAsync(new IdentityRole(UserRole.Student.AsUserRoleString()));
            // 初始化ROOT管理员和备用管理员
            await accountManager.CreateAsync(new ManagedGeneratingUser
            {
                Name = "ROOT",
                Username = "root",
                Role = UserRole.Administrator,
                InitialPassword = "MAR5_admin"
            });
            await accountManager.CreateAsync(new ManagedGeneratingUser
            {
                Name = "BACKUP",
                Username = "root_backup",
                Role = UserRole.Administrator,
                InitialPassword = "MAR5_admin"
            });


            // 初始化周期计时器
            await context.Configs.AddAsync(new DbConfig
            {
                Type = SystemInitializedToken,
                Value = ""
            });

            // 配置初始化

            var config = Configuration.GetSection("Initialization");
            await LoadIncludesAsync(config, scope.ServiceProvider);

            await context.SaveChangesAsync();
            Initialized = true;
            Logger.LogInformation("Initialized.");
        }

        private async Task LoadIncludesAsync(IConfiguration config, IServiceProvider serviceProvider)
        {
            Logger.LogWarning($"当前目录：{Directory.GetCurrentDirectory()}");
            try
            {
                var includes = config?.GetSection("Includes");
                if (includes == null) return;

                var userFiles = new Queue<string>();
                var formFiles = new Queue<string>();

                foreach (var include in includes.GetChildren())
                {
                    var type = include["Type"];
                    var file = include["File"];
                    switch (type)
                    {
                        case "User":
                            userFiles.Enqueue(file);
                            break;
                    }
                }

                while (userFiles.Count > 0) await LoadIncludedUsersAsync(userFiles.Dequeue(), serviceProvider);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "配置初始化出错。");
            }
        }

        private static async Task LoadIncludedUsersAsync(string? file, IServiceProvider serviceProvider)
        {
            var accountManager = serviceProvider.GetRequiredService<AccountManager>();
            if (file is null|| !File.Exists(file)) return;
            foreach (var u in new ChoCSVReader<ManagedGeneratingUser>(Path.GetFullPath(file)).WithFirstLineHeader())
                await accountManager.CreateAsync(u);
        }
    }
}