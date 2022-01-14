using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VueJS.Data.Abstract;
using VueJS.Data.Concrete;
using VueJS.Data.Concrete.EntityFramework.Contexts;
using VueJS.Entities.Concrete;
using VueJS.Services.Abstract;
using VueJS.Services.Concrete;
using System;

namespace VueJS.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<WebAppContext>(options => options.UseSqlServer(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            serviceCollection.AddIdentity<User, Role>(options =>
            {
                // User Password Options
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                // User Username and Email Options
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+$";
                options.User.RequireUniqueEmail = true;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<WebAppContext>();
            serviceCollection.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(15);
            });
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ISettingService, SettingManager>();
            serviceCollection.AddScoped<IPostService, PostManager>();
            serviceCollection.AddScoped<IUploadService, UploadManager>();
            serviceCollection.AddScoped<ITermService, TermManager>();
            serviceCollection.AddScoped<ISeoService, SeoManager>();
            serviceCollection.AddScoped<IUrlRedirectService, UrlRedirectManager>();
            serviceCollection.AddScoped<IMailService, MailManager>();

            return serviceCollection;
        }
    }
}