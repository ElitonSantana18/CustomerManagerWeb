using CustomerManagerWeb.Services.Interfaces;
using CustomerManagerWeb.Services;

namespace CustomerManagerWeb
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ISessionService, SessionService>();
        }
    }
}
