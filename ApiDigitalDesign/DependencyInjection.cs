using ApiDigitalDesign.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection
            services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();
            services.AddScoped<SessionService>();
            services.AddScoped<AttachService>();
            services.AddScoped<PostService>();
            services.AddScoped<LinkGeneratorService>();
            services.AddScoped<LikeService>();
            return services;
        }
    }
}
