using Coravel;
using Microsoft.AspNetCore.Components.Authorization;
using Spark.Library.Auth;
using Spark.Library.Database;
using Spark.Library.Logging;
using Spark.Library.Mail;
using StockSimulator.Application.Database;
using StockSimulator.Application.Events.Listeners;
using StockSimulator.Application.Models;
using StockSimulator.Application.Services;
using StockSimulator.Application.Services.Auth;
using StockSimulator.Application.Tasks;

namespace StockSimulator.Application.Startup
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddCustomServices();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDatabase<DatabaseContext>(config);
            services.AddLogger(config);
            services.AddAuthorization(config, new string[] { CustomRoles.Admin, CustomRoles.User });
            services.AddAuthentication<IAuthValidator>(config);
            services.AddScoped<AuthenticationStateProvider, SparkAuthenticationStateProvider>();
            services.AddTaskServices();
            services.AddScheduler();
            services.AddQueue();
            services.AddEventServices();
            services.AddEvents();
            services.AddMailer(config);
            return services;
        }

        private static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            // add custom services
            services.AddScoped<UsersService>();
            services.AddScoped<RolesService>();
            services.AddScoped<IAuthValidator, AuthValidator>();
            services.AddScoped<AuthService>();
            services.AddScoped<StockService>();
            services.AddHttpClient();
            return services;
        }

        private static IServiceCollection AddEventServices(this IServiceCollection services)
        {
            // add custom events here
            services.AddTransient<EmailNewUser>();
            return services;
        }

        private static IServiceCollection AddTaskServices(this IServiceCollection services)
        {
            // add custom background tasks here
            services.AddTransient<GetBareStocksTask>();
            return services;
        }
    }
}