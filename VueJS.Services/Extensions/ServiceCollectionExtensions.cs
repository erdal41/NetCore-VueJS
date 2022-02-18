using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VueJS.Data.Abstract;
using VueJS.Data.Concrete;
using VueJS.Data.Concrete.EntityFramework.Contexts;
using VueJS.Entities.Concrete;
using VueJS.Services.Abstract;
using VueJS.Services.Concrete;

namespace VueJS.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WebAppContext>(options => options.UseSqlServer(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            services.AddIdentity<User, Role>().AddDefaultTokenProviders().AddEntityFrameworkStores<WebAppContext>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISettingService, SettingManager>();
            services.AddScoped<IPostService, PostManager>();
            services.AddScoped<IUploadService, UploadManager>();
            services.AddScoped<ITermService, TermManager>();
            services.AddScoped<ISeoService, SeoManager>();
            services.AddScoped<IUrlRedirectService, UrlRedirectManager>();
            services.AddScoped<IMailService, MailManager>();           

            return services;
        }
    }
}